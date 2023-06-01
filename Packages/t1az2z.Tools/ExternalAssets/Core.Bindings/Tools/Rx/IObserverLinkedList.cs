namespace Core.Bindings.Tools.Rx {
    internal interface IObserverLinkedList<T> {
        void UnsubscribeNode(ObserverNode<T> node);
    }
}