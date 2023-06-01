using System;
using UnityEngine;

namespace Core.Bindings.Tools.Rx {
    public class DefaultObservers<T> : IObserverLinkedList<T> {
        [NonSerialized] private ObserverNode<T> _root;
        [NonSerialized] private ObserverNode<T> _last;

        public void Invoke(in T value) {
            var node = _root;
            while (node != null) {
                try {
                    node.OnNext(value);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                    node.OnError(e);
                    node.Dispose(); // Unsubscribe
                }

                node = node.Next;
            }
        }

        public IDisposable Subscribe(IObserver<T> observer, bool emitOnSubscribe, in T currentValue) {
            if (emitOnSubscribe) {
                // raise latest value on subscribe
                try {
                    observer.OnNext(currentValue);
                }
                catch (Exception e) {
                    Debug.LogException(e);
                    observer.OnError(e);
                    return Disposable.Empty;
                }
            }

            // subscribe node, node as subscription.
            var next = new ObserverNode<T>(this, observer);
            if (_root == null) {
                _root = _last = next;
            }
            else {
                _last.Next = next;
                next.Previous = _last;
                _last = next;
            }

            return next;
        }

        void IObserverLinkedList<T>.UnsubscribeNode(ObserverNode<T> node) {
            if (node == _root) {
                _root = node.Next;
            }

            if (node == _last) {
                _last = node.Previous;
            }

            if (node.Previous != null) {
                node.Previous.Next = node.Next;
            }

            if (node.Next != null) {
                node.Next.Previous = node.Previous;
            }
        }

        public bool IsEmpty()
        {
            return _root == null && _last == null;
        }
    }
}