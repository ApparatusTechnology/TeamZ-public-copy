using System.Timers;
using TeamZ.Code.Game.Characters;
using UniRx;
using UnityEngine;

namespace TeamZ.Code.Game.Boosters.Mutagen
{
    public delegate void MutagenFinishedEvent();

    public interface IMutagenBooster
    {
        bool IsMutagenApplied { get; set; }
        int MutagenDuration { get; set; }   // in milliseconds (minimum 10 sec)
        float MutagenTimeLeft { get; set; }

        event MutagenFinishedEvent OnMutagenFinished;

        void Apply(ICharacter character, int duration);
    }

    public abstract class MutagenBooster : IMutagenBooster
    {
        public bool IsMutagenApplied { get; set; }

        public int MutagenDuration { get; set; }

        public float MutagenTimeLeft { get; set; }

        public event MutagenFinishedEvent OnMutagenFinished;

        private Timer mutagenActionTimer = null;

        private const float MIN_MUTAGEN_TIME_FACTOR = 100000.0f;

        protected ICharacter character;

        public MutagenBooster()
        {
            this.IsMutagenApplied = false;
            this.MutagenDuration = 0;
            this.MutagenTimeLeft = 0.0f;
            this.mutagenActionTimer = null;
        }

        private void OnUpdate()
        {
            if (this.IsMutagenApplied && this.mutagenActionTimer.Enabled && this.MutagenTimeLeft > 0.0f)
            {
                this.MutagenTimeLeft -= Time.deltaTime * MIN_MUTAGEN_TIME_FACTOR / this.MutagenDuration;
            }
        }

        public virtual void Apply(ICharacter character, int duration)
        {
            this.character = character;
            this.IsMutagenApplied = true;
            this.MutagenDuration = duration;
            this.MutagenTimeLeft = 100.0f;
            this.mutagenActionTimer = new Timer(duration);
            Observable.EveryFixedUpdate().Subscribe(_ => OnUpdate());
            this.mutagenActionTimer.Elapsed += MutagenActionTimer_Elapsed;
            this.mutagenActionTimer.Start();
        }

        private void MutagenActionTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnMutagenFinished();
            this.mutagenActionTimer.Stop();
            this.IsMutagenApplied = false;
            this.MutagenDuration = 0;
            this.MutagenTimeLeft = 0.0f;
            this.mutagenActionTimer.Elapsed -= MutagenActionTimer_Elapsed;
            this.mutagenActionTimer = null;
            this.character.MutagenBooster = null;
            this.character = null;
        }
    }
}
