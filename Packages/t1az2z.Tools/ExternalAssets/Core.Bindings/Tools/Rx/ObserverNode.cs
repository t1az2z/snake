using System;
using System.Threading;

namespace Core.Bindings.Tools.Rx {
    internal sealed class ObserverNode<T> : IObserver<T>, IDisposable {
        private readonly IObserver<T> _observer;
        private IObserverLinkedList<T> _list;

        public ObserverNode<T> Previous { get; internal set; }
        public ObserverNode<T> Next { get; internal set; }

        public ObserverNode(IObserverLinkedList<T> list, IObserver<T> observer) {
            this._list = list;
            this._observer = observer;
        }

        public void OnNext(T value) {
            _observer.OnNext(value);
        }

        public void OnError(Exception error) {
            _observer.OnError(error);
        }

        public void OnCompleted() {
            _observer.OnCompleted();
        }

        public void Dispose() {
            var sourceList = Interlocked.Exchange(ref _list, null);
            if (sourceList != null) {
                sourceList.UnsubscribeNode(this);
                sourceList = null;
            }
        }
    }
}