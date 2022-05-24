using System;
using TeamZ.Code.Game.Characters.Hedgehog;
using TeamZ.Code.Game.Characters.Lizard;

namespace TeamZ.Characters
{
    public class CharacterDescriptor
    {
        public string AssetPath { get; set; }
        public Type ControllerType { get; set; }
        public string Name { get; set; }
    }

    public static class CharactersList
    {
        public static CharacterDescriptor Lizard { get; }
            = new CharacterDescriptor
            {
                Name = nameof(Lizard),
                AssetPath = "Lizard",
                ControllerType = typeof(LizardController)
            };

        public static CharacterDescriptor Hedgehog { get; }
            = new CharacterDescriptor
            {
                Name = nameof(Hedgehog),
                AssetPath = "Hedgehog",
                ControllerType = typeof(HedgehogController)
            };

        public static CharacterDescriptor[] Descriptors { get; }
            = new []
            {
                Lizard,
                Hedgehog
            };
    }
}
