using System.Text.Json;
using ZH_tool.Models;
using ZH_tool.Repository;

namespace ZH_tool.Services
{
    public class MegoldasService : IMegoldasService
    {
        private readonly IRepository<Megoldas> _megoldasRepository;
        private readonly IRepository<GeneraltZh> _generaltZhRepository;
        private readonly IRepository<Hallgato> _hallgatoRepository;
        private readonly IRepository<Ertekeles> _ertekelesRepository;
        private readonly IFeladatRepository _feladatRepository;
        private readonly IGeminiService _geminiService;

        public MegoldasService(IRepository<Hallgato> hallgatoRepository,
            IRepository<Megoldas> megoldasRepository,
            IRepository<GeneraltZh> generaltZhRepository,
            IRepository<Ertekeles> ertekelesRepository,
            IGeminiService geminiService,
            IFeladatRepository feladatRepository)
        {
            _megoldasRepository = megoldasRepository;
            _generaltZhRepository = generaltZhRepository;
            _hallgatoRepository = hallgatoRepository;
            _ertekelesRepository = ertekelesRepository;
            _geminiService = geminiService;
            _feladatRepository = feladatRepository;
        }
        /// <summary>
        /// Megoldás beküldése. Ellenőrzi, hogy a hivatkozott hallgató és generált ZH léteznek-e.
        /// </summary>
        public async Task<Megoldas?> SubmitMegoldasAsync(Megoldas megoldas)
        {
            // 1. Validáció: Létezik-e a Hallgató?
            var hallgato = await ((HallgatoRepository)_hallgatoRepository).GetByIdAsync(megoldas.HallgatoNeptunkod);
            if (hallgato == null)
            {
                // Nincs ilyen hallgató, nem lehet megoldást beküldeni
                return null;
            }

            // 2. Validáció: Létezik-e a Generált ZH?
            var generaltZh = await _generaltZhRepository.GetByIdAsync(megoldas.GeneraltZhId);
            if (generaltZh == null)
            {
                // Nincs ilyen ZH, nem lehet megoldást beküldeni
                return null;
            }
            var createdMegoldas = await _megoldasRepository.AddAsync(megoldas);
            await GradeMegoldasAsync(megoldas);
            // 3. Mentés
            return createdMegoldas;
        }
        /// <summary>
        /// Megoldás lekérdezése ID alapján.
        /// </summary>
        public async Task<Megoldas?> GetMegoldasByIdAsync(int id)
        {
            return await _megoldasRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// LLM segítségével pontozza a beküldött megoldást, és menti az értékelést.
        /// </summary>
        public async Task<Ertekeles?> GradeMegoldasAsync(Megoldas megoldas)
        {
            var megoldasId = megoldas.Id;
            // 2. Lekérjük a Generált ZH-t a prompt tartalmához
            var generaltZh = await _generaltZhRepository.GetByIdAsync(megoldas.GeneraltZhId);
            if (generaltZh == null) return null;

            var feladatok = await _feladatRepository.GetByGeneraltZhIdAsync(megoldas.GeneraltZhId);
            var pontozasLeiras = string.Join("\n\n", feladatok.Select(f => f.Pontozas));
            var zhLeiras = string.Join("\n\n", feladatok.Select(f => f.Leiras));
            var mintaMegoldas = string.Join("\n\n", feladatok.Select(f => f.MintaMegoldas));
            // 3. Prompt Készítése
            var hallgatoMegoldas = megoldas.BekuldottMegoldas;

            var prompt = $@"
Te egy felsőoktatási oktatási asszisztens vagy, aki programozási kurzusokhoz beadott ZH megoldásokat automatikusan értékel.
A célod: a hallgatói megoldásokat **részletesen, objektíven és kompetenciaalapon** értékelni a megadott pontozási szempontok és mintamegoldás alapján.

Feladataid:
1. Elemezd a megadott **ZH-feladatokat**, hogy megértsd, mit kellett megoldani.
2. Hasonlítsd össze a **hallgatói megoldást** a **mintamegoldással**.
3. Alkalmazd a megadott **pontozási szempontokat** (részpontszámok, kritériumok).
4. Adj részletes **értékelő szöveget** minden főbb szempont mellé.
5. Számítsd ki az **összesített pontszámot**.
6. Az összponthoz add össze a megadott feladatoknak a max pontjait.
7. Az elért pontot úgy kapjuk meg, hogy összeadod mindegyik feladatra kapott pontot.
8. Az összegző értékelésben add meg, hogy melyik feladatra mennyi pontot kapott a hallgató és, hogy miért. Ez legyen részletes.

A válasz formátuma mindig JSON, az alábbi struktúrában:
{{
  ""osszpont"": 0,
  ""elert_pont"": 0,
  ""osszegzo_ertekeles"": """",
}}

---

INPUT PARAMÉTEREK:

[ZÁRTHELYI FELADATOK]
A hallgató által megoldandó feladat szövege:
{zhLeiras}

[MINTAMEGOLDÁS]
A tanár által megadott, helyes megoldás:
{mintaMegoldas}

[PONTOZÁSI SZEMPONTOK]
Az értékelés alapját képező objektív kritériumok és részpontszámok:
{pontozasLeiras}

[HALLGATÓ MEGOLDÁSA]
A hallgató által adott megoldás:
{hallgatoMegoldas}

---

INSTRUKCIÓ:
A fenti paraméterek alapján **értékeld a [HALLGATÓ MEGOLDÁSÁT]**. A válasz formátuma szigorúan a megadott JSON struktúra. Az ""osszegzo_ertekeles"" mezőben részletesen indokold, hogy a hallgató **melyik feladatra (cím/sorszám alapján) mennyi pontot kapott és miért**.
";

            string response = await _geminiService.CallGeminiAPI(prompt);
            int elertPont = -1;
            int osszPont = -1;
            string osszegzoErtekeles = string.Empty;
            try
            {
                // 🟢 JSON FELDOLGOZÁS: JsonDocument használata DTO nélkül
                using (JsonDocument document = JsonDocument.Parse(response))
                {
                    JsonElement root = document.RootElement;
                    
                    // 3. Értékek kinyerése
                    if (root.TryGetProperty("osszpont", out JsonElement osszPontElement) &&
                    osszPontElement.ValueKind == JsonValueKind.Number)
                    {
                        osszPont = osszPontElement.GetInt32();
                    }
                    // "elert_pont" kinyerése (int-ként)
                    if (root.TryGetProperty("elert_pont", out JsonElement elertPontElement) &&
                        elertPontElement.ValueKind == JsonValueKind.Number)
                    {
                        elertPont = elertPontElement.GetInt32();
                    }

                    // "osszegzo_ertekeles" kinyerése (string-ként)
                    if (root.TryGetProperty("osszegzo_ertekeles", out JsonElement osszegzoErtekelesElement) &&
                        osszegzoErtekelesElement.ValueKind == JsonValueKind.String)
                    {
                        osszegzoErtekeles = osszegzoErtekelesElement.GetString() ?? string.Empty;
                    }

                    // Megjegyzés: Az "osszpont" mezőt nem mentjük külön az Ertekeles táblába, 
                    // de itt kinyerhetnénk, ha szükség lenne rá.
                }

                if (elertPont == 0 && string.IsNullOrEmpty(osszegzoErtekeles))
                {
                    // A JSON valid volt, de a kulcsok értéke üres vagy nulla (valószínűleg hibás válasz)
                    return null;
                }
            }
            catch (JsonException ex)
            {
                // Hiba kezelése, ha a JSON formátuma hibás
                Console.WriteLine($"[Hiba] Értékelés JSON parszolási hiba (MegoldasId: {megoldasId}): {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                // Általános hiba
                Console.WriteLine($"[Hiba] Ismeretlen hiba a parszoláskor (MegoldasId: {megoldasId}): {ex.Message}");
                return null;
            }
            // 5. Mentés az Értékelések táblába
            var newErtekeles = new Ertekeles
            {
                MegoldasId = megoldasId,
                Pontszam = elertPont,
                OsszPontszam = osszPont,
                LLMVisszajelzes = osszegzoErtekeles,
                ErtekelesDatuma = DateTime.UtcNow
            };

            // 6. Mentés az Értékelés Repository segítségével
            return await _ertekelesRepository.AddAsync(newErtekeles);
        }
    }
}
