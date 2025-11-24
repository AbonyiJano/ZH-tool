using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZH_tool.DTOs;
using ZH_tool.Models;
using ZH_tool.Services;

namespace ZH_tool.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ErtekelesController : ControllerBase
    {
        private readonly IErtekelesService _ertekelesService;
        private readonly IMapper _mapper;

        public ErtekelesController(IErtekelesService ertekelesService, IMapper mapper)
        {
            _ertekelesService = ertekelesService;
            _mapper = mapper;
        }

        // 🟢 1. ÖSSZES ÉRTÉKELÉS VISSZAADÁSA
        // GET /api/Ertekeles
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ErtekelesResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ErtekelesResponseDto>>> GetAllErtekelesek()
        {
            var ertekelesek = await _ertekelesService.GetAllErtekelesAsync();

            // Mappeljük az entitásokat DTO-kra a külső kommunikációhoz
            return Ok(_mapper.Map<IEnumerable<ErtekelesResponseDto>>(ertekelesek));
        }

        // 🟢 2. ÉRTÉKELÉS VISSZAADÁSA ID ALAPJÁN
        // GET /api/Ertekeles/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ErtekelesResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ErtekelesResponseDto>> GetErtekelesById(int id)
        {
            var ertekeles = await _ertekelesService.GetErtekelesByIdAsync(id);

            if (ertekeles == null)
            {
                return NotFound($"Értékelés a megadott ID-val ({id}) nem található.");
            }

            return Ok(_mapper.Map<ErtekelesResponseDto>(ertekeles));
        }

        // 🟢 3. ÉRTÉKELÉS FRISSÍTÉSE
        // PUT /api/Ertekeles/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ErtekelesResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ErtekelesResponseDto>> UpdateErtekeles(int id, [FromBody] ErtekelesResponseDto ertekelesDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ertekelesDto.Id)
            {
                return BadRequest("Az ID nem egyezik meg.");
            }

            var ertekelesEntity = _mapper.Map<Ertekeles>(ertekelesDto);
            var updatedErtekeles = await _ertekelesService.UpdateErtekelesAsync(ertekelesEntity);

            if (updatedErtekeles == null)
            {
                return NotFound($"Értékelés a megadott ID-val ({id}) nem található.");
            }

            return Ok(_mapper.Map<ErtekelesResponseDto>(updatedErtekeles));
        }
    }
}
