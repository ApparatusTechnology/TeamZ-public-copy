using System.Linq;
using System.Threading.Tasks;
using TeamZ.AI.Core;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Navigation;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.AI.States
{
    public class Move : AIMindState
    {
        private Dependency<NavigationService> navigationService;
        private readonly Vector3 targetPosition;

        public Move(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public async override void Activate(AIAgent agent)
        {
            var path = this.navigationService.Value
                .CalculatePath(agent.transform.position, this.targetPosition)
                .ToArray();

            await agent.Mover.MoveAsync(path);
            
            this.Next(new Idle());
        }
    }
}