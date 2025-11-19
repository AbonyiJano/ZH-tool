using System.ComponentModel.DataAnnotations;

namespace ZH_tool.DTOs
{
    public class CreateZhDto
    {
        [Required(ErrorMessage = "A ZH név megadása kötelező.")]
        [StringLength(100, ErrorMessage = "A név nem lehet hosszabb 100 karakternél.")]
        public string Nev { get; set; }
        public string MintaZh { get; set; }

        public string Tematika { get; set; }
        public string TemakorLeiras { get; set; }

        [Range(1, 100, ErrorMessage = "A feladatok száma 1 és 100 között lehet.")]
        public int FeladatokSzama { get; set; }

        public string ProgramozasiNyelv { get; set; }
        public string Nehezseg { get; set; }
    }
}
