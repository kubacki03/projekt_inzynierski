using projekt_inzynierski.Server.Achievments.Domain.Models;
using System.Text.Json;

namespace projekt_inzynierski.Server.Achievments.Application.Rules
{
    public static class RuleHelpers
    {
        public static T? ReadConfig<T>(Achievement a) =>
            string.IsNullOrWhiteSpace(a.RuleConfigJson) ? default :
            JsonSerializer.Deserialize<T>(a.RuleConfigJson);
    }
}
