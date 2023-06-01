using System;
using System.Collections;
using Core.Bindings.Tools.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Binding.Elements {
    public class BaseDraggableListElement : BaseListElement, IPointerDownHandler {
        public event Action<BaseDraggableListElement> OnPressed;
        public event Action<BaseDraggableListElement> OnCanStartDragging;
        public event Action<BaseDraggableListElement> OnCanEndDragging;

        private static readonly int StartDragTrigger = Animator.StringToHash("StartDrag");
        private static readonly int EndDragTrigger = Animator.StringToHash("EndDrag");

        [SerializeField] private float _dragInitiationTime = 1f;
        [SerializeField] private float _clickTolerance = 5f;

        protected Animator Animator;

        private Vector3 _pointerDownPosition;
        private bool _isPointerDown = false;
        private bool _dragInProgress = false;

        protected virtual void Awake() {
            Animator = GetComponent<Animator>();
        }

        private void OnDestroy() {
            StopAllCoroutines();
        }

        public virtual void StartDrag() {
            if ((Animator != null) && (Animator.HasParameter(StartDragTrigger))) {
                Animator.SetTrigger(StartDragTrigger);
            }
        }

        public virtual void EndDrag() {
            if ((Animator != null) && (Animator.HasParameter(EndDragTrigger))) {
                Animator.SetTrigger(EndDragTrigger);
            }
        }

        private void Update() {
            if (Input.GetMouseButtonUp(0)) {
                if (_dragInProgress) {
                    _dragInProgress = false;

                    OnCanEndDragging?.Invoke(this);
                }
                else if (_isPointerDown && ((_pointerDownPosition - transform.position).magnitude < _clickTolerance)) {
                    Data.ClickFromUI();
                }

                _isPointerDown = false;
            }
        }

        private IEnumerator DragInitiationCoroutine() {
            var startTime = Time.time;
            while (_isPointerDown && (Time.time < startTime + _dragInitiationTime)) {
                yield return null;
            }

            if (_isPointerDown) {
                _dragInProgress = true;

                OnCanStartDragging?.Invoke(this);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            _pointerDownPosition = transform.position;
            _isPointerDown = true;

            OnPressed?.Invoke(this);

            StartCoroutine(DragInitiationCoroutine());
        }
    }
}