using Binding;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class LayoutBinding : BaseBinding<FloatProperty> {
        [SerializeField] private ParameterTypeEnum ParameterType = ParameterTypeEnum.Spacing;

        private HorizontalOrVerticalLayoutGroup _group;
        private LayoutElement _element;

        protected override void Awake() {
            _group = GetComponent<HorizontalOrVerticalLayoutGroup>();
            _element = GetComponent<LayoutElement>();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            switch (ParameterType) {
                case ParameterTypeEnum.Spacing:
                    if (_group != null) {
                        _group.spacing = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.MinWidth:
                    if (_element != null) {
                        _element.minWidth = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.MinHeight:
                    if (_element != null) {
                        _element.minHeight = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.PreferredWidth:
                    if (_element != null) {
                        _element.preferredWidth = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.PreferredHeight:
                    if (_element != null) {
                        _element.preferredHeight = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.FlexibleWidth:
                    if (_element != null) {
                        _element.flexibleWidth = Property.Value;
                    }

                    break;

                case ParameterTypeEnum.FlexibleHeight:
                    if (_element != null) {
                        _element.flexibleHeight = Property.Value;
                    }

                    break;
            }
        }
        
        private enum ParameterTypeEnum {
            Spacing,
            MinWidth,
            MinHeight,
            PreferredWidth,
            PreferredHeight,
            FlexibleWidth,
            FlexibleHeight
        }
    }
}