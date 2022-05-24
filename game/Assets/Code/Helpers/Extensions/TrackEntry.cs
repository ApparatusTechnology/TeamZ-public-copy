using System;
using Spine;
using Spine.Unity;
using UniRx;
using UniRx.Async;

namespace Code.Helpers.Extensions
{
    public static class TrackEntryExtensions
    {
        public static UniTask OnCompleted(this TrackEntry track)
        {
            var source = new UniTaskCompletionSource();
            track.Complete += TrackOnComplete;

            return source.Task;
            
            void TrackOnComplete(TrackEntry trackEntry)
            {
                track.Complete -= TrackOnComplete;
                source.TrySetResult();
            }
        }

        public static UniTask OnEvent(this TrackEntry track, string eventName)
        {
            var source = new UniTaskCompletionSource();
            track.Event += TrackOnEvent;
            track.Complete += TrackOnComplete;

            return source.Task;

            void TrackOnComplete(TrackEntry trackEntry)
            {
                track.Complete -= TrackOnComplete;
            }

            void TrackOnEvent(TrackEntry trackEntry, Event e)
            {
                if (e.Data.Name == eventName)
                {
                    track.Event -= TrackOnEvent;
                    source.TrySetResult();
                }
            }
        }
        
        public static IObservable<Event> ToEventStream(this SkeletonAnimation animator, string eventName)
        {
            var source = new Subject<Event>();
            animator.state.Event += TrackOnEvent;
            source.AddTo(animator);

            return source;

            void TrackOnEvent(TrackEntry trackEntry, Event e)
            {
                if (e.Data.Name == eventName)
                {
                    source.OnNext(e);
                }
            }
        }

        public static UniTask OnEnd(this TrackEntry track)
        {
            var source = new UniTaskCompletionSource();
            track.End += TrackEnd;

            return source.Task;
            
            void TrackEnd(TrackEntry trackEntry)
            {
                track.Complete -= TrackEnd;
                source.TrySetResult();
            }
        }
    }
}