namespace ZH_tool.DTOs
{
    public class GeneraltZhResponseDto
    {
        public int Id { get; set; }
        public int ParentZhId { get; set; }
        public string GeneratedJson { get; set; } // Itt van a generált JSON
        public DateTime GenerationTime { get; set; }
    }
}
