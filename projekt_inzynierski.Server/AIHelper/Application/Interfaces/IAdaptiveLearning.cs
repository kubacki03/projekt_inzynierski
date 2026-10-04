namespace projekt_inzynierski.Server.AIHelper.Application.Interfaces
{
    public interface IAdaptiveLearning
    {
        public Task<bool> CanBeCreated(string language, string knowledge);
    }
}
