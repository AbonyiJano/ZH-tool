namespace ZH_tool.DTOs
{
    public class ErtekelesResponseDto
    {
        public int Id { get; set; }
        public int MegoldasId { get; set; }
        public int? Pontszam { get; set; }
        public int? OsszPontszam { get; set; }
        public string LLMVisszajelzes { get; set; } = string.Empty;
        public DateTime ErtekelesDatuma { get; set; }
        public string HallgatoNeptunkod { get; set; } = string.Empty;
    }
}
