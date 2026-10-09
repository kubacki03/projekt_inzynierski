namespace projekt_inzynierski.Server.PVP.Domain.Models
{
    public record PvpQuestion(int Id, string Text, string[] Options, int CorrectIndex);
}
