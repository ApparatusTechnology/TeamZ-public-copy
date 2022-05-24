using TeamZ.AI.Core;
using UnityEngine;

namespace TeamZ.AI.States
{
    public class Idle : AIMindState
    {
        public override void Activate(AIAgent agent)
        {
            Debug.Log($"Agent {agent.name} is idle.");
        }
    }
}