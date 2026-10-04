namespace projekt_inzynierski.Server.AIHelper.Application.Interfaces
{
    public interface IChatAssistant
    {
         Task<string> ChatResponse(string message);
    }
}
