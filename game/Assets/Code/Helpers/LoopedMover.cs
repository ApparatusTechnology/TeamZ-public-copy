using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LoopedMover : MonoBehaviour
{
    public AnimationCurve Curve;
    public Vector3 Direction;
    public float Duration;

    private IDisposable moving;

    public void Move()
    {
        this.moving?.Dispose();
        this.moving = Observable.FromMicroCoroutine(() => this.MoveMicroCoruotine(Time.time)).Subscribe();
    }

    private IEnumerator MoveMicroCoruotine(float startTime)
    {
        var transform = this.transform;
        var initialPosition = transform.localPosition;
        while (Time.time - startTime < this.Duration)
        {
            var progress = (Time.time - startTime) / this.Duration;
            var value = this.Curve.Evaluate(progress);
            transform.localPosition = initialPosition + (value * this.Direction);
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
