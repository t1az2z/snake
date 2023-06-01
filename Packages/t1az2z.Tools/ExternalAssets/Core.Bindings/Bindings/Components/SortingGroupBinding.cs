using Binding;
using UnityEngine;
using UnityEngine.Rendering;

namespace Core.Bindings.Components {
    [RequireComponent(typeof(SortingGroup))]
    public class SortingGroupBinding : BaseBinding<IntProperty> {
        [SerializeField] private bool CaptureOffset;
        
        private SortingGroup _group;
        private int _offset = 0;

        protected override void Awake() {
            _group = GetComponent<SortingGroup>();
            if (CaptureOffset) _offset = _group.sortingOrder;
            base.Awake();
        }

        protected override void OnValueUpdated() {
            _group.sortingOrder = Property.Value + _offset;
        }
    }
}