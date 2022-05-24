using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamZ.Code.Game;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace TeamZ.Code.Helpers.Extentions
{
    public static class ReactivePropertyExtentions
    {
        public static IObservable<bool> True(this IObservable<bool> observable)
        {
            return observable.Where(o => o);
        }

        public static IObservable<bool> False(this IObservable<bool> observable)
        {
            return observable.Where(o => !o);
        }

        public static IObservable<bool> PressAgain(this IObservable<bool> observable)
        {
            var subject = new Subject<bool>();
            observable
                .False()
                .First()
                .Subscribe(_ =>
                {
                    observable
                        .True()
                        .Subscribe(o => subject.OnNext(o));
                });

            return subject.AsObservable();
        }

        public static IObservable<TValue> GameActive<TValue>(this IObservable<TValue> observable)
        {
            return observable.Where(o => GameHelper.IsActive);
        }


        public static IObservable<bool> HoldFor(this IObservable<bool> reactiveProperty, TimeSpan time)
        {
            return reactiveProperty.HoldFor(false, time);
        }

        public static IObservable<TValue> HoldFor<TValue>(this IObservable<TValue> reactiveProperty,
            TValue negativeValue,
            TimeSpan time)
        {
            var id = Guid.NewGuid();
            return Observable.Create<TValue>(observable =>
            {
                var timeCalculation = reactiveProperty
                    .Where(o => !o.Equals(negativeValue))
                    .Subscribe(async o =>
                    {
                        var initialGuid = id;
                        await Task.Delay((int) time.TotalMilliseconds);
                        if (initialGuid == id)
                        {
                            observable.OnNext(o);
                        }
                    });

                var idReset = reactiveProperty
                    .Where(o => o.Equals(negativeValue))
                    .Subscribe(_ => id = Guid.NewGuid());

                return Disposable.Create(() =>
                {
                    timeCalculation.Dispose();
                    idReset.Dispose();
                });
            });;
        }

        public static IDisposable SubscribeMany<TValue>(this IEnumerable<IObservable<TValue>> objecvables,
            Action<TValue> action)
        {
            var subscrbtions = objecvables
                .Select(o => o.Subscribe(action))
                .ToArray();

            return Disposable.Create(() => subscrbtions.Dispose());
        }

        public static IObservable<LinkedList<(float Time, TValue Value)>> Window<TValue>(this IObservable<TValue> items, TimeSpan period)
        {
            var time = (float)period.TotalSeconds;
            var subject = new Subject<LinkedList<(float Time, TValue Value)>>();
            var windowItems = new LinkedList<(float Time, TValue Value)>();

            items.DoOnError(subject.OnError);
            items.DoOnCompleted(subject.OnCompleted);
            
            items.Subscribe(item =>
            {
                var currentTime = Time.time;
                while (windowItems.First != null && currentTime - windowItems.First.Value.Time > time)
                {
                    windowItems.RemoveFirst();
                }

                windowItems.AddLast((currentTime, item));
                subject.OnNext(windowItems);
            });

            return subject.AsObservable();
        }
        
        public static IObservable<IEnumerable<TValue>> Window<TValue>(this IObservable<TValue> items, int windowSize)
        {
            var subject = new Subject<IEnumerable<TValue>>();
            var windowItems = new LinkedList<TValue>();

            items.DoOnError(subject.OnError);
            items.DoOnCompleted(subject.OnCompleted);
            
            items.Subscribe(item =>
            {
                while (windowItems.First != null && windowItems.Count >= windowSize)
                {
                    windowItems.RemoveFirst();
                }

                windowItems.AddLast(item);
                
                subject.OnNext(windowItems);
            });

            return subject.AsObservable();
        }
    }
}