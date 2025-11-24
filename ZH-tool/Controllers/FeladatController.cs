using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZH_tool.DTOs;
using ZH_tool.Models;
using ZH_tool.Services;

namespace ZH_tool.Controllers
{

        [ApiController]
        [Route("api/[controller]")] // Pl.: /api/Feladat
        public class FeladatController : ControllerBase
        {
            private readonly IFeladatService _feladatService;
            private readonly IMapper _mapper;

            public FeladatController(IFeladatService feladatService, IMapper mapper)
            {
                _feladatService = feladatService;
                _mapper = mapper;
            }

            // --------------------------------------------------------
            // POST: Új feladat létrehozása
            // --------------------------------------------------------
            /// <summary>
            /// Új feladat mentése az adatbázisba.
            /// </summary>
            [HttpPost]
            [ProducesResponseType(typeof(FeladatDto), (int)HttpStatusCode.Created)] // 201
            [ProducesResponseType((int)HttpStatusCode.BadRequest)] // 400
            public async Task<ActionResult<FeladatDto>> CreateFeladat([FromBody] FeladatDto feladatDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var feladatEntity = _mapper.Map<Feladat>(feladatDto);
                var createdFeladat = await _feladatService.CreateFeladatAsync(feladatEntity);

                var responseDto = _mapper.Map<FeladatDto>(createdFeladat);

                return CreatedAtAction(nameof(GetFeladatById), new { id = responseDto.Id }, responseDto);
            }

            // --------------------------------------------------------
            // GET: Feladat lekérdezése ID alapján
            // --------------------------------------------------------
            /// <summary>
            /// Egy feladat lekérdezése ID alapján.
            /// </summary>
            [HttpGet("{id:int}")]
            [ProducesResponseType(typeof(FeladatDto), (int)HttpStatusCode.OK)] // 200
            [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404
            public async Task<ActionResult<FeladatDto>> GetFeladatById(int id)
            {
                var feladat = await _feladatService.GetFeladatByIdAsync(id);

                if (feladat == null)
                {
                    return NotFound($"Feladat a megadott azonosítóval ({id}) nem található.");
                }

                return Ok(_mapper.Map<FeladatDto>(feladat));
            }

            // --------------------------------------------------------
            // GET: Összes feladat lekérdezése
            // --------------------------------------------------------
            /// <summary>
            /// Összes feladat lekérdezése.
            /// </summary>
            [HttpGet]
            [ProducesResponseType(typeof(IEnumerable<FeladatDto>), (int)HttpStatusCode.OK)] // 200
            public async Task<ActionResult<IEnumerable<FeladatDto>>> GetAllFeladat()
            {
                var feladatok = await _feladatService.GetAllFeladatAsync();
                return Ok(_mapper.Map<IEnumerable<FeladatDto>>(feladatok));
            }

            // --------------------------------------------------------
            // PUT: Feladat frissítése
            // --------------------------------------------------------
            /// <summary>
            /// Egy feladat adatainak frissítése ID alapján.
            /// </summary>
            [HttpPut("{id:int}")]
            [ProducesResponseType(typeof(FeladatDto), (int)HttpStatusCode.OK)] // 200
            [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404
            public async Task<ActionResult<FeladatDto>> UpdateFeladat(int id, [FromBody] FeladatDto feladatDto)
            {
                if (!ModelState.IsValid || id != feladatDto.Id)
                {
                    // Ha az ID nem egyezik a DTO-ban lévő ID-val
                    return BadRequest("Az URL-ben lévő ID és a DTO ID-ja nem egyezik, vagy az adatok érvénytelenek.");
                }

                var feladatEntity = _mapper.Map<Feladat>(feladatDto);
                var updatedFeladat = await _feladatService.UpdateFeladatAsync(feladatEntity);

                if (updatedFeladat == null)
                {
                    return NotFound($"Feladat a megadott azonosítóval ({id}) nem található a frissítéshez.");
                }

                return Ok(_mapper.Map<FeladatDto>(updatedFeladat));
            }

            // --------------------------------------------------------
            // DELETE: Feladat törlése
            // --------------------------------------------------------
            /// <summary>
            /// Egy feladat törlése ID alapján.
            /// </summary>
            [HttpDelete("{id:int}")]
            [ProducesResponseType((int)HttpStatusCode.NoContent)] // 204
            [ProducesResponseType((int)HttpStatusCode.NotFound)] // 404
            public async Task<ActionResult> DeleteFeladat(int id)
            {
                var success = await _feladatService.DeleteFeladatAsync(id);

                if (!success)
                {
                    return NotFound($"Feladat a megadott azonosítóval ({id}) nem található a törléshez.");
                }

                return NoContent(); // 204 No Content
            }
        /// <summary>
        /// Feladatok lekérése Zh alapján.
        /// </summary>
        [HttpGet("generaltzh/{generaltZhId:int}")]
        [ProducesResponseType(typeof(IEnumerable<FeladatDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<FeladatDto>>> GetFeladatokByGeneraltZhId(int generaltZhId)
        {
            var feladatok = await _feladatService.GetFeladatokByGeneraltZhIdAsync(generaltZhId);

            return Ok(_mapper.Map<IEnumerable<FeladatDto>>(feladatok));
        }
    }
}
