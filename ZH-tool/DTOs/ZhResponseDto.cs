namespace ZH_tool.DTOs
{
    public class ZhResponseDto
    {
        public int Id { get; set; } // Az ID-t is visszaadjuk
        public string Nev { get; set; }
        public string MintaZh { get; set; }
        public string Tematika { get; set; }
        public string TemakorLeiras { get; set; }
        public int FeladatokSzama { get; set; }
        public string ProgramozasiNyelv { get; set; }
        public string Nehezseg { get; set; }
    }
}
