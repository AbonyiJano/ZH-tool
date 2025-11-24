using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZH_tool.Models
{
    public class Megoldas
    {
        public int Id { get; set; }

        // Külső kulcs a Hallgato táblához
        [Required]
        [StringLength(6)]
        [Column(TypeName = "varchar(6)")]
        public string HallgatoNeptunkod { get; set; } = string.Empty;
        [ForeignKey("HallgatoNeptunkod")]
        public Hallgato? Hallgato { get; set; }

        // Külső kulcs a GeneraltZh táblához (melyik feladatsorhoz tartozik a megoldás)
        public int GeneraltZhId { get; set; }
        [ForeignKey("GeneraltZhId")]
        public GeneraltZh? GeneraltZh { get; set; }

        // A diák megoldása (pl. egy JSON string a kódjával, vagy a válasz szövege)
        public string BekuldottMegoldas { get; set; } = string.Empty;

        public DateTime BekuldesDatuma { get; set; } = DateTime.UtcNow;

        public Ertekeles? Ertekeles { get; set; }
    }
}
