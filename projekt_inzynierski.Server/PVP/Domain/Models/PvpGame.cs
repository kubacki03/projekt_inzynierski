namespace projekt_inzynierski.Server.PVP.Domain.Models
{
    public record PvpGame(int Id, string Technology, string Level);

    public record PvpGameDto(int Id, string Technology, string Level, int UsersInQueue);
}
