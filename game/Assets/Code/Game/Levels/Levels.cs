using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamZ.Code.Game.Levels
{
    public class Level
    {
        public const string TEST = "Test";
        public const string LABORATORY = "Laboratory";
        public const string LABORATORY2 = "Laboratory2";

        static Level()
        {
            Test = new Level
            {
                Name = TEST,
                Scene = TEST,
                Id = Guid.Parse("089F511E-593C-460C-AEC6-36A0135DA960")
            };  
            
            Laboratory = new Level
            {
                Name = LABORATORY,
                Scene = LABORATORY,
                Id = Guid.Parse("F2B428DA-4C1E-4761-83F8-A6998F08B72F")
            };

            Laboratory2 = new Level
            {
                Name = LABORATORY2,
                Scene = LABORATORY2,
                Id = Guid.Parse("F19EE6AB-A9D2-4AF1-98EA-BD1D09CDE6E2")
            };

            Core = new Level
            {
                Name = "Core",
                Scene = "Core",
                Id = Guid.Parse("F09EE6AB-A9D2-4AF1-98EA-BD1D09CDE6E2")
            };

            All = new[]
            {
                Core,
                Test,
                Laboratory,
                Laboratory2
            }.ToDictionary(o => o.Name);

            AllById = All.Values
                .ToDictionary(o => o.Id);
        }


        public string Name;
        public string Scene;
        public Guid Id;

        public static Level Test { get; }
        public static Level Core { get; }
        public static Level Laboratory { get; }
        public static Level Laboratory2 { get; }
        public static Dictionary<string, Level> All { get; }
        public static Dictionary<Guid, Level> AllById { get; }
    }
}
