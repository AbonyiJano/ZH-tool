using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZH_tool.Models
{
    public class Hallgato
    {
        [Key]
        [StringLength(6)]
        [Column(TypeName = "varchar(6)")]
        public string Neptunkod { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nev { get; set; } = string.Empty;

        public ICollection<Megoldas>? Megoldasok { get; set; }
    }
}
