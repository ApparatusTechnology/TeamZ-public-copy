using System;
using Code.Helpers.Extensions;
using Spine;
using Spine.Unity;
using System.Timers;
using TeamZ.Code.Game.Grab;
using UniRx;
using UniRx.Async;
using UnityEngine;
using Event = Spine.Event;

namespace TeamZ.Assets.Code.Game.Animators
{
    public static class AnimationEvents
    {
        public const string ON_TAKE_BOX_STARTED = "on_take_box_started";
        public const string ON_BOX_THROW = "on_box_throw";
        public const string ON_BOX_RELEASED = "on_box_released";

        public const string ON_JUMP_STARTED = "on_jump_started";
        public const string ON_STEP_HAPPENED = "on_step_happened";
    }

    public class SpineAnimator : AnimatorWrapper
    {
        private const string IDLE = "actual/idle";
        private const string EYE_BLINK = "actual/blink";
        private const string IDLE_SPECIAL = "actual/idle_action";
        private const string IDLE_WITH_BOX = "actual/idle_box";
        private const string RUN = "actual/run";
        private const string WALK = "actual/walk";
        private const string DASH = "actual/dash";
        private const string RUN_END = "actual/run_end";
        private const string JUMP_START = "actual/jump_start";
        private const string JUMP_IDLE = "actual/jump_idle";
        private const string JUMP_END = "actual/jump_end";
        private const string JUMP_OVER_START = "actual/jump_over1-2";
        private const string JUMP_OVER_END = "actual/jump_over2-2";
        private const string LIGHT_PUNCH1 = "actual/light_punch";
        private const string LIGHT_PUNCH2 = "actual/strong punch";
        private const string STRONG_PUNCH1 = "actual/rocket punch";
        private const string STRONG_PUNCH2 = "actual/elbow_punch";
        private const string KICK = "actual/goal_kick";
        private const string TAKE_DAMAGE1 = "actual/takeDamage_acid_bot";
        private const string TAKE_DAMAGE2 = "actual/takeDamage_acid_top";
        private const string DIE = "actual/die";
        private const string PUSH_BUTTON = "actual/button";
        private const string PUSH_BUTTON_LEG = "actual/button_bot";
        private const string TAKE_BOX = "actual/take_box";
        private const string CARRY_BOX = "actual/walk_weight";
        private const string RELEASE_BOX = "actual/relese_box";
        private const string THROW_BOX = "actual/trow_box";
        private const string PUSH_BOX = "actual/push";
        private const string STAIRS_UP = "actual/idle";
        private const string STAIRS_IDLE = "actual/idle";

        private SkeletonAnimation animator;

        // WARNING: GOVNOKOD!
        // TODO: REMOVE
        private int PUNCH_COUNT = 0;
        private int LIGHT_PUNCH_COUNT = 0;
        // WARNING: GOVNOKOD!

        private GrabBoneMarker grabBone;

        private Timer idleTimer = null;
        private const double SPECIAL_IDLE_DELAY = 5000.0;

        public IObservable<Event> Steps { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            this.animator = this.GetComponent<SkeletonAnimation>();
            this.grabBone = this.animator.GetComponentInChildren<GrabBoneMarker>();
            this.animator.Initialize(false);

            this.Steps = this.animator.ToEventStream(AnimationEvents.ON_STEP_HAPPENED);
        }

        public override TrackEntry Idle(bool isLanded = false, TrackEntry prevTrack = null)
        {
            TrackEntry currentTrack = prevTrack;

            if (isLanded)
            {
                currentTrack = this.animator.state.SetAnimation(0, JUMP_END, false);
                currentTrack.TrackEnd = currentTrack.Animation.Duration;
                currentTrack.End += Jump_End;
            }
            else
            {
                currentTrack = this.animator.state.AddAnimation(0, IDLE, true, 0);
                currentTrack.Start += Idle_Started;
                currentTrack.End += Idle_Ended;
            }

            return currentTrack;
        }

        public override void EyeBlink()
        {
            this.animator.state.SetAnimation(1, EYE_BLINK, false);
        }

        public override void IdleSpecial()
        {
            if (this.animator.state.GetCurrent(0)?.Animation.Name == IDLE)
            {
                this.animator.state.SetAnimation(0, IDLE_SPECIAL, false).Complete += IdleSpecial_Complete;
            }
        }

        public override void IdleWithWeight()
        {
            this.animator.state.SetAnimation(0, IDLE_WITH_BOX, true);
        }

