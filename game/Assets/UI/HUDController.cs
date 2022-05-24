using System;
using TeamZ.Code.DependencyInjection;
using TeamZ.Code.Game.Characters.Hedgehog;
using TeamZ.Code.Game.Characters.Lizard;
using TeamZ.Code.Game.Inventory;
using TeamZ.Code.Game.UserInput;
using TeamZ.Code.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace TeamZ.UI
{
    public class HUDController : View
    {
        [Serializable]
        public struct SegmentVisualState
        {
            public float SegmentValue;
            public Image SegmentTexture;
        }

        public Slider HealthSliderPlayer1;
        public Slider ArmorSliderPlayer1;
        public SegmentVisualState[] MutagenSegmentsPlayer1;
        public SegmentVisualState[] StaminaSegmentsPlayer1;

        public Slider HealthSliderPlayer2;
        public Slider ArmorSliderPlayer2;
        public SegmentVisualState[] MutagenSegmentsPlayer2;
        public SegmentVisualState[] StaminaSegmentsPlayer2;

        public Image InventoryItemPlayer1;
        public Image InventoryItemPlayer2;

        private UnityDependency<Lizard> lizardCharacter;
        private UnityDependency<Hedgehog> hedgehogCharacter;

        private GameObject hudInfoBarPlayer1;
        private GameObject hudInfoBarPlayer2;

        private void Start()
        {
            this.InventoryItemPlayer1.enabled = false;
            this.InventoryItemPlayer2.enabled = false;

            this.hudInfoBarPlayer1 = GameObject.Find("HUDInfoBarPlayer1");
            this.hudInfoBarPlayer2 = GameObject.Find("HUDInfoBarPlayer2");

            foreach (var segment in this.StaminaSegmentsPlayer1)
            {
                var color = segment.SegmentTexture.color;
                color.a = 1.0f;
                segment.SegmentTexture.color = color;
            }

            for (int i = 1; i < this.MutagenSegmentsPlayer1.Length; ++i)
            {
                var color = this.MutagenSegmentsPlayer1[i].SegmentTexture.color;
                color.a = 0.0f;
                this.MutagenSegmentsPlayer1[i].SegmentTexture.color = color;
            }

            foreach (var segment in this.StaminaSegmentsPlayer2)
            {
                var color = segment.SegmentTexture.color;
                color.a = 1.0f;
                segment.SegmentTexture.color = color;
            }

            for (int i = 1; i < this.MutagenSegmentsPlayer2.Length; ++i)
            {
                var color = this.MutagenSegmentsPlayer2[i].SegmentTexture.color;
                color.a = 0.0f;
                this.MutagenSegmentsPlayer2[i].SegmentTexture.color = color;
            }
        }

        private void Update()
        {
            if (this.lizardCharacter)
            {
                this.InventoryItemPlayer1.enabled = InventoryManager.HasItem<AccessCard>();

                if (!this.hudInfoBarPlayer1.activeSelf)
                {
                    this.hudInfoBarPlayer1.SetActive(true);
                }

                this.HealthSliderPlayer1.value = this.lizardCharacter.Value.Health;
                this.ArmorSliderPlayer1.value = this.lizardCharacter.Value.Armor;

                foreach (var segment in this.StaminaSegmentsPlayer1)
                {
                    var tmpColor = segment.SegmentTexture.color;

                    if (this.lizardCharacter.Value.Stamina >= segment.SegmentValue)
                    {
                        tmpColor.a = 1.0f;
                    }
                    else
                    {
                        tmpColor.a = 0.0f;
                    }

                    segment.SegmentTexture.color = tmpColor;
                }

                if (this.lizardCharacter.Value.MutagenBooster != null)
                {
                    foreach (var segment in this.MutagenSegmentsPlayer1)
                    {
                        var tmpColor = segment.SegmentTexture.color;

                        if (this.lizardCharacter.Value.MutagenBooster.MutagenTimeLeft >= segment.SegmentValue)
                        {
                            tmpColor.a = 1.0f;
                        }
                        else
                        {
                            tmpColor.a = 0.0f;
                        }

                        segment.SegmentTexture.color = tmpColor;
                    }
                }
            }
            else if (this.hudInfoBarPlayer1.activeSelf)
            {
                this.hudInfoBarPlayer1.SetActive(false);
            }

            if (this.hedgehogCharacter)
            {
                this.InventoryItemPlayer2.enabled = InventoryManager.HasItem<AccessCard>();

                if (!this.hudInfoBarPlayer2.activeSelf)
                {
                    this.hudInfoBarPlayer2.SetActive(true);
                }

                this.HealthSliderPlayer2.value = this.hedgehogCharacter.Value.Health;
                this.ArmorSliderPlayer2.value = this.hedgehogCharacter.Value.Armor;

                foreach (var segment in this.StaminaSegmentsPlayer2)
                {
                    var tmpColor = segment.SegmentTexture.color;

                    if (this.hedgehogCharacter.Value.Stamina >= segment.SegmentValue)
                    {
                        tmpColor.a = 1.0f;
                    }
                    else
                    {
                        tmpColor.a = 0.0f;
                    }

                    segment.SegmentTexture.color = tmpColor;
                }

                if (this.hedgehogCharacter.Value.MutagenBooster != null)
                {
                    foreach (var segment in this.MutagenSegmentsPlayer2)
                    {
                        var tmpColor = segment.SegmentTexture.color;

                        if (this.hedgehogCharacter.Value.MutagenBooster.MutagenTimeLeft >= segment.SegmentValue)
                        {
                            tmpColor.a = 1.0f;
                        }
                        else
                        {
                            tmpColor.a = 0.0f;
                        }

                        segment.SegmentTexture.color = tmpColor;
                    }
                }
            }
            else if (this.hudInfoBarPlayer2.activeSelf)
            {
                this.hudInfoBarPlayer2.SetActive(false);
            }
        }
    }
}
