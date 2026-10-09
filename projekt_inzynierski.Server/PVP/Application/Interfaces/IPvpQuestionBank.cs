using projekt_inzynierski.Server.PVP.Domain.Models;

namespace projekt_inzynierski.Server.PVP.Application.Interfaces
{
    public interface IPvpQuestionBank
    {
        IReadOnlyList<PvpQuestion> Pick(int gameId, int count);
    }
}
