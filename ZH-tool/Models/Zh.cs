using System.ComponentModel.DataAnnotations;

namespace ZH_tool.Models
{
    public class Zh
    {
        [Key]
        public int Id { get; set; }

        // A minta zh azonosítója, pl. "Prog1_2025_tavasz"
        [Required]
        public string Nev { get; set; }

        //MintaZH
        public string MintaZH { get; set; }

        // A zh tematikája
        public string Tematika { get; set; }

        // Részletes leírás a témakörökről
        public string TemakorLeiras { get; set; }

        // Hány feladatot tartalmaz a zh
        public int FeladatokSzama { get; set; }

        // Milyen nyelven kell megoldani (pl. C#, Python, Java)
        public string ProgramozasiNyelv { get; set; }

        // A nehézségi szint (pl. Könnyű, Közepes, Nehéz)
        public string Nehezseg { get; set; }

        public ICollection<GeneraltZh>? GeneraltZhk { get; set; }
    }
}
