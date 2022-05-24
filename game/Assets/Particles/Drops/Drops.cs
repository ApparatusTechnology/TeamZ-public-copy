using TeamZ.Code.Game.Characters;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public int Damage;

    public AudioClip[] Clips;
    public AudioSource AudioSource;

    private void OnParticleCollision(GameObject other)
    {
        if (other.GetComponent<ICharacter>() is ICharacter character)
        {
            character.TakeDamage(this.Damage, 0);
            return;
        }

        var someClip = this.Clips.SelectRandom();
        this.AudioSource.PlayOneShot(someClip);
    }
}
