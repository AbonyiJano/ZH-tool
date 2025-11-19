using System.ComponentModel.DataAnnotations.Schema;

namespace ZH_tool.Models
{
    public class GeneraltZh
    {
        public int Id { get; set; }

        // Külső kulcs a ZH táblára (melyik ZH-ból generáltuk)
        public int ParentZhId { get; set; }
        [ForeignKey("ParentZhId")]
        public Zh? ParentZh { get; set; } // Navigációs property

        // A generált tartalom (JSON string, amit a Gemini ad vissza)
        public string GeneratedJson { get; set; } = string.Empty;

        public DateTime GenerationTime { get; set; } = DateTime.UtcNow;

        public ICollection<Feladat>? Feladatok { get; set; }
    }
}
