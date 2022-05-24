using System;
using System.Timers;
using TeamZ.Code.Game.Boosters.Mutagen;
using TeamZ.Code.Mediator;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States.Charaters;
using TeamZ.Helpers;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Characters
{
    public interface ICharacter : IDamageable
    {
        IMutagenBooster MutagenBooster { get; set; }

        string CharacterName { get; set; }

        int Health { get; set; }
        int Armor { get; set; }
        float Stamina { get; set; }
        float RunSpeed { get; set; }
        float CreepSpeed { get; set; }
        float StrikeSpeed { get; set; }
        float JumpSpeed { get; set; }
        float JumpForce { get; set; }
        int PunchDamage { get; set; }
        int KickDamage { get; set; }
        int PunchImpulse { get; }
        int KickImpulse { get; }
        int JumpsAllowed { get; set; }
        int JumpsLeft { get; set; }
        float JumpCooldown { get; set; }
        float JumpStaminaUsage { get; set; }
        bool IsCooldownEnabled { get; set; }
        float AttackCooldown { get; set; }
        float AttackStaminaUsage { get; set; }
        float DashImpulse { get; set; }
        float DashStaminaUsage { get; set; }

        int MakeDamage(FightMode fightMode);
        void TakeHealth(int health);
        void TakeArmor(int armor);
        void ApplyMutagen(IMutagenBooster booster);
        bool IsLifeFull();
    }


    public abstract class Character<TState> : MonoBehaviourWithState<TState>, ICharacter
        where TState : CharacterState
    {
        private const int MAX_HEALTH = 100;

        private const float MAX_STAMINA = 100;

        [SerializeField]
        private int health;

        [SerializeField]
        private int armor;

        [SerializeField]
        private float stamina = MAX_STAMINA;

        [SerializeField]
        private float staminaIncrement = 1.0f;

        [SerializeField]
        private double staminaRecoverySpeed = 200;

        [SerializeField]
        private float runSpeed;

        [SerializeField]
        private float creepSpeed;

        [SerializeField]
        private float strikeSpeed;

        [SerializeField]
        private float jumpSpeed;

        [SerializeField]
        private float jumpHeight;

        [SerializeField]
        private int punchDamage;

        [SerializeField]
        private int kickDamage;

        [SerializeField]
        private bool isCooldownEnabled = true;

        [SerializeField]
        private float attackCooldown = 0;

        [SerializeField]
        private float attackStaminaUsage = 15;

        [SerializeField]
        private string characterName;

        [SerializeField]
        public int jumpsAllowed = 2;

        [SerializeField]
        public int jumpsLeft = 0;

        [SerializeField]
        public float jumpCooldown = 0;

        [SerializeField]
        public float jumpStaminaUsage = 12;

        [SerializeField]
        public float dashImpulse = 140;

        [SerializeField]
        public float dashStaminaUsage = 30;

        [SerializeField]
        private int punchImpulse = 75;

        [SerializeField]
        private int kickImpulse = 100;

        public IMutagenBooster MutagenBooster { get; set; } = null;

        public string CharacterName
        {
            get { return this.characterName; }
            set { this.characterName = value; }
        }

        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        public int Armor
        {
            get { return this.armor; }
            set { this.armor = value; }
        }

        public float Stamina
        {
            get
            {
                return this.stamina;
            }
            set
            {
                if (this.MutagenBooster == null)
                {
                    this.stamina = value;
                }
                else
                {
                    this.stamina = MAX_STAMINA;
                }
            }
        }

        public float StaminaIncrement
        {
            get { return this.staminaIncrement; }
            set { this.staminaIncrement = value; }
        }

        public double StaminaRecoverySpeed
        {
            get { return this.staminaRecoverySpeed; }
            set { this.staminaRecoverySpeed = value; }
        }

        public float RunSpeed
        {
            get { return this.runSpeed; }
            set { this.runSpeed = value; }
        }

        public float CreepSpeed
        {
            get { return this.creepSpeed; }
            set { this.creepSpeed = value; }
        }

        public float StrikeSpeed
        {
            get { return this.strikeSpeed; }
            set { this.strikeSpeed = value; }
        }

        public float JumpSpeed
        {
            get { return this.jumpSpeed; }
            set { this.jumpSpeed = value; }
        }

        public float JumpForce
        {
            get { return this.jumpHeight; }
            set { this.jumpHeight = value; }
        }

        public float DashImpulse
        {
            get => dashImpulse;
            set => dashImpulse = value;
        }

        public float DashStaminaUsage
        {
            get => dashStaminaUsage;
            set => dashStaminaUsage = value;
        }

        public int PunchDamage
        {
            get { return this.punchDamage; }
            set { this.punchDamage = value; }
        }

        public int KickDamage
        {
            get { return this.kickDamage; }
            set { this.kickDamage = value; }
        }

        public int PunchImpulse
        {
            get => punchImpulse;
            private set => punchImpulse = value;
        }

        public int KickImpulse
        {
            get => kickImpulse;
            private set => kickImpulse = value;
        }

        public bool IsCooldownEnabled
        {
            get { return this.isCooldownEnabled; }
            set { this.isCooldownEnabled = value; }
        }

        public float AttackCooldown
        {
            get { return this.attackCooldown; }
            set { this.attackCooldown = value; }
        }

        public float AttackStaminaUsage
        {
            get { return this.attackStaminaUsage; }
            set { this.attackStaminaUsage = value; }
        }

        public int JumpsLeft
        {
            get => jumpsLeft;
            set => jumpsLeft = value;
        }

        public int JumpsAllowed
        {
            get => jumpsAllowed;
            set => jumpsAllowed = value;
        }

        public float JumpCooldown
        {
            get => jumpCooldown;
            set => jumpCooldown = value;
        }

        public float JumpStaminaUsage
        {
            get { return this.jumpStaminaUsage; }
            set { this.jumpStaminaUsage = value; }
        }

        public override void SetState(TState state)
        {
            this.Armor = state.Armor;
            this.PunchDamage = state.PunchDamage;
            this.KickDamage = state.KickDamage;
            this.Health = state.Health;
            this.CharacterName = state.Name;
            this.RunSpeed = state.RunSpeed;
            this.CreepSpeed = state.CreepSpeed;
            this.StrikeSpeed = state.StrikeSpeed;
            this.JumpSpeed = state.JumpSpeed;
            this.JumpForce = state.JumpForce;
        }

        // Use this for initialization
        protected virtual void Start()
        {
            this.IsCooldownEnabled = false;
            this.AttackCooldown = 0.0f;
            this.AttackStaminaUsage = 10.0f;

            this.MutagenBooster = null;

            Observable.Interval(TimeSpan.FromMilliseconds(this.StaminaRecoverySpeed))
                .Where(o => this.Stamina < MAX_STAMINA)
                .Subscribe(_ => this.Stamina += this.staminaIncrement)
                .AddTo(this);
        }

        public int MakeDamage(FightMode fightMode)
        {
            if (fightMode == FightMode.Punch)
                return this.PunchDamage;
            else
                return this.KickDamage;
        }

        public void TakeDamage(int value, int impulse = 0)
        {
            if (this.Health == 0)
            {
                return;
            }

            int blockedDamage = this.Armor - value;

            if (blockedDamage >= 0)
            {
                this.Armor = blockedDamage;
            }
            else
            {
                this.Armor = 0;
                this.Health += blockedDamage;
            }

            MessageBroker.Default.Publish(new CharacterDamaged(this.characterName));

            if (this.Health <= 0)
            {
                this.Health = 0;

                MessageBroker.Default.Publish(new CharacterDead(this));
                Debug.Log("You are die!");
            }
        }

        public void TakeHealth(int value)
        {
            this.Health += value;

            if (this.Health > MAX_HEALTH)
            {
                this.Health = MAX_HEALTH;
            }
        }

        public void TakeArmor(int value)
        {
            this.Armor += value;
        }

        public void ApplyMutagen(IMutagenBooster booster)
        {
            this.Stamina = MAX_STAMINA;
            this.MutagenBooster = booster;
        }

        public bool IsLifeFull()
        {
            return this.Health == MAX_HEALTH;
        }
    }

    public class CharacterDamaged
    {
        public string CharacterName;

        public CharacterDamaged(string characterName)
        {
            this.CharacterName = characterName;
        }
    }

    public class CharacterDead : ICommand
    {
        public ICharacter Character { get; }

        public CharacterDead(ICharacter character)
        {
            this.Character = character;
        }
    }
}