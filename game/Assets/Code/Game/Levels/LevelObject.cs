using System;
using TeamZ.Code.Game.Tips;
using TeamZ.GameSaving.MonoBehaviours;
using TeamZ.GameSaving.States;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Levels
{
	public class LevelObject : MonoBehaviourWithState<LevelObjectState>, IDamageable
	{
		[Serializable]
		public struct VisualState
		{
			public int hp;
			public Sprite texture;
		}

		public IntReactiveProperty Strength = new IntReactiveProperty(100);

		public bool IsDestructible
		{
			get { return this.isDestructible; }
			set { this.isDestructible = value; }
		}

        public bool IsOnlyMovable
        {
            get { return this.isOnlyMovable; }
            set { this.isOnlyMovable = value; }
        }

		public VisualState[] VisualStates;

		protected SpriteRenderer Renderer2D;

		[SerializeField]
		private bool isDestructible = false;

        [SerializeField]
        private bool isOnlyMovable = false;

		protected virtual void Start()
		{
			this.Renderer2D = this.GetComponent<SpriteRenderer>();

			if (this.Renderer2D != null && this.VisualStates.Length > 0)
			{
				this.Renderer2D.sprite = this.VisualStates[0].texture;
			}

			this.Strength.Subscribe(value =>
			{
				this.SwitchVisualState();

				if (value <= 0)
				{
					this.StrengthTooLow();
				}
			});
		}

		public virtual void TakeDamage(int damage, int impulse)
		{
			if (this.isOnlyMovable)
			{
				return;
			}

			if (this.IsDestructible)
			{
                // destroy object
				this.Strength.Value = Mathf.Max(this.Strength.Value - damage, 0);
			}
            else
            {
                // throw object
                this.TakeImpuls(impulse * damage);
            }
		}

		protected virtual void StrengthTooLow()
		{
			// CRUTCH!!! TODO: REWORK!!!
			var tip = GameObject.Find("KickOrJump");

			if (this.name == "Wooden_Box_distr (1)" && tip != null)
			{
				Destroy(tip);
			}
			// CRUTCH!!! TODO: REWORK!!!

			if (this.GetComponent<ConstantMessageTip>() != null)
			{
				Destroy(this.GetComponent<ConstantMessageTip>());
			}

			Destroy(this.GetComponent<Rigidbody2D>());
			Destroy(this.GetComponent<BoxCollider2D>());

			foreach (var box in this.GetComponentsInChildren<BoxCollider2D>())
			{
				Destroy(box);
			}
		}

		private void TakeImpuls(float impulse)
		{
			if (impulse == 0)
			{
				return;
			}

			var rigidBody = this.GetComponent<Rigidbody2D>();

			if (rigidBody == null)
			{
                rigidBody = this.GetComponentInParent<Rigidbody2D>();
			}

            if (rigidBody == null)
            {
                rigidBody = this.GetComponentInChildren<Rigidbody2D>();
            }

            if (rigidBody != null)
            {
                rigidBody.AddForce(new Vector2(impulse, Math.Abs(impulse / 2.0f)));
            }
        }

		private void SwitchVisualState()
		{
			foreach (var state in this.VisualStates)
			{
				if (this.Strength.Value >= state.hp)
				{
					this.Renderer2D.sprite = state.texture;
					return;
				}
			}
		}

		public override LevelObjectState GetState()
		{
			var grab = this.GetComponent<Grab.Grabbable>();
			
			return new LevelObjectState
			{
				IsDestructible = this.IsDestructible,
				Strength = this.Strength.Value,
                IsOnlyMovable = this.IsOnlyMovable,
				HighlightingColor = grab != null ? new Vector3(grab.HighlightingColor.r, grab.HighlightingColor.g, grab.HighlightingColor.b) : Vector3.zero
			};
		}

		public override void SetState(LevelObjectState state)
		{
			this.IsDestructible = state.IsDestructible;
			this.Strength.Value = state.Strength;
            this.IsOnlyMovable = state.IsOnlyMovable;

			var grab = this.GetComponent<Grab.Grabbable>();

			if (grab != null)
			{
				grab.HighlightingColor = new Color(state.HighlightingColor.x, state.HighlightingColor.y, state.HighlightingColor.z, 1);
			}
		}
	}
}
