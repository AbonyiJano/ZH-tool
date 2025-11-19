namespace ZH_tool.DTOs
{
    public class MegoldasResponseDto
    {
        public int Id { get; set; }
        public string HallgatoNeptunkod { get; set; } = string.Empty;
        public int GeneraltZhId { get; set; }
        public string BekuldottMegoldas { get; set; } = string.Empty;
        public DateTime BekuldesDatuma { get; set; }
    }
}
