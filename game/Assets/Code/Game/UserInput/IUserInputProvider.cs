using System;
using System.Collections.Generic;
using UniRx;

namespace TeamZ.Code.Game.UserInput
{
    public interface IUserInputProvider : IDisposable
    {
        HashSet<int> DeviceIds { get; }

        ReactiveProperty<float> Horizontal { get; }
        ReactiveProperty<float> Vertical { get; }

        ReactiveProperty<bool> Jump { get; }

        ReactiveProperty<bool> Kick { get; }
        ReactiveProperty<bool> Punch { get; }
        ReactiveProperty<bool> Activate { get; }
        ReactiveProperty<bool> Start { get; }
        ReactiveProperty<bool> Cancel { get; }

        IDisposable StartMonitoring();

        void StopMonitoring();
    }
}
