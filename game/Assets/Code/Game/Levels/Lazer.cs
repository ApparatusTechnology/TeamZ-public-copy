using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Lazer : MonoBehaviour
{
    public LayerMask LayerMask;
    public float Distance = 25;

    private int gameObjectInZone = 0;
    private IDisposable lazerLengthTraking;
    private LineRenderer lineRender;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        this.lineRender = this.GetComponent<LineRenderer>();
        this.lineRender.SetPosition(1, new Vector3(this.Distance, 0));
        
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        this.boxCollider.offset = new Vector2(this.Distance / 2, 0);
        this.boxCollider.size = new Vector2(this.Distance, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.LayerMask.Contains(collision.gameObject.layer))
        {
            this.gameObjectInZone++;
            if (this.gameObjectInZone == 1)
            {
                this.lazerLengthTraking = Observable.FromMicroCoroutine(() => this.TrackLazerLength())
                    .Subscribe()
                    .AddTo(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.LayerMask.Contains(collision.gameObject.layer))
        {
            this.gameObjectInZone--;
            if (this.gameObjectInZone == 0)
            {
                this.lazerLengthTraking?.Dispose();
                this.lineRender.SetPosition(1, new Vector3(this.Distance, 0));
            }
        }
    }

    private IEnumerator TrackLazerLength()
    {
        while (true)
        {
            var hit = Physics2D.Raycast(this.transform.position, -this.transform.right, this.Distance, this.LayerMask.value);
            if (hit)
            {
                this.lineRender.SetPosition(1, new Vector3(hit.distance, 0));
            }
            
            yield return null;
        }
    }
}
