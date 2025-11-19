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
    public class HallgatoController : ControllerBase
    {
        private readonly IHallgatoService _hallgatoService;
        private readonly IMapper _mapper;

        public HallgatoController(IHallgatoService hallgatoService, IMapper mapper)
        {
            _hallgatoService = hallgatoService;
            _mapper = mapper;
        }
        /// <summary>
        /// Új hallgató regisztrálása (beküldés a Neptunkóddal és névvel).
        /// </summary>
        /// <param name="hallgatoDto">A hallgató adatai (Neptunkod, Nev).</param>
        /// <returns>A regisztrált hallgató adatait tartalmazó DTO.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(HallgatoDto), (int)HttpStatusCode.Created)] // 201
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)] // 400
        public async Task<ActionResult<HallgatoDto>> CreateHallgato([FromBody] HallgatoDto hallgatoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. DTO -> Entitás
            var hallgatoEntity = _mapper.Map<Hallgato>(hallgatoDto);

            try
            {
                // 2. Service hívás (a Service kezeli az egyediségi ellenőrzést is)
                var createdHallgato = await _hallgatoService.CreateHallgatoAsync(hallgatoEntity);

                // 3. Entitás -> Response DTO
                var responseDto = _mapper.Map<HallgatoDto>(createdHallgato);

                // 4. 201 Created válasz
                // CreatedAtAction hivatkozik a GET metódusra
                return CreatedAtAction(nameof(GetHallgatoByNeptun), new { neptunkod = responseDto.Neptunkod }, responseDto);
            }
            catch (ArgumentException ex)
            {
                // Ha a Service-ben az egyediségi ellenőrzés hibát dob
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Egy hallgató adatainak lekérdezése Neptunkód alapján.
        /// </summary>
        /// <param name="neptunkod">A hallgató egyedi Neptunkódja.</param>
        /// <returns>A hallgató adatait tartalmazó DTO.</returns>
        [HttpGet("{neptunkod}")]
        [ProducesResponseType(typeof(HallgatoDto), (int)HttpStatusCode.OK)] // 200
        [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404
        public async Task<ActionResult<HallgatoDto>> GetHallgatoByNeptun(string neptunkod)
        {
            var hallgato = await _hallgatoService.GetHallgatoByNeptunAsync(neptunkod);

            if (hallgato == null)
            {
                return NotFound($"Hallgató a megadott Neptunkóddal ({neptunkod}) nem található.");
            }

            return Ok(_mapper.Map<HallgatoDto>(hallgato));
        }
    }
}
