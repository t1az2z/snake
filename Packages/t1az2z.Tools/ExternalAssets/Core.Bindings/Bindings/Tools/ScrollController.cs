using System;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Bindings.Scroll {
    [Serializable]
    public class ScrollController {
        public RectTransform Content { get { return _content; } set { _content = value; } }
        public RectTransform Viewport { get { return _viewport; } set { _viewport = value; } }
        public MovementTypeEnum MovementType { get { return _movementType; } set { _movementType = value; } }


        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private MovementTypeEnum _movementType = MovementTypeEnum.Elastic;

        [SerializeField] private float _elasticity = 0.1f;
        [SerializeField] private bool _inertia = true;
        [SerializeField] private float _decelerationRate = 0.135f; // Only used when inertia is enabled
        [SerializeField] private float _scrollSensitivity = 1.0f;

        [SerializeField] private Scrollbar _scrollbar;
        public Scrollbar Scrollbar => _scrollbar;

        [SerializeField] private ScrollbarVisibilityEnum _scrollbarVisibility;

        private Bounds _contentBounds;
        private Bounds _viewBounds;

        private Vector2 _velocity;
        public Vector2 Velocity { get { return _velocity; } set { _velocity = value; } }

        public bool IsScrollbarDragged { private set; get; }
        private bool _isDragging;
        private float _initialLocalDragPoint;
        private float _lastLocalDragPoint;
        private bool _isScrolling;

        private Vector2 _prevPosition = Vector2.zero;
        private Bounds _prevContentBounds;
        private Bounds _prevViewBounds;

        private bool _isVertical = true;

        public void Initialize(bool isVertical) {
            this._isVertical = isVertical;
        }

        public void OnEnable() {
            _scrollbar?.onValueChanged.AddListener(SetNormalizedPosition);
            if (_scrollbar?.GetOrAddComponent<ScrollbarStateTracker>() is var tracker && tracker) {
                tracker.OnDragStateChanged += OnScrollbarCaptured;
            }
        }

        public void OnDisable() {
            _scrollbar?.onValueChanged.RemoveListener(SetNormalizedPosition);
            if (_scrollbar?.GetOrAddComponent<ScrollbarStateTracker>() is var tracker && tracker) {
                tracker.OnDragStateChanged -= OnScrollbarCaptured;
            }

            _isScrolling = false;
            _velocity = Vector2.zero;
        }

        private void OnScrollbarCaptured(bool state) {
            IsScrollbarDragged = state;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _isDragging = true;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewport, eventData.position, eventData.pressEventCamera, out var localPoint);
            _initialLocalDragPoint = _isVertical ? localPoint.y : localPoint.x;
        }

        public void OnDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_viewport, eventData.position, eventData.pressEventCamera, out var localPoint);
            _lastLocalDragPoint = _isVertical ? localPoint.y : localPoint.x;
        }

        public void OnEndDrag(PointerEventData eventData) {
            _isDragging = false;
        }

        public void StopMovement() {
            _velocity = Vector2.zero;
        }

        public void OnScroll(PointerEventData data) {
            UpdateBounds();

            Vector2 delta = data.scrollDelta;
            // Down is positive for scroll events, while in UI system up is positive.
            delta.y *= -1;
            if (_isVertical) {
                if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
                    delta.y = delta.x;
                delta.x = 0;
            }
            else {
                if (Mathf.Abs(delta.y) > Mathf.Abs(delta.x))
                    delta.x = delta.y;
                delta.y = 0;
            }

            if (data.IsScrolling())
                _isScrolling = true;

            Vector2 position = _content.anchoredPosition;
            position += delta * _scrollSensitivity;
            if (_movementType == MovementTypeEnum.Clamped)
                position += CalculateOffset(position - _content.anchoredPosition);

            SetContentAnchoredPosition(position);
            UpdateBounds();
        }

        /// <summary>
        /// Sets the anchored position of the content.
        /// </summary>
        protected void SetContentAnchoredPosition(Vector2 position) {
            if (_isVertical) {
                position.x = _content.anchoredPosition.x;
            }
            else {
                position.y = _content.anchoredPosition.y;
            }

            if (position != _content.anchoredPosition) {
                _content.anchoredPosition = position;
                UpdateBounds();
            }
        }

        public void UpdateScrollbar() {
            if (!_content)
                return;

            UpdateBounds();
            float deltaTime = Time.unscaledDeltaTime;
            var axis = _isVertical ? 1 : 0;
            Vector2 offset = CalculateOffset(Vector2.zero);
            if (!_isDragging && (offset != Vector2.zero || _velocity != Vector2.zero)) {
                Vector2 position = _content.anchoredPosition;
                // Apply spring physics if movement is elastic and content has an offset from the view.
                if (_movementType == MovementTypeEnum.Elastic && offset[axis] != 0) {
                    float speed = _velocity[axis];
                    float smoothTime = _elasticity;
                    if (_isScrolling)
                        smoothTime *= 3.0f;
                    position[axis] = Mathf.SmoothDamp(_content.anchoredPosition[axis], _content.anchoredPosition[axis] + offset[axis], ref speed, smoothTime, Mathf.Infinity, deltaTime);
                    if (Mathf.Abs(speed) < 1)
                        speed = 0;
                    _velocity[axis] = speed;
                }
                // Else move content according to velocity with deceleration applied.
                else if (_inertia) {
                    _velocity[axis] *= Mathf.Pow(_decelerationRate, deltaTime);
                    if (Mathf.Abs(_velocity[axis]) < 1)
                        _velocity[axis] = 0;
                    position[axis] += _velocity[axis] * deltaTime;
                }
                // If we have neither elaticity or friction, there shouldn't be any velocity.
                else {
                    _velocity[axis] = 0;
                }

                if (_movementType == MovementTypeEnum.Clamped) {
                    offset = CalculateOffset(position - _content.anchoredPosition);
                    position += offset;
                }

                SetContentAnchoredPosition(position);
            }

            if (_isDragging) {
                Vector2 position = _content.anchoredPosition;
                position[axis] += (_lastLocalDragPoint - _initialLocalDragPoint);
                SetContentAnchoredPosition(position);
                _initialLocalDragPoint = _lastLocalDragPoint;
            }

            if (_isDragging && _inertia) {
                Vector3 newVelocity = (_content.anchoredPosition - _prevPosition) / deltaTime;
                _velocity = Vector3.Lerp(_velocity, newVelocity, deltaTime * 10);
            }

            if (_viewBounds != _prevViewBounds || _contentBounds != _prevContentBounds || _content.anchoredPosition != _prevPosition) {
                UpdateScrollbars(offset);
                UpdatePrevData();
            }
            UpdateScrollbarVisibility();
            _isScrolling = false;
        }

        /// <summary>
        /// Helper function to update the previous data fields on a ScrollRect. Call this before you change data in the ScrollRect.
        /// </summary>
        protected void UpdatePrevData() {
            if (_content == null)
                _prevPosition = Vector2.zero;
            else
                _prevPosition = _content.anchoredPosition;
            _prevViewBounds = _viewBounds;
            _prevContentBounds = _contentBounds;
        }

        private void UpdateScrollbars(Vector2 offset) {
            if (!_scrollbar) {
                return;
            }

            if (_isVertical) {
                _scrollbar.size = (_contentBounds.size.y > 0) ? Mathf.Clamp01((_viewBounds.size.y - Mathf.Abs(offset.y)) / _contentBounds.size.y) : 1;
                _scrollbar.value = verticalNormalizedPosition;
            } else {
                _scrollbar.size = (_contentBounds.size.x > 0) ? Mathf.Clamp01((_viewBounds.size.x - Mathf.Abs(offset.x)) / _contentBounds.size.x) : 1;
                _scrollbar.value = horizontalNormalizedPosition;
            }
        }

        public float horizontalNormalizedPosition {
            get {
                UpdateBounds();
                if ((_contentBounds.size.x <= _viewBounds.size.x) || Mathf.Approximately(_contentBounds.size.x, _viewBounds.size.x))
                    return (_viewBounds.min.x > _contentBounds.min.x) ? 1 : 0;
                return (_viewBounds.min.x - _contentBounds.min.x) / (_contentBounds.size.x - _viewBounds.size.x);
            }
        }

        public float verticalNormalizedPosition {
            get {
                UpdateBounds();
                if ((_contentBounds.size.y <= _viewBounds.size.y) || Mathf.Approximately(_contentBounds.size.y, _viewBounds.size.y))
                    return (_viewBounds.min.y > _contentBounds.min.y) ? 1 : 0;

                return (_viewBounds.min.y - _contentBounds.min.y) / (_contentBounds.size.y - _viewBounds.size.y);
            }
        }

        protected void SetNormalizedPosition(float value) {
            var axis = _isVertical ? 1 : 0;
            UpdateBounds();
            // How much the content is larger than the view.
            float hiddenLength = _contentBounds.size[axis] - _viewBounds.size[axis];
            // Where the position of the lower left corner of the content bounds should be, in the space of the view.
            float contentBoundsMinPosition = _viewBounds.min[axis] - value * hiddenLength;
            // The new content localPosition, in the space of the view.
            float newAnchoredPosition = _content.anchoredPosition[axis] + contentBoundsMinPosition - _contentBounds.min[axis];

            Vector3 anchoredPosition = _content.anchoredPosition;
            if (Mathf.Abs(anchoredPosition[axis] - newAnchoredPosition) > 0.01f) {
                anchoredPosition[axis] = newAnchoredPosition;
                _content.anchoredPosition = anchoredPosition;
                _velocity[axis] = 0;
                UpdateBounds();
            }
        }

        private bool isScrollingNeeded {
            get {
                if (Application.isPlaying) {
                    return _isVertical ? (_contentBounds.size.y > _viewBounds.size.y + 0.01f) : (_contentBounds.size.x > _viewBounds.size.x + 0.01f);
                }
                return true;
            }
        }

        private void UpdateScrollbarVisibility() {
            if (!_scrollbar) {
                return;
            }

            var visibility = (_scrollbarVisibility == ScrollbarVisibilityEnum.Permanent) || isScrollingNeeded;
            if (_scrollbar.gameObject.activeSelf != visibility) {
                _scrollbar.gameObject.SetActive(visibility);
            }
        }

        protected void UpdateBounds() {
            _viewBounds = new Bounds(_viewport.rect.center, _viewport.rect.size);
            _contentBounds = GetBounds();

            if (_content == null)
                return;

            Vector3 contentSize = _contentBounds.size;
            Vector3 contentPos = _contentBounds.center;
            var contentPivot = _content.pivot;
            AdjustBounds(ref _viewBounds, ref contentPivot, ref contentSize, ref contentPos);
            _contentBounds.size = contentSize;
            _contentBounds.center = contentPos;

            if (MovementType == MovementTypeEnum.Clamped) {
                // Adjust content so that content bounds bottom (right side) is never higher (to the left) than the view bounds bottom (right side).
                // top (left side) is never lower (to the right) than the view bounds top (left side).
                // All this can happen if content has shrunk.
                // This works because content size is at least as big as view size (because of the call to InternalUpdateBounds above).
                Vector2 delta = Vector2.zero;
                if (_viewBounds.max.x > _contentBounds.max.x) {
                    delta.x = Math.Min(_viewBounds.min.x - _contentBounds.min.x, _viewBounds.max.x - _contentBounds.max.x);
                }
                else if (_viewBounds.min.x < _contentBounds.min.x) {
                    delta.x = Math.Max(_viewBounds.min.x - _contentBounds.min.x, _viewBounds.max.x - _contentBounds.max.x);
                }

                if (_viewBounds.min.y < _contentBounds.min.y) {
                    delta.y = Math.Max(_viewBounds.min.y - _contentBounds.min.y, _viewBounds.max.y - _contentBounds.max.y);
                }
                else if (_viewBounds.max.y > _contentBounds.max.y) {
                    delta.y = Math.Min(_viewBounds.min.y - _contentBounds.min.y, _viewBounds.max.y - _contentBounds.max.y);
                }
                if (delta.sqrMagnitude > float.Epsilon) {
                    contentPos = _content.anchoredPosition + delta;
                    if (_isVertical) {
                        contentPos.x = _content.anchoredPosition.x;
                    }
                    else {
                        contentPos.y = _content.anchoredPosition.y;
                    }
                    AdjustBounds(ref _viewBounds, ref contentPivot, ref contentSize, ref contentPos);
                }
            }
        }

        internal static void AdjustBounds(ref Bounds viewBounds, ref Vector2 contentPivot, ref Vector3 contentSize, ref Vector3 contentPos) {
            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 excess = viewBounds.size - contentSize;
            if (excess.x > 0) {
                contentPos.x -= excess.x * (contentPivot.x - 0.5f);
                contentSize.x = viewBounds.size.x;
            }
            if (excess.y > 0) {
                contentPos.y -= excess.y * (contentPivot.y - 0.5f);
                contentSize.y = viewBounds.size.y;
            }
        }

        private readonly Vector3[] _corners = new Vector3[4];
        private Bounds GetBounds() {
            if (_content == null)
                return new Bounds();
            _content.GetWorldCorners(_corners);
            var viewWorldToLocalMatrix = _viewport.worldToLocalMatrix;
            return InternalGetBounds(_corners, ref viewWorldToLocalMatrix);
        }

        internal static Bounds InternalGetBounds(Vector3[] corners, ref Matrix4x4 viewWorldToLocalMatrix) {
            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            for (int j = 0; j < 4; j++) {
                Vector3 v = viewWorldToLocalMatrix.MultiplyPoint3x4(corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Vector2 CalculateOffset(Vector2 delta) {
            return InternalCalculateOffset(ref _viewBounds, ref _contentBounds, !_isVertical, _isVertical, _movementType, ref delta);
        }

        internal static Vector2 InternalCalculateOffset(ref Bounds viewBounds, ref Bounds contentBounds, bool horizontal, bool vertical, MovementTypeEnum movementType, ref Vector2 delta) {
            Vector2 offset = Vector2.zero;
            if (movementType == MovementTypeEnum.Unrestricted)
                return offset;

            Vector2 min = contentBounds.min;
            Vector2 max = contentBounds.max;

            // min/max offset extracted to check if approximately 0 and avoid recalculating layout every frame (case 1010178)

            if (horizontal) {
                min.x += delta.x;
                max.x += delta.x;

                float maxOffset = viewBounds.max.x - max.x;
                float minOffset = viewBounds.min.x - min.x;

                if (minOffset < -0.001f)
                    offset.x = minOffset;
                else if (maxOffset > 0.001f)
                    offset.x = maxOffset;
            }

            if (vertical) {
                min.y += delta.y;
                max.y += delta.y;

                float maxOffset = viewBounds.max.y - max.y;
                float minOffset = viewBounds.min.y - min.y;

                if (maxOffset > 0.001f)
                    offset.y = maxOffset;
                else if (minOffset < -0.001f)
                    offset.y = minOffset;
            }

            return offset;
        }

        public enum MovementTypeEnum {
            Unrestricted,
            Elastic,
            Clamped,
        }

        public enum ScrollbarVisibilityEnum {
            Permanent,
            AutoHide,
            AutoHideAndExpandViewport,
        }

        private class ScrollbarStateTracker : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {
            public event Action<bool> OnDragStateChanged;
            public void OnBeginDrag(PointerEventData eventData) {
                OnDragStateChanged?.Invoke(true);
            }

            public void OnDrag(PointerEventData eventData) {
            }

            public void OnEndDrag(PointerEventData eventData) {
                OnDragStateChanged?.Invoke(false);
            }

            public void OnInitializePotentialDrag(PointerEventData eventData) {
            }
        }
    }
}