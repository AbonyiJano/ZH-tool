namespace ZH_tool.Services
{
    public interface IGeminiService
    {
        Task<string> CallGeminiAPI(string prompt);
    }
}
