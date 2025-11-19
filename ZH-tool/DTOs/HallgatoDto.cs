using System.ComponentModel.DataAnnotations;

namespace ZH_tool.DTOs
{
    public class HallgatoDto
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Neptunkod { get; set; } = string.Empty;

        [Required]
        public string Nev { get; set; } = string.Empty;
    }
}
