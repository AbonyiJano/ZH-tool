namespace ZH_tool.Configurations
{
    public static class GeminiOptions
    {
        public const string Gemini = "GeminiSettings";
    }

    public class GeminiSettings
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
    }
}
