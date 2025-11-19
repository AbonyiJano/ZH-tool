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
    public class MegoldasController : ControllerBase
    {
        private readonly IMegoldasService _megoldasService;
        private readonly IMapper _mapper;

        public MegoldasController(IMegoldasService megoldasService, IMapper mapper)
        {
            _megoldasService = megoldasService;
            _mapper = mapper;
        }
    
        /// <summary>
        /// Egy hallgató beküldi egy generált ZH megoldását.
        /// </summary>
        /// <param name="megoldasDto">A beküldött megoldás adatai (Neptunkód, GeneraltZhId, BekuldottMegoldas).</param>
        /// <returns>A mentett megoldás adatait tartalmazó DTO.</returns>
        [HttpPost("submit-megoldas")]
        [ProducesResponseType(typeof(MegoldasResponseDto), (int)HttpStatusCode.Created)] // 201
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)] // 400
        [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404 (Ha a ZH vagy a Hallgató nem létezik)
        public async Task<ActionResult<MegoldasResponseDto>> SubmitMegoldas([FromBody] MegoldasInputDto megoldasDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. DTO -> Entitás
            var megoldasEntity = _mapper.Map<Megoldas>(megoldasDto);

            // 2. Service hívás (A Service ellenőrzi a külső kulcsokat és ment)
            var createdMegoldas = await _megoldasService.SubmitMegoldasAsync(megoldasEntity);

            if (createdMegoldas == null)
            {
                // A Service null-t ad vissza, ha a Hallgato vagy a GeneraltZh nem létezik
                return NotFound("A beküldés sikertelen: a hivatkozott Hallgató vagy Generált ZH nem található.");
            }

            // 3. Entitás -> Response DTO
            var responseDto = _mapper.Map<MegoldasResponseDto>(createdMegoldas);

            // 4. 201 Created válasz
            return CreatedAtAction(
                nameof(GetMegoldasById),
                new { id = responseDto.Id },
                responseDto
            );
        }
        /// <summary>
        /// Egy adott megoldás lekérdezése ID alapján. (A CreatedAtAction célja)
        /// </summary>
        /// <param name="id">A megoldás egyedi azonosítója.</param>
        /// <returns>A megoldás adatait tartalmazó DTO.</returns>
        [HttpGet("megoldas/{id:int}")]
        [ProducesResponseType(typeof(MegoldasResponseDto), (int)HttpStatusCode.OK)] // 200
        [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404
        public async Task<ActionResult<MegoldasResponseDto>> GetMegoldasById(int id)
        {
            // Lekérés a Service rétegen keresztül
            var megoldas = await _megoldasService.GetMegoldasByIdAsync(id);

            if (megoldas == null)
            {
                return NotFound($"Megoldás a megadott azonosítóval ({id}) nem található.");
            }

            return Ok(_mapper.Map<MegoldasResponseDto>(megoldas));
        }
    }
}
