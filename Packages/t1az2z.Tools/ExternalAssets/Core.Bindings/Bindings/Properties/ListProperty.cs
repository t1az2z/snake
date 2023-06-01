using Binding.Base;
using System;
using System.Collections.Generic;
using System.Collections;
using Core.Bindings.Tools.Extensions;
#if !BASE_GLOBAL_EXTENSIONS
#endif

namespace Binding {
    public interface IListProperty<out T> : IProperty where T : BaseListElementData {
        event Action<BaseListElementData> OnElementAdded;
        event Action<int, BaseListElementData> OnElementInserted;
        event Action<int> OnElementRemoved;
        event Action<int, float> OnScrollToElement;
        event Action<int, BaseListElementData> OnElementChanged;
        event Action<int, int> OnElementMoved;
        event Action<bool> OnChangeValueProcess;
        event Action OnFullReorder;
        event Action OnClear;
        event Action<BaseListElementData> OnClick;

        int Count { get; }
        IEnumerable<T> Value { get; }
        T this[int index] { get; }

        void Move(BaseListElementData data, int position);
        T AllocateNew();
    }

    public interface ICountOfListProperty {
        event Action OnCountChanged;
        int Count { get; }
    }

    public class ListProperty<T> : Property<IEnumerable<T>>, IListProperty<T>, IList<T>, ICountOfListProperty where T : BaseListElementData, new() {
        public event Action<BaseListElementData> OnElementAdded;
        public event Action<int, BaseListElementData> OnElementInserted;
        public event Action<int> OnElementRemoved;
        public event Action<int, float> OnScrollToElement;
        public event Action<int, BaseListElementData> OnElementChanged;
        public event Action<int, int> OnElementMoved;
        public event Action<bool> OnChangeValueProcess;
        public event Action OnFullReorder;
        public event Action<BaseListElementData> OnClick;
        public event Action OnClear;
        public event Action OnCountChanged;

        public T this[int index] {
            get { return GetByIndex(index); }
            set { SetByIndex(index, value); }
        }

        public int Count {
            get { return _elementsData.Count; }
        }

        public bool IsReadOnly => false;

        private List<T> _elementsData = new List<T>();

        #region List methods
        public void Add(T data) {
            _elementsData.Add(data);
            data.OnClick += ClickHandler;
            OnElementAdded?.TryCatchCall(data);
            OnCountChanged?.TryCatchCall();
        }

        public T Add() {
            var data = AllocateNew();
            OnElementAdded?.TryCatchCall(data);
            OnCountChanged?.TryCatchCall();
            return data;
        }

        public void Insert(int index, T data) {
            InsertElement(index, data);
        }

        public void MarkElementChanged(int index) {
            if ((index < 0) || (index >= _elementsData.Count)) {
                return;
            }
            OnElementChanged?.TryCatchCall(index, _elementsData[index]);
        }

