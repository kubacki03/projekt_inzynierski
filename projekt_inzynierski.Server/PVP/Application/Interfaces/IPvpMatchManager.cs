using projekt_inzynierski.Server.PVP.Application.DTOs;

namespace projekt_inzynierski.Server.PVP.Application.Interfaces
{
    public interface IPvpMatchManager
    {
        bool Join(string sessionId, string userId);
        PvpStateDto? GetState(string sessionId, string userId);
        void SubmitAnswer(string sessionId, string userId, int questionIndex, int answerIndex);
        Task ReportFocusLostAsync(string sessionId, string userId);
    }
}