        public override void Run()
        {
            this.SetEmpty(0);
            this.animator.state.SetAnimation(0, RUN, true).TimeScale = this.character.RunSpeed / 10;
        }

        public override void RunSpeed(float speed)
        {
            this.SetEmpty(0);
            this.animator.state.SetAnimation(0, RUN, true).TimeScale = speed;
        }

        public override void Walk()
        {
            this.SetEmpty(0);
            this.animator.state.SetAnimation(0, WALK, true).TimeScale = this.character.CreepSpeed;
        }

        public override void WalkWithWeight()
        {
            var trackEntry = this.animator.state.SetAnimation(0, CARRY_BOX, true);
            trackEntry.TimeScale = this.character.CreepSpeed / 3;
        }

        public override void Dash()
        {
            this.SetEmpty(0);
            var track = this.animator.state.SetAnimation(0, DASH, false);
            track.TimeScale = 0.5f;
            track.TrackEnd = track.Animation.Duration;
            track.Complete += Dash_End;
        }

        public override void RunEnd()
        {
            this.animator.state.SetAnimation(0, RUN_END, false);
        }

        public override (UniTask JumpStart, UniTask Completed) JumpStart()
        {
            var track = this.animator.state.SetAnimation(0, JUMP_START, false);
            return (track.OnEvent(AnimationEvents.ON_JUMP_STARTED), track.OnCompleted());
        }

        public override void JumpIdle()
        {
            this.animator.state.SetAnimation(0, JUMP_IDLE, true);
        }

        public override void JumpEnd()
        {
            this.animator.state.SetAnimation(0, JUMP_END, false).TimeScale = 1.5f;
        }

        public override void JumpOverStart()
        {
            this.animator.state.SetAnimation(0, JUMP_OVER_START, false);
        }

        public override void JumpOverEnd()
        {
            this.animator.state.SetAnimation(0, JUMP_OVER_END, false);
        }

        // WARNING: GOVNOKOD!
        // TODO: REMOVE
        // this should be reworked. 
        // strong punch and elbow punch animations should trigger by combo attacks  
        public override void Punch()
        {
            this.SetEmpty(0);
            TrackEntry track = null;

            if (PUNCH_COUNT >= 2)
            {
                var rnd = new System.Random();
                int punch = rnd.Next(0, 2);

                if (punch == 0)
                {
                    track = this.animator.state.SetAnimation(1, STRONG_PUNCH1, false);
                }
                else
                {
                    track = this.animator.state.SetAnimation(1, STRONG_PUNCH2, false);
                }

                track.TimeScale = 1.5f;

                PUNCH_COUNT = 0;
                LIGHT_PUNCH_COUNT = 0;
            }
            else
            {
                if (LIGHT_PUNCH_COUNT > 0)
                {
                    track = this.animator.state.SetAnimation(1, LIGHT_PUNCH2, false);
                    track.TimeScale = 2.0f;
                }
                else
                {
                    track = this.animator.state.SetAnimation(1, LIGHT_PUNCH1, false);
                    track.TimeScale = 1.5f;
                }

                PUNCH_COUNT++;
                LIGHT_PUNCH_COUNT++;
            }

            track.Complete += Punch_Complete;
        }

        public override void Kick()
        {
            this.SetEmpty(0);

            TrackEntry track = this.animator.state.SetAnimation(1, KICK, false);

            track.Complete += Punch_Complete;
        }

