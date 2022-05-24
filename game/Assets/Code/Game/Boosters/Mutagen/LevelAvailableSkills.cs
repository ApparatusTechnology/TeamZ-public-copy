using System;
using System.Collections.Generic;
using TeamZ.Code.Game.Boosters.Mutagen.MutagenBoosters;

namespace TeamZ.Code.Game.Boosters.Mutagen
{
    public enum LizardAvailableSkills
    {
        SPEED,
        POWER,
        AGILITY,
        DISGUISE,
        THERMAL_VISION,
        ELECTRIC_RESIST,
        FIRE_RESIST
    };

    public enum HedgehogAvailableSkills
    {
        SPEED,
        POWER,
        RAGE,
        NIGHT_VISION,
        PATH_FINDING,
        TOXIC_RESIST,
        RADIATION_RESIST
    };

    public static class LevelAvailableSkills
    {
        public static IMutagenBooster GetLizardBoosterByType(LizardAvailableSkills type)
        {
            switch(type)
            {
                case LizardAvailableSkills.SPEED:
                    return new MutagenSpeedBooster();
                case LizardAvailableSkills.POWER:
                    return new MutagenPowerBooster();
                case LizardAvailableSkills.AGILITY:
                    return new MutagenAgilityBooster();
                case LizardAvailableSkills.DISGUISE:
                    return new MutagenDisguiseBooster();
                case LizardAvailableSkills.THERMAL_VISION:
                    return new MutagenThermalVisionBooster();
                case LizardAvailableSkills.FIRE_RESIST:
                    return new MutagenFireResist();
                case LizardAvailableSkills.ELECTRIC_RESIST:
                    return new MutagenElectricResist();
                default:
                    throw new NotSupportedException();
            }
        }

        public static IMutagenBooster GetHedgehogBoosterByType(HedgehogAvailableSkills type)
        {
            switch (type)
            {
                case HedgehogAvailableSkills.SPEED:
                    return new MutagenSpeedBooster();
                case HedgehogAvailableSkills.POWER:
                    return new MutagenPowerBooster();
                case HedgehogAvailableSkills.RAGE:
                    return new MutagenRageBooster();
                case HedgehogAvailableSkills.NIGHT_VISION:
                    return new MutagenNightVisionBooster();
                case HedgehogAvailableSkills.PATH_FINDING:
                    return new MutagenFootmarkSearchBooster();
                case HedgehogAvailableSkills.TOXIC_RESIST:
                    return new MutagenToxicResist();
                case HedgehogAvailableSkills.RADIATION_RESIST:
                    return new MutagenRadiationResist();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}