        public bool Remove(T data) {
            var index = _elementsData.IndexOf(data);
            if (index != -1) {
                RemoveElementAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index) {
            if ((index < 0) || (index >= _elementsData.Count)) {
                return;
            }

            RemoveElementAt(index);
        }

        public void RemoveAll(Predicate<T> predicate) {
            var indexes = new List<int>();
            for (int i = 0; i < _elementsData.Count; i++) {
                var data = _elementsData[i];
                if (predicate(data)) {
                    indexes.Add(i);
                }
            }

            for (int i = 0; i < indexes.Count; i++) {
                var index = indexes[i];
                RemoveElementAt(index);
            }
        }

        public void Fill(int count, Func<T> constructor) {
            ClearElements();

            for (int i = 0; i < count; i++) {
                var element = constructor();
                element.OnClick += ClickHandler;
                _elementsData.Add(element);
            }

            ValueChanged();
        }

        public void Fill(int count, Func<int, T> constructor) {
            ClearElements();

            for (int i = 0; i < count; i++) {
                var element = constructor(i);
                element.OnClick += ClickHandler;
                _elementsData.Add(element);
            }

            ValueChanged();
        }

        public void Move(int index, int position) {
            if ((index < 0) || (index >= _elementsData.Count) || (position < 0) || (position >= _elementsData.Count)) {
                return;
            }

            MoveElement(index, position);
        }

        void IListProperty<T>.Move(BaseListElementData data, int position) {
            for (int i = 0; i < _elementsData.Count; ++i) {
                if (_elementsData[i] == data) {
                    Move(i, position);
                    return;
                }
            }
        }

        public void Move(T data, int position) {
            var index = _elementsData.IndexOf(data);
            if (index != -1) {
                Move(index, position);
            }
        }

        public void ScrollTo(int index, float time = -1f) {
            if ((index < 0) || (index >= _elementsData.Count)) {
                return;
            }

            OnScrollToElement?.TryCatchCall(index, time);
        }

        public void ScrollTo(T data, float time = -1f) {
            var index = _elementsData.IndexOf(data);
            if (index != -1) {
                OnScrollToElement?.TryCatchCall(index, time);
            }
        }

        public void Sort() {
            _elementsData.Sort();
            OnFullReorder?.TryCatchCall();
        }

        public void Sort(Comparison<T> comparison) {
            _elementsData.Sort(comparison);
            OnFullReorder?.TryCatchCall();
        }

        public T Find(Predicate<T> predicate) => _elementsData.Find(predicate);
        public List<T> FindAll(Predicate<T> predicate) => _elementsData.FindAll(predicate);
        public bool Exists(Predicate<T> predicate) => _elementsData.Exists(predicate);
        public int IndexOf(T data) => _elementsData.IndexOf(data);
        public int IndexOf(Predicate<T> predicate) => _elementsData.FindIndex(predicate);
        public bool Contains(T item) => _elementsData.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => _elementsData.CopyTo(array, arrayIndex);
        IEnumerator IEnumerable.GetEnumerator() => _elementsData.GetEnumerator();
        public IEnumerator<T> GetEnumerator() => _elementsData.GetEnumerator();
        #endregion List methods

        public void AddOrRemoveDiff(int count, Action<BaseListElementData> onClick = null) {
            var diff = count - _elementsData.Count;
            if (diff > 0) {
                for (int i = 0; i < diff; i++) {
                    var element = Add();
                    if (onClick != null) {
                        element.OnClick += onClick;
                    }
                }
            }
            else if (diff < 0) {
                var elementsCount = _elementsData.Count;
                for (int i = elementsCount - 1; i >= elementsCount - diff; i--) {
                    RemoveAt(i);
                }
            }
        }
        
        public int GetLoopedIndex(int indexForLoop) {
            return _elementsData.GetLoopedIndex(indexForLoop);
        }

        public int GetClampedIndex(int indexForClamp) {
            return _elementsData.GetClampedIndex(indexForClamp);
        }

        public void Clear() {
            if (_elementsData.Count > 0) {
                ClearElements();
                ValueChanged();
            }
        }

        protected override IEnumerable<T> GetValue() {
            return _elementsData;
        }

        protected override void SetValue(IEnumerable<T> value) {
            var newElementsData = value != null ? new List<T>(value) : new List<T>();
            if ((_elementsData.Count == 0) || (newElementsData.Count == 0)) {
                ClearElements();
                _elementsData = newElementsData;
                _elementsData.ForEach((data) => {
                    data.OnClick += ClickHandler;
                });
                ValueChanged();
                return;
            }

            OnChangeValueProcess?.TryCatchCall(true);

            if (newElementsData.Count > _elementsData.Count) {
                for (int i = 0; i < newElementsData.Count; i++) {
                    if (i < _elementsData.Count) {
                        var data = newElementsData[i];
                        ChangeElement(i, data);
                    }
                    else {
                        var data = newElementsData[i];
                        InsertElement(_elementsData.Count, data);
                    }
                }
            }
            else {
                for (int i = 0; i < newElementsData.Count; i++) {
                    var data = newElementsData[i];
                    ChangeElement(i, data);
                }

                var index = newElementsData.Count;
                while (index < _elementsData.Count) {
                    RemoveElementAt(index);
                }
            }

            OnChangeValueProcess?.TryCatchCall(false);
        }

        protected T GetByIndex(int index) {
            return (index >= 0) && (index < _elementsData.Count) ? _elementsData[index] : null;
        }

        protected void SetByIndex(int index, T data) {
            if ((index < 0) || (index >= _elementsData.Count)) {
                return;
            }

            ChangeElement(index, data);
        }

        private void MoveElement(int index, int position) {
            var element = _elementsData[index];
            _elementsData.RemoveAt(index);
            _elementsData.Insert(position, element);

            OnElementMoved?.TryCatchCall(index, position);
        }

        public T AllocateNew() {
            var data = new T();
            _elementsData.Add(data);
            data.OnClick += ClickHandler;
            return data;
        }

        private void InsertElement(int index, T data) {
            _elementsData.Insert(index, data);
            data.OnClick += ClickHandler;

            OnElementInserted?.TryCatchCall(index, data);
            OnCountChanged?.TryCatchCall();
        }

        private void RemoveElementAt(int index) {
            var data = _elementsData[index];
            data.OnClick -= ClickHandler;
            data.OnRemoved();
            _elementsData.RemoveAt(index);

            OnElementRemoved?.TryCatchCall(index);
            OnCountChanged?.TryCatchCall();
        }

        private void ClearElements() {
            for (int i = 0; i < _elementsData.Count; i++) {
                var data = _elementsData[i];
                data.OnClick -= ClickHandler;
                data.OnRemoved();
            }

            _elementsData.Clear();
            OnClear?.TryCatchCall();
            OnCountChanged?.TryCatchCall();
        }

        private void ChangeElement(int index, T data) {
            var oldData = _elementsData[index];
            if (oldData == data) {
                return;
            }

            _elementsData[index] = data;
            oldData.OnClick -= ClickHandler;
            oldData.OnRemoved();
            data.OnClick += ClickHandler;

            OnElementChanged?.TryCatchCall(index, data);
        }

        private void ClickHandler(BaseListElementData data) {
            OnClick?.TryCatchCall(data as T);
        }
    }
}