using System;
using TeamZ.Assets.Code.Game.Animators;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Levels;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Helpers
{
	public class SoundManager : MonoBehaviour
	{
		private const float MUSIC_CHANGE_RATE = 0.4f;
		private const float MUSIC_CHANGE_RATE_X4 = MUSIC_CHANGE_RATE * 4;
		private const float MUSIC_VOLUME = 0.2f;

		private const string NOISE = "NOISE";
		private const string MENU = "MENU";

		// characters effects
		public AudioClip[] Steps;
		public AudioClip Jump;
		public AudioClip Landing;
		public AudioClip Climb;
		public AudioClip Punch;
		public AudioClip Kick;
		public AudioClip Hurting;
		public AudioClip Hurting2;
		public AudioClip Dying;
		public AudioClip Die;
		// world effects
		public AudioClip Portal;
		public AudioClip TakeObject;
		public AudioClip MoveObject;
		public AudioClip DropObject;
		public AudioClip KillObject;
		public AudioClip EnemyGunShooting;
        public AudioClip Activate;
		public AudioClip Explosion;
		public AudioClip Fire;
        // ambient sounds
        public AudioClip AmbientNoize1;
		public AudioClip AmbientNoize2;
		public AudioClip AmbientNoize3;
		// game effects
		public AudioClip MenuOpenClose;
		public AudioClip MenuClick;
		// game music
		public AudioClip MenuBackgroungMusic;
		public AudioClip Level1BackgroungMusic;
		public AudioClip Level2BackgroungMusic;
		public AudioClip Level3BackgroungMusic;

		private AudioSource defaultAudioSource;

		private string currentLevelMusic = "Level_Laboratory";
		private AudioSourcePull sounds;
		private Guid soundId;

		// Use this for initialization
		void Start ()
		{
			this.defaultAudioSource = GetComponent<AudioSource>();
			this.defaultAudioSource.Stop();
			this.defaultAudioSource.volume = 0.4f;
			this.defaultAudioSource.loop = false;
			this.sounds = Dependency<AudioSourcePull>.Resolve();

			MessageBroker.Default.Receive<RunHappened>().Subscribe(this.PlayStepsSound).AddTo(this);
			MessageBroker.Default.Receive<RunEnded>().Subscribe(this.StopStepsSound).AddTo(this);
			MessageBroker.Default.Receive<StepHappened>().Subscribe(this.StepHappened).AddTo(this);
			MessageBroker.Default.Receive<JumpHappened>().Subscribe(this.PlayJumpSound).AddTo(this);
			MessageBroker.Default.Receive<LandingHappened>().Subscribe(this.PlayLandingSound).AddTo(this);
			MessageBroker.Default.Receive<PunchHappened>().Subscribe(this.PlayPunchSound).AddTo(this);
			MessageBroker.Default.Receive<KickHappened>().Subscribe(this.PlayKickSound).AddTo(this);
			MessageBroker.Default.Receive<TakeObjectHappened>().Subscribe(this.PlayTakeObjectSound).AddTo(this);
            MessageBroker.Default.Receive<ActivationHappened>().Subscribe(this.PlayActivateSound).AddTo(this);
            MessageBroker.Default.Receive<PortalToNextLevelHappened>().Subscribe(this.PlayPortalToNextLevelSound).AddTo(this);
            MessageBroker.Default.Receive<CharacterDamaged>().Subscribe(this.PlayCharacterDamageSound).AddTo(this);
			MessageBroker.Default.Receive<CharacterDead>().Subscribe(o => 
				{
					this.sounds.PlayOnce(this.Dying, "Dying", 5.0f).Forget();
					this.sounds.PlayOnce(this.Die, "Death", 6.0f).Forget(); 
				}
			).AddTo(this);
			MessageBroker.Default.Receive<PauseGame>().Subscribe(this.OnGamePausedAsync).AddTo(this);
			MessageBroker.Default.Receive<ResumeGame>().Subscribe(this.OnGameResumedAsync).AddTo(this);
			MessageBroker.Default.Receive<ExplosionHappened>().Subscribe(this.PlayExplosionSound).AddTo(this);
			MessageBroker.Default.Receive<BurningHappened>().Subscribe(this.PlayFireSound).AddTo(this);
			MessageBroker.Default.Receive<BurningEnded>().Subscribe(this.StopFireSound).AddTo(this);
		}
	
		private void OnGamePausedAsync(PauseGame soundObj)
		{
			Debug.Log($"Game paused");

			this.defaultAudioSource.loop = false;
			this.defaultAudioSource.Stop();
			this.defaultAudioSource.clip = null;

			this.sounds.SoftPause(NOISE, MUSIC_CHANGE_RATE_X4).Forget();
			this.sounds.SoftPause(this.currentLevelMusic, MUSIC_CHANGE_RATE_X4).Forget();

			this.sounds.PlayLooped(this.MenuBackgroungMusic, MENU, MUSIC_VOLUME * 2);
		}

		private void OnGameResumedAsync(ResumeGame message)
		{
			if (string.IsNullOrWhiteSpace(message.Level))
			{
				return;
			}

			Debug.Log($"Game resumed");

			this.defaultAudioSource.loop = false;
			this.defaultAudioSource.Stop();
			this.defaultAudioSource.clip = null;

			this.sounds.SoftRelease(MENU, MUSIC_CHANGE_RATE_X4).Forget();
			this.sounds.PlayLooped(this.AmbientNoize2, NOISE, MUSIC_VOLUME / 3, MUSIC_CHANGE_RATE);

			string levelName = $"Level_{message.Level}";
			switch (message.Level)
			{
				case Level.LABORATORY:
					if (this.sounds.IsPlaying(levelName, this.Level1BackgroungMusic))
					{
						break;
					}

					this.sounds.PlayLooped(this.Level1BackgroungMusic, levelName, MUSIC_VOLUME, MUSIC_CHANGE_RATE);
					break;
				case Level.LABORATORY2:
					if (this.sounds.IsPlaying(levelName, this.Level2BackgroungMusic))
					{
						break;
					}

					this.sounds.PlayLooped(this.Level2BackgroungMusic, levelName, MUSIC_VOLUME, MUSIC_CHANGE_RATE);
					break;
			}

			if (this.currentLevelMusic != levelName)
			{
				this.sounds.SoftRelease(this.currentLevelMusic).Forget();
			}
			this.currentLevelMusic = levelName;
		}

		private void PlayStepsSound(RunHappened soundObj)
		{
            if (soundObj.isClimbing)
			{
				if (this.Climb != null && this.defaultAudioSource.clip != this.Climb)
				{
					this.defaultAudioSource.volume = 0.2f;
					this.defaultAudioSource.loop = true;
					this.defaultAudioSource.clip = this.Climb;
					this.defaultAudioSource.Play();
				}
			}
		}

		private void StopStepsSound(RunEnded soundObj)
		{
			if (soundObj.isClimbing)
			{
				if (this.Climb != null && this.defaultAudioSource.clip == this.Climb && this.defaultAudioSource.isPlaying)
				{
					this.defaultAudioSource.loop = false;
					this.defaultAudioSource.Stop();
					this.defaultAudioSource.clip = null;
				}
			}
		}

		private void PlayJumpSound(JumpHappened soundObj)
		{
			if (this.Jump != null)
			{
				this.sounds.Play(this.Jump, 2.5f).Forget();
			}
		}

		private void PlayLandingSound(LandingHappened soundObj)
		{
			if (this.Landing != null)
			{
				this.sounds.Play(this.Landing, 3.5f).Forget();
			}
		}

		private void PlayPunchSound(PunchHappened soundObj)
		{
			if (this.Punch != null)
			{
				this.sounds.Play(this.Punch, 0.6f).Forget();
			}
		}

		private void PlayKickSound(KickHappened soundObj)
		{
			if (this.Kick != null)
			{
				this.sounds.Play(this.Kick, 0.6f).Forget();
			}
		}

		private void PlayTakeObjectSound(TakeObjectHappened soundObj)
		{
			if (this.TakeObject != null)
			{
				this.sounds.Play(this.TakeObject, 1.5f).Forget();
			}
		}

		private void StepHappened(StepHappened step)
		{
			if (this.Steps != null)
			{
				this.sounds.Play(this.Steps.SelectRandom(), 1.0f).Forget();
			}
		}

		
		private void PlayPortalToNextLevelSound(PortalToNextLevelHappened soundObj)
		{
			if (this.Portal != null)
			{
				this.sounds.Play(this.Portal, 5.0f).Forget();
			}
		}

        private void PlayCharacterDamageSound(CharacterDamaged soundObj)
        {
            if (this.Hurting != null && this.Hurting2 != null)
            {
				switch (soundObj.CharacterName)
				{
					case "Lizard":
						this.sounds.Play(this.Hurting, 1.5f).Forget();
						break;
					case "Hedgehog":
						this.sounds.Play(this.Hurting2, 1.5f).Forget();
						break;
				}
			}
        }

        private void PlayActivateSound(ActivationHappened soundObj)
        {
            if (this.Activate != null)
            {
                this.sounds.Play(this.Activate, 1.5f).Forget();
            }
        }

		private void PlayExplosionSound(ExplosionHappened soundObj)
		{
			if (this.Explosion != null)
			{
				this.sounds.Play(this.Explosion, 1.5f).Forget();
			}
		}

		private void PlayFireSound(BurningHappened soundObj)
		{
			if (this.Fire != null)
			{
				this.soundId = Guid.NewGuid();
				this.sounds.PlayLooped(this.Fire, this.soundId, 2.0f);
			}
		}

		private void StopFireSound(BurningEnded soundObj)
		{
			if (this.Fire != null)
			{
				this.sounds.Stop(this.soundId);
			}
		}
	}
}
