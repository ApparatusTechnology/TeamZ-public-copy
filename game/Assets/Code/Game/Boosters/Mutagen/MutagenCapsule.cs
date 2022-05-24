using System.Collections.Generic;
using TeamZ.Code.Game.Characters;
using TeamZ.Code.Game.Characters.Hedgehog;
using TeamZ.Code.Game.Characters.Lizard;
using UnityEngine;

namespace TeamZ.Code.Game.Boosters.Mutagen
{
    public class MutagenCapsule : MonoBehaviour
    {
        public int MutagenDuration;  // in milliseconds
        public LizardAvailableSkills[] LizardBoosters;
        public HedgehogAvailableSkills[] HedgehogBoosters;

        private ICharacter character;
        private List<IMutagenBooster> boostersToChoose;

        void OnTriggerEnter2D(Collider2D col)
        {
            this.character = col.gameObject.GetComponentInParent<ICharacter>();

            if (this.character == null)
            {
                return;
            }

            // TEST
            var selectedBoosters = this.SelectRandomMutagen();
            int selectIndex = 0;

            if (selectedBoosters.Count > 0)
            {
                this.ApplyMutagen(selectIndex);
            }
            // TEST
        }

        public List<IMutagenBooster> SelectRandomMutagen()
        {
            if (this.character != null)
            {
                List<IMutagenBooster> characterBoosters = new List<IMutagenBooster>();

                if (this.character is Lizard)
                {
                    foreach (var boosterType in this.LizardBoosters)
                    {
                        characterBoosters.Add(LevelAvailableSkills.GetLizardBoosterByType(boosterType));
                    }
                }
                else if (this.character is Hedgehog)
                {
                    foreach (var boosterType in this.HedgehogBoosters)
                    {
                        characterBoosters.Add(LevelAvailableSkills.GetHedgehogBoosterByType(boosterType));
                    }
                }

                // random selection
                int firstRandSkillIdx = Random.Range(0, characterBoosters.Count);
                int secondRandSkillIdx = Random.Range(0, characterBoosters.Count);

                while (firstRandSkillIdx == secondRandSkillIdx)
                {
                    secondRandSkillIdx = Random.Range(0, characterBoosters.Count);
                }

                this.boostersToChoose = new List<IMutagenBooster>();

                this.boostersToChoose.Add(characterBoosters[firstRandSkillIdx]);
                this.boostersToChoose.Add(characterBoosters[secondRandSkillIdx]);

                return this.boostersToChoose;
            }

            return null;
        }

        public void ApplyMutagen(int boosterIndex)
        {
            if (this.character != null && this.boostersToChoose != null && this.boostersToChoose.Count > boosterIndex)
            {
                this.boostersToChoose[boosterIndex].Apply(this.character, this.MutagenDuration);
            }
        }
    }
}
