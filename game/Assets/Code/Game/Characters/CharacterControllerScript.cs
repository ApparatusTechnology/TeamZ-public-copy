using System;
using System.Linq;
using TeamZ.Assets.Code.Game.Animators;
using TeamZ.Characters.MovementHandlers;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Activation.Core;
using TeamZ.Code.Game.Boosters;
using TeamZ.Code.Game.Boosters.Mutagen;
using TeamZ.Code.Game.ComboAttacks;
using TeamZ.Code.Game.Inventory;
using TeamZ.Code.Game.Levels;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using TeamZ.Code.Helpers.Extentions;
using TeamZ.GameSaving;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Game.Characters
{
    public class CharacterControllerScript : MonoBehaviourWithState<CharacterControllerState>
    {
        public Transform GroundCheck;
        public Transform ClimbCheck;

        public Transform Punch;
        public float PunchRadius;

        public Transform Kick;
        public float KickRadius;

        public UserInputProvider UserInputProvider;

        public LayerMask SurfaceForClimbing;
        public LayerMask WalkLayerMask;

        public enum Direction
        {
            Empty,
            Left = 1,
            Right = -1,
            Up,
            Down
        }

        public ReactiveProperty<bool> UserInputEnabled
            = new ReactiveProperty<bool>(true);

        protected ReactiveProperty<bool> CanClimb
            = new ReactiveProperty<bool>();

        protected ReactiveProperty<bool> IsClimbed
            = new ReactiveProperty<bool>();

        public ReactiveProperty<Direction> HorizontalDirection
            = new ReactiveProperty<Direction>(Direction.Empty);

        public ReactiveProperty<Direction> VerticalDirection
            = new ReactiveProperty<Direction>(Direction.Up);

        public float ClimbRadius = 0.4f;

        [HideInInspector]
        public CollisionIgnorer PlatformCollision;

        [HideInInspector]
        public SpineAnimator Animator;

        [HideInInspector]
        public Rigidbody2D RigidBody;

        [HideInInspector]
        public ICharacter Character;

        public ReactiveProperty<FightMode> Attacks = new ReactiveProperty<FightMode>(FightMode.None);

        private bool loadingStarted;

        private int[] activeLayersToInteraction = { 9, 10, 13, 14 }; // level object, enemy, character1, character2

        private int impulseDirection = 1;

        public ReactiveProperty<float> HorizontalValue
            = new ReactiveProperty<float>();

        public ReactiveProperty<float> VerticalValue
            = new ReactiveProperty<float>();

        public ReactiveProperty<bool> CanWalk
            = new ReactiveProperty<bool>();

        protected ClimbingSurface climbingSurface = null;

        public ReactiveProperty<MovementHandler> MovementHandler
            = new ReactiveProperty<MovementHandler>();

        public float GroundRadius;

        public ComboAttackHandler ComboAttackHandler { get; set; }

        // Use this for initialization
        protected virtual void Start()
        {
            this.ComboAttackHandler = this.CreateComboAttackHandler();
            this.ComboAttackHandler.Init(this);
            
            MessageBroker.Default.Receive<CharacterDead>()
                .Subscribe(o =>
                {
                    if (this.GetComponent<ICharacter>() != o.Character)
                    {
                        return;
                    }

                    this.MovementHandler.Value.Next(new DeathHandler());
                    this.MovementHandler.Value.Dispose();
                })
                .AddTo(this);

            this.RigidBody = this.GetComponent<Rigidbody2D>();
            this.Animator = this.GetComponentInChildren<SpineAnimator>();
            this.Animator.Steps
                .Subscribe(o => MessageBroker.Default.Publish(StepHappened.Default))
                .AddTo(this);
            
            this.PlatformCollision = this.GetComponentInChildren<CollisionIgnorer>();

            this.UserInputProvider.Horizontal
                .Subscribe(o => this.HorizontalValue.Value = o)
                .AddTo(this);

            this.UserInputProvider.Vertical
                .Subscribe(o => this.VerticalValue.Value = o)
                .AddTo(this);


            this.HorizontalDirection
                .Subscribe(o => this.Flip())
                .AddTo(this);

            this.MovementHandler
                .Skip(1)
                .Subscribe(newHandler =>
                {
                    newHandler.OnNext
                        .Subscribe(nextHandler => this.MovementHandler.Value = nextHandler)
                        .AddTo(this);

                    newHandler.Init(this);
                })
                .AddTo(this);

            this.MovementHandler.Value = new WalkHandler();

            this.UserInputEnabled
                .Subscribe(value =>
                {
                    if (value)
                    {
                        this.MovementHandler.Value.Next(new WalkHandler());
                    }
                    else
                    {
                        this.MovementHandler.Value.Next(new FreezeHandler());
                    }
                })
                .AddTo(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<FirstAidKit>() != null)
            {
                if (!this.Character.IsLifeFull())
                {
                    MessageBroker.Default.Publish(new TakeObjectHappened());
                    // TODO: add effect of flying aid kit to health bar on HUD

                    collider.gameObject.Destroy();
                }
            }

            if (collider.gameObject.GetComponent<ArmorKit>() != null)
            {
                MessageBroker.Default.Publish(new TakeObjectHappened());
                // TODO: add effect of flying armor kit to armor bar on HUD
                collider.gameObject.Destroy();
            }

            if (collider.gameObject.GetComponent<MutagenCapsule>() != null)
            {
                MessageBroker.Default.Publish(new TakeObjectHappened());
                // TODO: add effect of flying mutagen capsule to mutagen bar on HUD
                // start mutagen timer
                // get 2 random boosters
                // open mutagen popup, choose one booster
                // apply selected mutagen
                collider.gameObject.Destroy();
            }

            if (collider.gameObject.GetComponent<IInventoryItem>() != null)
            {
                MessageBroker.Default.Publish(new TakeObjectHappened());
                InventoryManager.AddInventory(collider.gameObject.GetComponent<IInventoryItem>());
                // TODO: add effect of flying access card to the inventory on HUD
                collider.gameObject.Destroy();
            }

            if (collider.gameObject.GetComponent<AbyssCollider>() != null)
            {
                // Something strange happening with this OnTriggerEnter
                // It called OnTriggerEnter several times when it ought to only one
                if (this.loadingStarted)
                {
                    return;
                }

                this.loadingStarted = true;

                Dependency<GameController>.Resolve().LoadLastSavedGameAsync().Forget();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (this.WalkLayerMask.Contains(collision.gameObject.layer))
            {
                this.CanWalk.Value = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (this.WalkLayerMask.Contains(collision.gameObject.layer))
            {
                this.CanWalk.Value = false;
            }
        }

        private void Flip()
        {
            var sign = Mathf.Sign(this.HorizontalValue.Value);
            Vector3 currentScale = this.transform.localScale;

            currentScale.x = sign * Mathf.Abs(currentScale.x);
            this.impulseDirection = (int)sign * Mathf.Abs(this.impulseDirection);
            this.transform.localScale = currentScale;
        }

        public async void Attack()
        {
            switch (this.Attacks.Value)
            {
                case FightMode.Punch:
                    MessageBroker.Default.Publish(new PunchHappened());
                    await UniTask.Delay(200);
                    Fight2D.Action(this.Punch.position, this.PunchRadius, this.activeLayersToInteraction, false,
                        this.Character.PunchDamage, this.Character.PunchImpulse * this.impulseDirection);
                    break;
                case FightMode.Kick:
                    MessageBroker.Default.Publish(new KickHappened());
                    await UniTask.Delay(200);
                    Fight2D.Action(this.Kick.position, this.KickRadius, this.activeLayersToInteraction, false,
                        this.Character.KickDamage, this.Character.KickImpulse * this.impulseDirection);
                    break;
                default:
                    break;
            }

            this.Attacks.Value = FightMode.None;
        }

        public void AlertObservers(string message)
        {
            if (message.Equals("AttackAnimationEnded"))
            {
                switch (this.Attacks.Value)
                {
                    case FightMode.Punch:
                        Fight2D.Action(this.Punch.position, this.PunchRadius, this.activeLayersToInteraction, false,
                            this.Character.PunchDamage, this.Character.PunchImpulse * this.impulseDirection);
                        break;
                    case FightMode.Kick:
                        Fight2D.Action(this.Kick.position, this.KickRadius, this.activeLayersToInteraction, false,
                            this.Character.KickDamage, this.Character.KickImpulse * this.impulseDirection);
                        break;
                    default:
                        break;
                }

                this.Attacks.Value = FightMode.None;
            }

            if (message.Equals("PunchHappened"))
            {
                MessageBroker.Default.Publish(new PunchHappened());
            }

            if (message.Equals("KickHappened"))
            {
                MessageBroker.Default.Publish(new KickHappened());
            }
        }

        public override CharacterControllerState GetState()
            => new CharacterControllerState
            {
                IsClimbed = this.IsClimbed.Value,
            };

        public override void SetState(CharacterControllerState state)
        {
            this.HorizontalDirection.Value = Direction.Empty;
            this.IsClimbed.Value = state.IsClimbed;
            this.CanClimb.Value = state.IsClimbed;
        }

        public virtual ComboAttackHandler CreateComboAttackHandler()
            => new ComboAttackHandler();

        private void OnDestroy()
        {
            Debug.Log("Character destroy");
            this.MovementHandler.Value?.Dispose();
        }
    }

    public enum FightMode
    {
        None = -1,
        Punch = 0,
        Kick,
        TailHit,
        HullHit
    }

    public class RunHappened
    {
        public bool isClimbing = false;

        public RunHappened(bool _isClimbing)
        {
            this.isClimbing = _isClimbing;
        }
    }

    public class RunEnded
    {
        public bool isClimbing = false;

        public RunEnded(bool _isClimbing)
        {
            this.isClimbing = _isClimbing;
        }
    }

    public class JumpHappened
    {
        public JumpHappened()
        {
        }
    }

    public class LandingHappened
    {
        public LandingHappened()
        {
        }
    }

    public class PunchHappened
    {
        public PunchHappened()
        {
        }
    }

    public class KickHappened
    {
        public KickHappened()
        {
        }
    }

    public class TakeObjectHappened
    {
        public TakeObjectHappened()
        {
        }
    }

    public class ActivationHappened
    {
        public ActivationHappened()
        {
        }
    }
}