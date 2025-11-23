using System.ComponentModel.DataAnnotations;

namespace ZH_tool.DTOs
{
    public class FeladatDto
    {
        // Az Id-t csak lekérdezéskor adjuk vissza
        public int? Id { get; set; }

        public int GeneraltZhId { get; set; }

        [Required]
        [StringLength(200)]
        public string FeladatCime { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Temakor { get; set; } = string.Empty;

        [Required]
        public string Leiras { get; set; } = string.Empty;

        // Lista a kompetenciákhoz
        public List<string> Kompetenciak { get; set; } = new List<string>();

        [Required]
        public string Pontozas { get; set; } = string.Empty;

        public string MintaMegoldas { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NehezsegiSzint { get; set; } = string.Empty;
    }
}
