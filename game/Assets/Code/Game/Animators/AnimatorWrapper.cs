using Spine;
using TeamZ.Code.Game.Characters;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Assets.Code.Game.Animators
{
    public abstract class AnimatorWrapper : MonoBehaviour
    {
        public enum DamageType
        {
            AcidBottom,
            AcidTop,
            GasOrRadiation, // poisonous gas, radiation or lack of oxygen
            Bullet,
            Melee,
            Explosion,
            Fall
        }

        protected ICharacter character;

        protected virtual void Awake()
        {
            this.character = this.GetComponent<ICharacter>() ?? 
                this.GetComponentInParent<ICharacter>();
        }

        public abstract TrackEntry Idle(bool isLanded = false, TrackEntry prevTrack = null);

        public abstract void EyeBlink();

        public abstract void IdleSpecial();

        public abstract void IdleWithWeight();

        public abstract void Run();

        public abstract void RunSpeed(float value);

        public abstract void Walk();

        public abstract void WalkWithWeight();

        public abstract void Dash();

        public abstract void RunEnd();

        public abstract (UniTask JumpStart, UniTask Completed) JumpStart();

        public abstract void JumpIdle();

        public abstract void JumpEnd();

        public abstract void JumpOverStart();

        public abstract void JumpOverEnd();

        public abstract void Punch();

        public abstract void Kick();

        public abstract void TakeDamage(DamageType type);

        public abstract void Die();

        public abstract void PushButton(bool onTheFloor = false);

        public abstract (UniTask TakeBox, UniTask Completed) Grab(Vector3 grabPosition);

        public abstract TrackEntry ReleaseBox();

        public abstract TrackEntry ThrowBox();

        public abstract void PushBox();

        public abstract void Climbing(bool value);

        public abstract void ClimbingSpeed(float value);

        public abstract TrackEntry GetCurrentTrack(int trackIndex);

        public abstract void SetEmpty(int index = 1);

        public abstract void SetSpeed(float speed);
    }
}
