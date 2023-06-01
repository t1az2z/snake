using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectNormalizedPositionBinding : BaseBinding<ScrollRectNormalizedPositionProperty> {
        [SerializeField] private ModeEnum Mode;
        [SerializeField] private OrientationEnum Orientation;

        private ScrollRect _scrollRect;

        protected override void Awake() {
            _scrollRect = GetComponent<ScrollRect>();
            base.Awake();
            if (Mode == ModeEnum.Write) {
                _scrollRect.onValueChanged.AddListener(OnScrollChanged);
            }
        }

        private void OnScrollChanged(Vector2 position) {
            Property.Value = Orientation == OrientationEnum.Horizontal ? _scrollRect.horizontalNormalizedPosition : _scrollRect.verticalNormalizedPosition;
        }

        protected override void OnValueUpdated() {
            if (Mode != ModeEnum.Read && !Property.ForceWrite)
                return;

            if (Orientation == OrientationEnum.Horizontal)
                _scrollRect.horizontalNormalizedPosition = Property.Value;
            else
                _scrollRect.verticalNormalizedPosition = Property.Value;
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            if(Mode == ModeEnum.Write)
                _scrollRect.onValueChanged.RemoveListener(OnScrollChanged);
        }

        private enum ModeEnum {
            Read = 0,
            Write = 1
        }

        private enum OrientationEnum {
            Horizontal = 0,
            Vertical = 1
        }
    }

    public class ScrollRectNormalizedPositionProperty : FloatProperty {
        public bool ForceWrite { get; private set; }
        public void SetForce(float normalizedPosition) {
            ForceWrite = true;
            Value = normalizedPosition;
            ForceWrite = false;
        }
    }
}