using System;
using TeamZ;
using UniRx;
using UnityEngine;

public class MovableObjectPusher : MonoBehaviour
{
    private IDisposable timer;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponentInParent<MovableObject>() is MovableObject obj)
        {
            this.timer?.Dispose();
            if (!obj.GetComponent<Rigidbody2D>())
            {
                var rigibody = obj.gameObject.AddComponent<Rigidbody2D>();
                rigibody.mass = obj.Mass;
                rigibody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponentInParent<MovableObject>()?.GetComponent<Rigidbody2D>() is Rigidbody2D rigidBody2d)
        {
            this.timer?.Dispose();
            this.timer = Observable
                .Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    rigidBody2d.Destroy();
                    this.timer = null;
                });
        }
    }
}
