using TeamZ.AI.Core;
using UnityEngine;

namespace TeamZ.AI.States
{
    public class Attack : AIMindState
    {
        private readonly GameObject target;

        public Attack(GameObject target)
        {
            this.target = target;
        }
        
        public override void Activate(AIAgent agent)
        {
            
        }
    }
}