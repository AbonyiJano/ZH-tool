using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using ZH_tool.Configurations;
using ZH_tool.Models;
using ZH_tool.Repository;

namespace ZH_tool.Services
{
    public class ZhService : IZhService
    {
        private readonly IRepository<Zh> _zhRepository;
        private readonly IRepository<GeneraltZh> _generaltZhRepository;
        private readonly IGeminiService _geminiService;
        private readonly IRepository<Feladat> _feladatRepository;

        public ZhService(IRepository<Zh> zhRepository, IRepository<GeneraltZh> generaltZhRepository, IGeminiService geminiService,
                           IRepository<Feladat> feladatRepository)
        {
            _zhRepository = zhRepository;
            _generaltZhRepository = generaltZhRepository;
            _geminiService = geminiService;
            _feladatRepository = feladatRepository;
        }
        public async Task<IEnumerable<Zh>> ListAllZhkAsync()
        {
            // Itt valósítható meg a komplexebb lekérdezés/szűrés/rendezés, 
            // ami meghaladja a Repository egyszerű CRUD funkcióit
            return await _zhRepository.GetAllAsync();
        }
        public async Task<Zh?> GetZhByIdAsync(int id)
        {
            return await _zhRepository.GetByIdAsync(id);
        }
        public async Task<Zh> CreateZhAsync(Zh zh)
        {
            // Itt lehetne validáció (pl. nehézség csak 1-től 5-ig lehet)
            return await _zhRepository.AddAsync(zh);
        }
        public async Task<Zh?> UpdateZhAsync(Zh zh)
        {
            return await _zhRepository.UpdateAsync(zh);
        }
        public async Task<bool> DeleteZhAsync(int id)
        {
            return await _zhRepository.DeleteAsync(id);
        }
        public async Task<GeneraltZh?> GenerateZhContentAsync(int parentZhId)
        {
            // 1. Keresd meg a ZHt a megadott ID alapján
            var parentZh = await _zhRepository.GetByIdAsync(parentZhId);
            if (parentZh == null)
            {
                return null;
            }

            // 2. Készítsd el a Promptot CSUPÁN ennek a ZH-nak az adatai alapján
            var prompt= $@"Te egy felsőoktatási oktatási asszisztens vagy, aki programozási kurzusokhoz automatikusan generál ZH (zárthelyi) feladatokat.

Feladatod: a megadott kurzusadatok alapján új, a kurzushoz illeszkedő, értékelhető és kompetenciamérő ZH-feladatokat készíteni.

A ZH-feladatok legyenek:
- tematikusan illeszkedők a megadott témakörökhöz,
- formailag konzisztensen felépítettek a mintafeladatok alapján,
- kompetencia-alapúak, azaz konkrét tanulási kimenetet mérjenek,
- pontozhatók és egyértelműek
- mintafeladatokhoz nagyon hasonlóak
- pontozásnál add meg azt is, hogy mire kap pontot a hallgató

A válasz formátuma mindig JSON, az alábbi struktúrában:
{{
  ""feladatcime"": """",
  ""temakor"": """",
  ""leiras"": """",
  ""kompetenciak"": [],
  ""pontozas"": """",
  ""nehezsegiszint"": """"
}}

---

TÁRGYTEMATIKA:
{parentZh.Tematika}

MINTA ZÁRTHELYI FELADATOK:
{parentZh.MintaZH}

TÉMAKÖRLEÍRÁSOK:
{parentZh.TemakorLeiras}

FELHASZNÁLÓI BEÁLLÍTÁSOK:
- Feladatok száma: {parentZh.FeladatokSzama}
- Nehézség: {parentZh.Nehezseg}, könnyű: kezdő programozó szintje(frissen felvett Bsc programtervező informatikus), közepes:másodéves Bsc  programtervező informatikus hallgató, nehéz: Msc vagy végzős Bsc  programtervező informatikus hallgató szintje, a nehézség illeszkedjen a mintafeladatok nehézségéhez is
- Programozási nyelv: {parentZh.ProgramozasiNyelv}

---

Generálj {parentZh.ProgramozasiNyelv} darab új ZH-feladatot JSON-tömb formájában. Csak a JSON-t add vissza, más szöveget ne!";


            var generatedJson = await _geminiService.CallGeminiAPI(prompt);

            var newGeneraltZh = new GeneraltZh
            {
                ParentZhId = parentZhId, 
                GeneratedJson = generatedJson,
                GenerationTime = DateTime.UtcNow
            };

            var createdGeneraltZh = await _generaltZhRepository.AddAsync(newGeneraltZh);
            try

            {
                // 4. JSON Deszerializálás List<FeladatDto>-ra (vagy közvetlenül List<Feladat>-ra, ha a mapping egyszerű)

                // Először definiáld egy DTO-ban, vagy használjuk az entitást, ha a JSON mezőnevei megegyeznek:
                // Ha a JSON mezőnevek kisbetűsek (pl. "feladat_cime"), 
                // a System.Text.Json használatakor szükség lehet a PropertyNamingPolicy-ra, vagy attribútumokra.

                // FELTÉTELEZÉS: A JSON mezőnevek megegyeznek a C# tulajdonságokkal (CamelCase vagy SnakeCase beállítás)

                var feladatLista = System.Text.Json.JsonSerializer.Deserialize<List<Feladat>>(
                    generatedJson,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                        // Ha a JSON snake_case (pl. feladat_cime) a C# CamelCase (FeladatCime) ellenére, 
                        // akkor JsonNamingPolicy-t kell használni, de a PropertyNameCaseInsensitive gyakran elég
                    }
                );

                if (feladatLista != null && feladatLista.Any())
                {
                    // 5. Feladatok mentése a Feladat táblába
                    foreach (var feladat in feladatLista)
                    {
                        // Beállítjuk a Generált ZH külső kulcsát minden egyes feladaton
                        feladat.GeneraltZhId = createdGeneraltZh.Id;

                        // Mivel az IRepository<Feladat> AddAsync() metódusa egy entitást vár,
                        // és egyenként menteni kell, az alábbi hívás a megfelelő:
                        await _feladatRepository.AddAsync(feladat);
                    }
                }
            }
            catch (System.Text.Json.JsonException ex)
            {
                // Hibakezelés, ha a Gemini nem érvényes JSON-t ad vissza
                Console.WriteLine($"Hiba a ZH parszolásakor: {ex.Message}");
                // Ekkor a GeneraltZh már mentve van, de a Feladatok nincsenek. 
                // Dönthetsz a GeneraltZh törlése mellett, vagy megtarthatod a hibás JSON-nal is.
            }

            return createdGeneraltZh;



        }
        public async Task<GeneraltZh?> GetGeneratedZhByIdAsync(int id)
        {
            return await _generaltZhRepository.GetByIdAsync(id);
        }
        public async Task<GeneraltZh?> UpdateGeneratedZhAsync(GeneraltZh zh)
        {
            return await _generaltZhRepository.UpdateAsync(zh);
        }
        public async Task<bool> DeleteGeneratedZhAsync(int id)
        {
            return await _generaltZhRepository.DeleteAsync(id);
        }

    }
    
}
