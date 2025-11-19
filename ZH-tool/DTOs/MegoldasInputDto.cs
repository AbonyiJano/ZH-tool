using System.ComponentModel.DataAnnotations;

namespace ZH_tool.DTOs
{
    public class MegoldasInputDto
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string HallgatoNeptunkod { get; set; } = string.Empty;

        [Required]
        public int GeneraltZhId { get; set; }

        [Required]
        public string BekuldottMegoldas { get; set; } = string.Empty;
    }
}
