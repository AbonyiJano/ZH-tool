using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZH_tool.Models
{
    public class Feladat
    {
        public int Id { get; set; }
        public int GeneraltZhId { get; set; }
        [ForeignKey("GeneraltZhId")]
        public GeneraltZh? GeneraltZh { get; set; }

        [Required]
        [StringLength(200)]
        public string FeladatCime { get; set; } = string.Empty; // "feladat_cime"

        [Required]
        [StringLength(250)]
        public string Temakor { get; set; } = string.Empty; // "temakor"

        [Required]
        public string Leiras { get; set; } = string.Empty; // "leiras"

        public List<string> Kompetenciak { get; set; } = new List<string>(); // "kompetenciak"

        [Required]
        public string Pontozas { get; set; } = string.Empty; // "pontozas" (Pl: "10 pont", "részpontozás")

        public string MintaMegoldas { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NehezsegiSzint { get; set; } = string.Empty;
    }
}