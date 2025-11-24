using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZH_tool.DTOs;
using ZH_tool.Models;
using ZH_tool.Repository;
using ZH_tool.Services;

namespace ZH_tool.Controllers
{
    [ApiController] // API Controller
    [Route("api/[controller]")] // Végpont útvonal: /api/Zh
    public class ZhController : ControllerBase
    {
        private readonly IZhService _zhService;
        private readonly IMapper _mapper; // AutoMapper injektálása

        public ZhController(IZhService zhService, IMapper mapper)
        {
            _zhService = zhService;
            _mapper = mapper;
        }
        /// <summary>
        /// Lekérdezi az összes ZH-t az adatbázisból.
        /// </summary>
        // --- GET ALL Végpont ---
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ZhListDto>), 200)] // Kimeneti DTO
        public async Task<ActionResult<IEnumerable<ZhListDto>>> GetAllZhk()
        {
            var zhkEntities = await _zhService.ListAllZhkAsync();

            // Entitás lista átalakítása Listázó DTO-vá
            var zhDtos = _mapper.Map<IEnumerable<ZhListDto>>(zhkEntities);

            return Ok(zhDtos);
        }
        /// <summary>
        /// Lekérdez egy ZH-t azonosító alapján.
        /// </summary>
        /// <param name="id">A ZH azonosítója (Id).</param>
        // --- GET BY ID Végpont ---
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ZhResponseDto), 200)] // Kimeneti DTO
        public async Task<ActionResult<ZhResponseDto>> GetZh(int id)
        {
            var zhEntity = await _zhService.GetZhByIdAsync(id);
            if (zhEntity == null)
            {
                return NotFound();
            }

            // Entitás átalakítása Response DTO-vá
            var responseDto = _mapper.Map<ZhResponseDto>(zhEntity);

            return Ok(responseDto);
        }
        /// <summary>
        /// Új ZH-t vesz fel az adatbázisba.
        /// </summary>
        /// <param name="zh">A felvenni kívánt ZH adatai.</param>
        // --- POST (CREATE) Végpont ---
        [HttpPost]
        [ProducesResponseType(typeof(ZhResponseDto), 201)] // Bemeneti és kimeneti DTO
        [ProducesResponseType(400)]
        public async Task<ActionResult<ZhResponseDto>> CreateZh([FromBody] CreateZhDto zhDto)
        {
            // 1. DTO validációja (az [Required] és [Range] attribútumok miatt)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. Bemeneti DTO átalakítása ZH entitássá
            var zhEntity = _mapper.Map<Zh>(zhDto);

            // 3. Service meghívása az entitással
            var createdEntity = await _zhService.CreateZhAsync(zhEntity);

            // 4. Létrejött entitás átalakítása Response DTO-vá
            var responseDto = _mapper.Map<ZhResponseDto>(createdEntity);

            return CreatedAtAction(nameof(GetZh), new { id = responseDto.Id }, responseDto);
        }
        // --- PUT (UPDATE) Végpont (DTO-val módosítva) ---
        /// <summary>
        /// Frissíti egy létező ZH adatait.
        /// </summary>
        /// <param name="id">A frissítendő ZH azonosítója (Id).</param>
        /// <param name="zhDto">A ZH új adatai.</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ZhResponseDto), 200)] // Kimeneti DTO
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ZhResponseDto>> UpdateZh(int id, [FromBody] CreateZhDto zhDto) // Bemeneti DTO-t várunk!
        {
            // 1. Validáció (DTO-n beállított szabályok alapján)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. Keresd meg a frissítendő Entitást az adatbázisban
            var existingZh = await _zhService.GetZhByIdAsync(id);
            if (existingZh == null)
            {
                return NotFound($"A megadott azonosítóval ({id}) nem található ZH.");
            }

            _mapper.Map(zhDto, existingZh);

            existingZh.Id = id;

            // 4. Frissítés a Service rétegen keresztül
            var updatedEntity = await _zhService.UpdateZhAsync(existingZh);

            // 5. Kimeneti DTO-vá alakítás és visszatérés
            var responseDto = _mapper.Map<ZhResponseDto>(updatedEntity);

            return Ok(responseDto);
        }
        // --- DELETE Végpont (DTO-val módosítva) ---
        /// <summary>
        /// Töröl egy ZH-t azonosító alapján.
        /// </summary>
        /// <param name="id">A törlendő ZH azonosítója (Id).</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)] 
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteZh(int id)
        {
            var success = await _zhService.DeleteZhAsync(id);

            if (!success)
            {
                return NotFound($"A megadott azonosítóval ({id}) nem található ZH a törléshez.");
            }
            return NoContent(); 
        }
        // --- POST (GENERATE) Végpont ---
        /// <summary>
        /// Generál egy feladatsort egy adott ZH paraméterei alapján (Gemini hívás).
        /// </summary>
        /// <param name="parentZhId">Annak a ZH-nak az azonosítója, amely alapján generálni kell.</param>
        [HttpPost("{parentZhId:int}/generalas")] // Itt kapjuk meg az ID-t
        [ProducesResponseType(typeof(GeneraltZhResponseDto), 201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GeneraltZhResponseDto>> GenerateZh(int parentZhId)
        {
            // A Service metódus most csak ezt az egy ID-t dolgozza fel
            var generatedEntity = await _zhService.GenerateZhContentAsync(parentZhId);

            if (generatedEntity == null)
            {
                return NotFound($"A megadott ZH ({parentZhId}) nem található, így nem generálható tartalom.");
            }

            // ... (Mapping és visszatérés, mint korábban) ...
            var responseDto = _mapper.Map<GeneraltZhResponseDto>(generatedEntity);

            return CreatedAtAction(nameof(GetGeneratedZh), new { id = responseDto.Id }, responseDto);
        }
        /// <summary>
        /// Lekérdezi az összes generált ZH-t.
        /// </summary>
        [HttpGet("generalt")]
        [ProducesResponseType(typeof(IEnumerable<GeneraltZhResponseDto>), 200)]
        public async Task<ActionResult<IEnumerable<GeneraltZhResponseDto>>> GetAllGeneratedZhk()
        {
            var generatedZhk = await _zhService.ListAllGeneratedZhkAsync();
            return Ok(_mapper.Map<IEnumerable<GeneraltZhResponseDto>>(generatedZhk));
        }
        /// <summary>
        /// Lekérdezi egy korábban generált ZH adatait azonosító alapján.
        /// </summary>
        /// <param name="id">A generált ZH azonosítója.</param>
        /// <returns>A generált ZH adatait tartalmazó objektum, benne a generált JSON-nal.</returns>
        [HttpGet("generalt/{id:int}")]
        [ProducesResponseType(typeof(GeneraltZhResponseDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GeneraltZhResponseDto>> GetGeneratedZh(int id)
        {
            // Lekérés a GeneraltZh Repository-ból
            var generatedZh = await _zhService.GetGeneratedZhByIdAsync(id);

            if (generatedZh == null)
            {
                return NotFound();
            }

            // Mapping: Entitásból kimeneti DTO-vá alakítás
            var responseDto = _mapper.Map<GeneraltZhResponseDto>(generatedZh);

            return Ok(responseDto);
        }
        /// <summary>
        /// Töröl egy generált ZH-t azonosító alapján.
        /// </summary>
        [HttpDelete("generalt/{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteGeneratedZh(int id)
        {
            // 🟢 Helyes mód: Törlés a Service rétegen keresztül
            var success = await _zhService.DeleteGeneratedZhAsync(id);

            if (!success)
            {
                return NotFound($"A megadott azonosítóval ({id}) nem található generált ZH a törléshez.");
            }

            return NoContent();
        }
    }
}