        public override void TakeDamage(DamageType type)
        {
            switch (type)
            {
                case DamageType.AcidBottom:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE1, false);
                    break;
                case DamageType.AcidTop:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE2, false);
                    break;
                case DamageType.GasOrRadiation:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE2, false);
                    break;
                case DamageType.Bullet:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE1, false);
                    break;
                case DamageType.Melee:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE1, false);
                    break;
                case DamageType.Explosion:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE1, false);
                    break;
                case DamageType.Fall:
                    this.animator.state.SetAnimation(0, TAKE_DAMAGE1, false);
                    break;
                default:
                    break;
            }
        }

        public override void Die()
        {
            this.animator.state.SetAnimation(0, DIE, false);
        }

        public override void PushButton(bool onTheFloor = false)
        {
            this.SetEmpty(0);

            TrackEntry track = null;

            if (onTheFloor)
            {
                this.animator.state.SetAnimation(1, PUSH_BUTTON_LEG, false);
            }
            else
            {
                this.animator.state.SetAnimation(1, PUSH_BUTTON, false);
            }

            track.TimeScale = 3.0f;
            track.Complete += Activate_Complete;
        }

        public override (UniTask TakeBox, UniTask Completed) Grab(Vector3 grabPosition)
        {
            this.grabBone.transform.position = grabPosition + this.grabBone.GrabOffset;
            var track = this.animator.state.SetAnimation(0, TAKE_BOX, false);
            return (track.OnEvent(AnimationEvents.ON_TAKE_BOX_STARTED), track.OnCompleted());
        }

        public override TrackEntry ReleaseBox()
        {
            return this.animator.state.SetAnimation(0, RELEASE_BOX, false);
        }

        public override TrackEntry ThrowBox()
        {
            var track = this.animator.state.SetAnimation(0, THROW_BOX, false);
            return track;
        }

        public override void PushBox()
        {
            this.SetEmpty(0);
            this.animator.state.SetAnimation(0, PUSH_BOX, true).TimeScale = this.character.CreepSpeed;
        }

        public override void Climbing(bool value)
        {
            if (value)
            {
                this.animator.state.SetAnimation(0, STAIRS_UP, true);
            }
        }

        public override void ClimbingSpeed(float speed)
        {
            if (speed > 0)
            {
                if (this.animator.state.GetCurrent(0).Animation.Name != STAIRS_UP)
                {
                    this.animator.state.SetAnimation(0, STAIRS_UP, true);
                }
            }
            else
            {
                if (this.animator.state.GetCurrent(0).Animation.Name != STAIRS_IDLE)
                {
                    this.animator.state.SetAnimation(0, STAIRS_IDLE, true);
                }
            }
        }

        public override TrackEntry GetCurrentTrack(int trackIndex)
        {
            return this.animator.state.GetCurrent(trackIndex);
        }

        public override void SetEmpty(int index = 1)
        {
            this.animator.state.SetEmptyAnimation(index, 0);
        }

        public override void SetSpeed(float speed)
        {
            this.animator.state.TimeScale = speed;
        }

        private void Punch_Complete(TrackEntry trackEntry)
        {
            trackEntry.Complete -= Punch_Complete;
            trackEntry = this.animator.state.SetEmptyAnimation(1, -1f);
            this.Idle(false, trackEntry);
        }

        private void Activate_Complete(TrackEntry trackEntry)
        {
            trackEntry.Complete -= Activate_Complete;
        }

        private void Jump_End(TrackEntry trackEntry)
        {
            trackEntry.End -= Jump_End;
            trackEntry = this.Idle(false, trackEntry);
            trackEntry.MixDuration = 1.5f;
        }

        private void Dash_End(TrackEntry trackEntry)
        {
            trackEntry.Complete -= Dash_End;
            trackEntry = this.animator.state.SetAnimation(0, RUN_END, false);
            trackEntry.TimeScale = 1.8f;
            trackEntry.MixDuration = 0.4f;
        }

        private void Idle_Started(TrackEntry trackEntry)
        {
            trackEntry.Start -= Idle_Started;

            // first launch after Idle_Ended 
            if (this.idleTimer == null)
            {
                var rand = new System.Random();
                float randSecs = rand.Next(1, 4);   // 1, 2 or 3 sec

                this.idleTimer = new Timer(SPECIAL_IDLE_DELAY + (randSecs * 2 * 1000.0));
                this.idleTimer.Elapsed += WaitingInIdle_Elapsed;
                this.idleTimer.Start();
            }
        }

        private void Idle_Ended(TrackEntry trackEntry)
        {
            trackEntry.End -= Idle_Ended;

            // stop and delete SPECIAL_IDLE timer if we're switching to the non idle animation
            if (this.animator.state.GetCurrent(0)?.Animation.Name != IDLE_SPECIAL && 
                this.idleTimer != null)
            {
                this.idleTimer.Elapsed -= WaitingInIdle_Elapsed;
                this.idleTimer.Stop();
                this.idleTimer = null;
            }
        }

        private void IdleSpecial_Complete(TrackEntry trackEntry)
        {
            trackEntry.Complete -= IdleSpecial_Complete;

            // while we're inside the Idle animation loop
            if (this.idleTimer != null)
            {
                var rand = new System.Random();
                float randSecs = rand.Next(1, 4);   // 1, 2 or 3 sec

                this.idleTimer.Interval = (SPECIAL_IDLE_DELAY * 2) + (randSecs * 2 * 1000.0);
            }

            this.Idle(false, trackEntry);
        }

        private void WaitingInIdle_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.animator.state.GetCurrent(0)?.Animation.Name == IDLE)
            {
                this.IdleSpecial();
            }
        }
    }

    public class StepHappened
    {
        public static StepHappened Default { get; } = new StepHappened();
    }
}