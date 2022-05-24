using System;

namespace TeamZ.GameSaving.States
{
    public class GeneratorState : ActivatorState
    {
        public Guid DoorId { get; set; }

        public Guid PortalId { get; set; }
    }
}