using System.ComponentModel.DataAnnotations.Schema;

namespace ZH_tool.Models
{
    public class Ertekeles
    {
        public int Id { get; set; }

        // Külső kulcs a Megoldas táblához (melyik beküldött megoldást pontozzuk)
        public int MegoldasId { get; set; }
        [ForeignKey("MegoldasId")]
        public Megoldas? Megoldas { get; set; }

        // Az LLM által adott pontszám (lehet null, ha még nem értékeltük)
        public int? Pontszam { get; set; }

        public int? OsszPontszam { get; set; }

        // Az LLM által adott részletes visszajelzés (JSON vagy string)
        public string LLMVisszajelzes { get; set; } = string.Empty;

        public DateTime ErtekelesDatuma { get; set; } = DateTime.UtcNow;
    }
}
