using System;
using System.Collections.Generic;
using Binding;
using Binding.Elements;
using Core.Bindings.Tools.Extensions;
using Core.Bindings.Tools.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class ListBinding : BaseBinding<IListProperty<BaseListElementData>>, IBeginDragHandler, IEndDragHandler {
        public event Action OnElementsCountChanged = delegate { };
        
        [SerializeField] private BaseListElement ElementPrefab = null;
        [SerializeField] private Transform Spawner = null;
        [SerializeField] private AutoScrollPivotTypeEnum AutoScrollPivot = AutoScrollPivotTypeEnum.LeftOrTop;
        [SerializeField] private EaseType AutoscrollEaseType = EaseType.Lerp;
        [SerializeField] private float DragTolerance = 5f;
        [SerializeField] private float ScrollSpeedByDrag = 100f;
        [SerializeField] private bool NewOnTop = false;
        
        private readonly List<BaseListElement> _elements = new List<BaseListElement>();
        private readonly HashSet<BaseListElement> _inactiveElements = new HashSet<BaseListElement>();
        private readonly List<int> _indexCache = new List<int>();
        
        private ScrollRect _scrollRect;
        private RectTransform _content;
        private RectTransform _viewport;
        private BaseDraggableListElement _draggableElement;
        private RectTransform _draggableElementBlank;
        private Vector2 _contentStartPosition;
        private int _draggableElementIndex;
        private bool _canDragElements;
        private bool _canAnimateElements;
        private bool _scrollVertical;
        private bool _scrollHorizontal;

        private PhaseTimeController _autoScrollController = new PhaseTimeController(PhaseControllerTimerMode.UnscaledTime, PhaseControllerWorkMode.Once, 1f, true);
        private bool _autoScrollTargetReached = false;
        private bool _autoScrollCanBeInterrupted = false;

        private int _fromAutoScroll;
        private int _toAutoScroll;
        private Vector2 _prevPos;
        private int _prevCount;
        private bool _isDragging;

        public BaseListElement GetElementPrefab() => ElementPrefab;

        protected override void Awake() {
            _scrollRect = GetComponent<ScrollRect>();

            _draggableElement = null;
            _draggableElementBlank = null;
            _draggableElementIndex = -1;
            _canDragElements = ElementPrefab is BaseDraggableListElement;
            _canAnimateElements = ElementPrefab is BaseAnimatedListElement;

            if (_scrollRect != null) {
                _content = _scrollRect.content;
                _viewport = _scrollRect.viewport;
                
                if (!_content) throw new Exception("ScrollRect.Content must be set");
                if (!_viewport) throw new Exception("ScrollRect.Viewport must be set");
            }
            else {
                _content = transform as RectTransform;
                _viewport = transform as RectTransform;
            }

            var layoutGroup = _content.GetComponent<LayoutGroup>();
            _scrollVertical = layoutGroup is VerticalLayoutGroup;
            _scrollHorizontal = layoutGroup is HorizontalLayoutGroup;

            if (ElementPrefab && ElementPrefab.gameObject.scene.IsValid()) {
                ElementPrefab.gameObject.SetActive(false);
            }

            base.Awake();
            Subscribe();
            GrabInitial();
        }

        private void GrabInitial() {
            var spawned = false;
            foreach (Transform itr in transform) {
                var element = itr.GetComponent<BaseListElement>();
                if (element && (element.Data == null) && (element != ElementPrefab)) {
                    spawned = true;
                    var data = Property.AllocateNew();
                    ConfigureElement(element, data);
                }
            }

            if (spawned) {
                UpdateDatasOffScreens(true);
            }
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (Property != null) {
                Unsubscribe();
            }
        }

        private void Update() {
            if (_draggableElement != null) {
                DragElementToMousePosition();
                MoveBlankElementIfNeeded();
                MoveContentByDragIfNeeded();
            }

            if (_scrollRect != null) {
                AutoScrollProcess();
                UpdateDatasOffScreens();
            }
        }

        private void UpdateDatasOffScreens(bool force = false) {
            var pos = _content.anchoredPosition;
            var count = _elements.Count;
            if (!force && (pos == _prevPos) && (count == _prevCount)) {
                return;
            }

            GetTransformExtremePositions(_viewport, _scrollVertical, out float minViewportPosition, out float maxViewportPosition);
            foreach (var element in _elements) {
                var transform = element.transform as RectTransform;
                GetTransformExtremePositions(transform, _scrollVertical, out float minPosition, out float maxPosition);
                element.Data.UpdateOffScreen(minViewportPosition, maxViewportPosition, minPosition, maxPosition);
            }
            _prevPos = pos;
            _prevCount = count;
        }

        protected override void OnValueUpdated() {
            RebuildList();
        }

        protected override void OnForceUpdate() {
            Unsubscribe();

            base.OnForceUpdate();

            Subscribe();
        }
        
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
            _isDragging = true;
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
            _isDragging = false;
        }

        private BaseListElement InstantiateElement(BaseListElementData data, int index = -1) {
            var element = ElementPrefab.Spawn<BaseListElement>(Spawner == null ? _content : Spawner, false);
            if (index != -1) {
                element.transform.SetSiblingIndex(NewOnTop ? 0 : index);
            }

            return ConfigureElement(element, data);
        }

        private BaseListElement ConfigureElement(BaseListElement element, BaseListElementData data) {
            element.Init(data);
            element.gameObject.SetActive(true);

            if (_canDragElements) {
                var draggableElement = element as BaseDraggableListElement;
                draggableElement.OnPressed -= ElementPressHandler;
                draggableElement.OnPressed += ElementPressHandler;
                draggableElement.OnCanStartDragging -= ElementCanStartDragHandler;
                draggableElement.OnCanStartDragging += ElementCanStartDragHandler;
                draggableElement.OnCanEndDragging -= ElementCanEndDragHandler;
                draggableElement.OnCanEndDragging += ElementCanEndDragHandler;
            }

            return element;
        }

        private void DestroyElement(BaseListElement element) {
            if (_canDragElements) {
                var draggableElement = element as BaseDraggableListElement;
                draggableElement.OnPressed -= ElementPressHandler;
                draggableElement.OnCanStartDragging -= ElementCanStartDragHandler;
                draggableElement.OnCanEndDragging -= ElementCanEndDragHandler;
            }

            Destroy(element.gameObject);
        }

        private HashSet<BaseAnimatedListElement> GetVisibleElements(bool vertical) {
            GetTransformExtremePositions(_viewport, vertical, out float minViewportPosition, out float maxViewportPosition);

            var visibleElements = new HashSet<BaseAnimatedListElement>();
            for (int i = 0; i < _elements.Count; i++) {
                var element = _elements[i] as BaseAnimatedListElement;
                var transform = element.transform as RectTransform;
                GetTransformExtremePositions(transform, vertical, out float minPosition, out float maxPosition);

                if ((maxPosition > minViewportPosition) && (minPosition < maxViewportPosition)) {
                    visibleElements.Add(element);
                }
            }

            return visibleElements;
        }

        private void GetTransformExtremePositions(RectTransform transform, bool vertical, out float minPosition, out float maxPosition) {
            GetTransformAxisParameters(transform, vertical, out float position, out float pivot, out float size);

            minPosition = position - pivot * size;
            maxPosition = position + (1f - pivot) * size;
        }

        private void GetTransformAxisParameters(RectTransform transform, bool vertical, out float position, out float pivot, out float size) {
            if (vertical) {
                position = transform.position.y;
                pivot = transform.pivot.y;
                size = transform.rect.height * transform.lossyScale.y;
            }
            else {
                position = transform.position.x;
                pivot = transform.pivot.x;
                size = transform.rect.width * transform.lossyScale.x;
            }
        }

        private void CreateDraggableElementBlank(BaseListElement element) {
            var elementTransform = element.transform as RectTransform;
            var blank = new GameObject("DraggableElementBlank");
            _draggableElementBlank = blank.AddComponent<RectTransform>();
            _draggableElementBlank.SetParent(_content);
            _draggableElementBlank.SetSiblingIndex(_draggableElementIndex);
            _draggableElementBlank.anchorMax = elementTransform.anchorMax;
            _draggableElementBlank.anchorMin = elementTransform.anchorMin;
            _draggableElementBlank.pivot = elementTransform.pivot;
            _draggableElementBlank.sizeDelta = elementTransform.sizeDelta;
        }

        private void DragElementToMousePosition() {
            if (_scrollVertical) {
                _draggableElement.transform.position = new Vector3(_draggableElement.transform.position.x, Input.mousePosition.y);
            }
            else if (_scrollHorizontal) {
                _draggableElement.transform.position = new Vector3(Input.mousePosition.x, _draggableElement.transform.position.y);
            }
        }

        private void MoveBlankElementIfNeeded() {
            var blankIndex = _draggableElementBlank.GetSiblingIndex();
            var draggablePosition = _draggableElement.transform.position;
            for (int i = 0; i < _content.childCount; i++) {
                if (i == blankIndex) {
                    continue;
                }

                var prevPosition = _content.GetChild(i).position;
                var nextPosition = i < _content.childCount - 1 ? _content.GetChild(i + 1).position : GetExtremePosition(true, _content, 0f);

                if (i == 0) {
                    var extremePosition = GetExtremePosition(true, _content, 1f);
                    if (ElementPositionInRange(_scrollVertical, extremePosition, prevPosition, draggablePosition)) {
                        _draggableElementBlank.SetSiblingIndex(0);
                        break;
                    }
                }

                if (ElementPositionInRange(_scrollVertical, prevPosition, nextPosition, draggablePosition)) {
                    var index = blankIndex > i ? i + 1 : i;
                    _draggableElementBlank.SetSiblingIndex(index);
                    break;
                }
            }
        }

        private void MoveContentByDragIfNeeded() {
            var draggablePosition = _draggableElement.transform.position;
            if (_scrollVertical) {
                var topPoint = GetExtremePosition(true, _viewport, 1f);
                var bottomPoint = GetExtremePosition(true, _viewport, 0f);
                if ((draggablePosition.y > topPoint.y) && (_content.anchoredPosition.y > 0f)) {
                    _content.position = new Vector3(_content.position.x, _content.position.y - ScrollSpeedByDrag * Time.deltaTime);
                }

                if ((draggablePosition.y < bottomPoint.y) && (_content.anchoredPosition.y < (_content.rect.height - _viewport.rect.height))) {
                    _content.position = new Vector3(_content.position.x, _content.position.y + ScrollSpeedByDrag * Time.deltaTime);
                }
            }
            else if (_scrollHorizontal) { }
        }

        private bool ElementPositionInRange(bool vertical, Vector3 prevPosition, Vector3 nextPosition, Vector3 position) {
            return vertical
                       ? (prevPosition.y > position.y) && (position.y > nextPosition.y)
                       : (prevPosition.x > position.x) && (position.x > nextPosition.x);
        }

        private void AutoScrollProcess() {
            if (!_autoScrollController.IsActive) {
                return;
            }

            if (_autoScrollTargetReached || (_isDragging && (_scrollRect.horizontal || _scrollRect.vertical))) {
                _autoScrollTargetReached = _autoScrollCanBeInterrupted;
                _autoScrollController.Reset();
                return;
            }
            
            var value = Mathf.LerpUnclamped(_fromAutoScroll, _toAutoScroll, _autoScrollController.GetPhase().Ease(AutoscrollEaseType));
            _content.anchoredPosition = _scrollVertical ? _content.anchoredPosition.WithY(value) : _content.anchoredPosition.WithX(value);

            if (!_autoScrollTargetReached) {
                _autoScrollCanBeInterrupted = true;
            }
        }

        private void SetScrollActive(bool active) {
            if (active) {
                _scrollRect.vertical = _scrollVertical;
                _scrollRect.horizontal = _scrollHorizontal;
            }
            else {
                _scrollRect.vertical = false;
                _scrollRect.horizontal = false;
            }
        }

        private void ScrollToElement(int index, float time) {
            if (_scrollRect == null) {
                return;
            }

            _content.RebuildNearestLayout();

            var contentSize = _scrollVertical ? _content.rect.height : _content.rect.width;
            var viewportSize = _scrollVertical ? _viewport.rect.height : _viewport.rect.width;
            if ((contentSize > viewportSize) && (index >= 0)) {
                _autoScrollTargetReached = false;

                var rectTransform = _elements[index].transform as RectTransform;
                var oldContentPos = _content.anchoredPosition;
                _fromAutoScroll = (int) (_scrollVertical ? oldContentPos.y : oldContentPos.x);
                var newContentPos = GetAutoScrollContentPosition(rectTransform);
                _toAutoScroll = (int) (_scrollVertical ? newContentPos.y : newContentPos.x);
                var bound = (int)(_scrollVertical ? _content.rect.height - _viewport.rect.height : _content.rect.width - _viewport.rect.width);
                _toAutoScroll = _scrollVertical ? Mathf.Clamp(_toAutoScroll, 0, bound) : Mathf.Clamp(_toAutoScroll, -bound, 0);
            }
            else {
                _toAutoScroll = 0;
                time = 0f;
            }

            if (time.IsZero()) {
                _autoScrollTargetReached = true;
                _content.anchoredPosition = _scrollVertical ? _content.anchoredPosition.WithY(_toAutoScroll) : _content.anchoredPosition.WithX(_toAutoScroll);
                _scrollRect.velocity = Vector2.zero;
                UpdateDatasOffScreens(true);
                return;
            }

            if (time < 0f) {
                time = Mathf.Abs(0.5f * (_toAutoScroll - _fromAutoScroll) / viewportSize);
                time = Mathf.Max(0.5f, time);
            }

            _autoScrollController.Length = time;
            _autoScrollController.Reset();
            _autoScrollController.Play();
            _scrollRect.velocity = Vector2.zero;
        }

        private void GetPivotAndOffset(bool vertical, out float offset, out float pivot) {
            offset = 0f;
            pivot = 1f;
            switch (AutoScrollPivot) {
                case AutoScrollPivotTypeEnum.RightOrBottom:
                    offset += vertical ? _viewport.rect.height : _viewport.rect.width;
                    pivot = 0f;
                    break;

                case AutoScrollPivotTypeEnum.Center:
                    offset += vertical ? _viewport.rect.height / 2f : _viewport.rect.width / 2f;
                    pivot = 0.5f;
                    break;
            }
        }

        private Vector2 GetAutoScrollContentPosition(RectTransform rectTransform) {
            GetPivotAndOffset(_scrollVertical, out var offset, out _);
            var elementPos = _content.InverseTransformPoint(rectTransform.position);
            var contentPosition = Vector2.zero;
            if (_scrollVertical) {
                contentPosition.x = _content.anchoredPosition.x;
                contentPosition.y = -elementPos.y - offset;
            }
            else {
                contentPosition.x = -elementPos.x + offset;
                contentPosition.y = _content.anchoredPosition.y;
            }

            return contentPosition;
        }

        private Vector3 GetAutoScrollPosition(RectTransform rectTransform) {
            GetPivotAndOffset(_scrollVertical, out float offset, out float pivot);
            return GetExtremePosition(_scrollVertical, rectTransform, pivot, offset);
        }

        private Vector3 GetExtremePosition(bool vertical, RectTransform rectTransform, float pivot, float offset = 0f) {
            var position = Vector3.zero;
            if (vertical) {
                position.x = rectTransform.position.x;
                position.y = rectTransform.position.y + (pivot - rectTransform.pivot.y) * rectTransform.rect.height + offset;
            }
            else {
                position.x = rectTransform.position.x - (pivot - rectTransform.pivot.x) * rectTransform.rect.width + offset;
                position.y = rectTransform.position.y;
            }

            return position;
        }

        private void RebuildList() {
            if (ElementPrefab == null) {
                var i = 0;
                foreach (var elementData in Property.Value) {
                    var child = transform.GetChild(i);
                    var element = child.GetComponent<BaseListElement>();
                    if (element != null) {
                        ConfigureElement(element, elementData);
                    }

                    i++;
                }
                return;
            }
            
            _indexCache.Clear();
            var elementIndex = 0;
            foreach (var data in Property.Value) {
                while (_elements.Count > elementIndex && _inactiveElements.Contains(_elements[elementIndex])) {
                    elementIndex++;
                }

                if (_elements.Count > elementIndex && data == _elements[elementIndex].Data) {
                    elementIndex++;
                    continue;
                }

                var element = InstantiateElement(data, NewOnTop ? 0 : -1);
                
                if (_elements.Count > elementIndex) {
                    _elements.Insert(elementIndex, element);
                    elementIndex++;
                    _indexCache.Add(elementIndex);
                }
                else {
                    _elements.Add(element);
                }
                elementIndex++;
            }
            
            for (var i = _elements.Count - 1; i >= elementIndex; i--) {
                ElementRemoveHandler(i);
            }

            for (var i = _indexCache.Count - 1; i >= 0; i--) {
                ElementRemoveHandler(_indexCache[i]);
            }

            OnElementsCountChanged?.Invoke();
            UpdateDatasOffScreens(true);
        }

        private int CorrectListIndex(int index) {
            if (_inactiveElements.Count == 0) return index;
            
            for (var i = 0; i <= index && i < _elements.Count; i++) {
                if (!_inactiveElements.Contains(_elements[i])) continue;
                if (index <= i) index++;
            }
            return index < _elements.Count ? index : -1;
        }

        private (int, int) CorrectListIndices(int index1, int index2) {
            if (_inactiveElements.Count == 0) return (index1, index2);
            
            var max = Mathf.Max(index1, index2);
            for (var i = 0; i <= max && i < _elements.Count; i++) {
                if (!_inactiveElements.Contains(_elements[i])) continue;
                if (index1 <= i) index1++;
                if (index2 <= i) index1++;
                max++;
            }
            return (index1 < _elements.Count ? index1 : -1, index2 < _elements.Count ? index2 : -1);
        }

        private void Clear() {
            for (int i = 0; i < _elements.Count; i++) {
                DestroyElement(_elements[i]);
            }

            _elements.Clear();
            _inactiveElements.Clear();

            OnElementsCountChanged?.Invoke();
        }

        private void Subscribe() {
            Property.OnElementAdded += ElementAddHandler;
            Property.OnElementInserted += ElementInsertHandler;
            Property.OnElementRemoved += ElementRemoveHandler;
            Property.OnElementChanged += ElementChangeHandler;
            Property.OnElementMoved += ElementMoveHandler;
            Property.OnScrollToElement += ScrollToElementHandler;
            Property.OnFullReorder += FullReorderHandler;
        }

        private void Unsubscribe() {
            Property.OnElementAdded -= ElementAddHandler;
            Property.OnElementInserted -= ElementInsertHandler;
            Property.OnElementRemoved -= ElementRemoveHandler;
            Property.OnElementChanged -= ElementChangeHandler;
            Property.OnElementMoved -= ElementMoveHandler;
            Property.OnScrollToElement -= ScrollToElementHandler;
            Property.OnFullReorder -= FullReorderHandler;
        }

        private void ElementAddHandler(BaseListElementData data) {
            var element = InstantiateElement(data, NewOnTop ? 0 : -1);
            _elements.Add(element);

            if (_canAnimateElements) ((BaseAnimatedListElement) element).ShowAsync(instant: false);
            OnElementsCountChanged?.Invoke();
        }

        private void ElementInsertHandler(int index, BaseListElementData data) {
            index = CorrectListIndex(index);
            var element = InstantiateElement(data, NewOnTop ? 0 : index);
            _elements.Insert(index, element);

            if (_canAnimateElements) ((BaseAnimatedListElement) element).ShowAsync(instant: false);
            OnElementsCountChanged?.Invoke();
        }

        private async void ElementRemoveHandler(int index) {
            var element = _elements[index];

            if (_canAnimateElements) {
                _inactiveElements.Add(element);
                
                OnElementsCountChanged?.Invoke();
                
                await ((BaseAnimatedListElement) element).HideAsync(instant: false);
                if (this == null) return;
                
                _inactiveElements.Remove(element);
            }
            _elements.Remove(element);
            DestroyElement(element);
        }

        private void ElementChangeHandler(int index, BaseListElementData data) {
            index = CorrectListIndex(index);
            var element = _elements[index];
            element.Init(data);
        }

        private void ElementMoveHandler(int index, int position) {
            var (listIndex, listPosition) = CorrectListIndices(index, position);

            if (listIndex < 0 || listPosition < 0) {
                Debug.Log($"{nameof(ListBinding)}> Unable to move element. Index {Mathf.Max(index, position)} is out of range!");
                return;
            }
            var element = _elements[listIndex];
            _elements.RemoveAt(listIndex);
            _elements.Insert(listPosition, element);
            element.transform.SetSiblingIndex(listPosition);
        }

        private void ScrollToElementHandler(int index, float time) {
            if (_scrollRect != null) {
                ScrollToElement(index, time);
            }
        }

        private void FullReorderHandler() {
            var elementBinds = new Dictionary<BaseListElementData, BaseListElement>();
            foreach (var itr in _elements) {
                elementBinds.Add(itr.Data, itr);
            }

            _elements.Clear();
            foreach (var data in Property.Value) {
                _elements.Add(elementBinds[data]);
                elementBinds[data].transform.SetAsLastSibling();
            }
        }

        private void ElementPressHandler(BaseDraggableListElement element) {
            if (!_canDragElements) {
                return;
            }

            _contentStartPosition = _content.anchoredPosition;
        }

        private void ElementCanStartDragHandler(BaseDraggableListElement element) {
            if (!_canDragElements) {
                return;
            }

            var contentDistance = (_contentStartPosition - _content.anchoredPosition).magnitude;
            if (contentDistance < DragTolerance) {
                _draggableElementIndex = element.transform.GetSiblingIndex();
                _draggableElement = element;
                element.transform.SetParent(_viewport);
                CreateDraggableElementBlank(element);

                element.StartDrag();
                SetScrollActive(false);
            }
        }

        private void ElementCanEndDragHandler(BaseDraggableListElement element) {
            if (!_canDragElements) {
                return;
            }

            if (_draggableElement != null) {
                var blankIndex = _draggableElementBlank.GetSiblingIndex();
                Destroy(_draggableElementBlank.gameObject);
                _draggableElementBlank = null;

                _draggableElement.transform.SetParent(_content);
                _draggableElement = null;

                Property.Move(element.Data, blankIndex);

                element.EndDrag();
                SetScrollActive(true);
            }
        }
        
        private enum AutoScrollPivotTypeEnum {
            LeftOrTop,
            RightOrBottom,
            Center
        }
    }
}