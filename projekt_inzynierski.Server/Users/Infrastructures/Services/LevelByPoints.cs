using projekt_inzynierski.Server.Users.Application.DTOs;

namespace projekt_inzynierski.Server.Users.Infrastructures.Services
{
    public static class LevelByPoints
    {
        public static int BeginnerPoints = 0;
        public static int IntermediatePoints = 500;
        public static int AdvancedPoints = 1000;
        public static int ExpertPoints = 5000;

        public static LevelDto GetLevelByPoints(long points)
        {
            if (points < IntermediatePoints)
            {
                return new LevelDto
                {
                    Level = Levels.Beginner,
                    MinimumPoints = BeginnerPoints,
                    MaximumPoints = IntermediatePoints - 1,
                    TotalPoints = points
                };
            }
            if (points < AdvancedPoints)
            {
                return new LevelDto
                {
                    Level = Levels.Intermediate,
                    MinimumPoints = IntermediatePoints,
                    MaximumPoints = AdvancedPoints - 1,
                    TotalPoints = points
                };
            }
            if (points < ExpertPoints)
            {
                return new LevelDto
                {
                    Level = Levels.Advanced,
                    MinimumPoints = AdvancedPoints,
                    MaximumPoints = ExpertPoints - 1,
                    TotalPoints = points
                };
            }

            return new LevelDto
            {
                Level = Levels.Expert,
                MinimumPoints = ExpertPoints,
                MaximumPoints = long.MaxValue,
                TotalPoints = points
            };
        }
    }



}
