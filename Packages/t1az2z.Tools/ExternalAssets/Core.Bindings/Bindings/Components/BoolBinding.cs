using System.Globalization;
using Binding;
using Binding.Base;
using Core.Bindings.Tools.Extensions;
using Core.Bindings.Tools.Helpers;
using UnityEngine;

namespace Core.Bindings.Components {
    public class BoolBinding : BaseBinding {
        [SerializeField] private CheckTypeEnum CheckType = CheckTypeEnum.Boolean;
        [SerializeField] private string RefValue = "0";
        [SerializeField] private bool Invert = false;

        private float[] _parsedRefs;
        private bool? _boolValue;

        protected override void Awake() {
            var vals = RefValue.Split(',', ' ');
            _parsedRefs = new float[vals.Length];
            for (var i = 0; i < vals.Length; ++i) {
                _parsedRefs[i] = float.TryParse(vals[i], NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0f;
            }
            base.Awake();
        }

        protected override void Bind(bool total = false) {
            base.Bind(total);
            if (Property is ICountOfListProperty icolp) {
                icolp.OnCountChanged += OnPropertyValueUpdated;
            }
        }

        protected override void Unbind(bool total = false) {
            if (Property is ICountOfListProperty icolp) {
                icolp.OnCountChanged -= OnPropertyValueUpdated;
            }
            base.Unbind(total);
        }

        public bool BoolValue => _boolValue.Value;

        protected override void OnPropertyValueUpdated() {
            var prevValue = _boolValue;
            var newValue = _boolValue;

            switch (Property) {
                case BoolProperty bp:
                    newValue = bp.Value;
                    break;

                case StringProperty sp:
                    newValue = StringProcess(sp.Value);
                    break;

                case IntProperty ip:
                    newValue = FloatProcess(ip.Value);
                    break;

                case ICountOfListProperty ip:
                    newValue = FloatProcess(ip.Count);
                    break;

                case FloatProperty ip:
                    newValue = FloatProcess(ip.Value);
                    break;

                case IProperty ip when (CheckType == CheckTypeEnum.EmptyNull):
                    newValue = ip.IsEmpty();
                    break;
            }

            if (Invert) {
                newValue = !newValue;
            }

            if (newValue != prevValue) {
                _boolValue = newValue;
                OnValueUpdated();
            }
        }

        private bool FloatProcess(float val) {
            switch (CheckType) {
                case CheckTypeEnum.Boolean: return !MathTools.IsZero(val);
                case CheckTypeEnum.EmptyNull: return MathTools.IsZero(val);

                case CheckTypeEnum.Equal: {
                    var result = false;
                    foreach (var itr in _parsedRefs) {
                        result = result || MathTools.IsZero(val - itr);
                    }
                    return result;
                }

                case CheckTypeEnum.Greater: {
                    var result = false;
                    foreach (var itr in _parsedRefs) {
                        result = result || (val > itr + MathTools.Epsilon);
                    }
                    return result;
                }

                case CheckTypeEnum.Less: {
                    var result = false;
                    foreach (var itr in _parsedRefs) {
                        result = result || (val < itr - MathTools.Epsilon);
                    }
                    return result;
                }

                case CheckTypeEnum.InRange: {
                    var (min, max) = GetTwoRefs();
                    return (val >= min - MathTools.Epsilon) && (val <= max + MathTools.Epsilon);
                }

                default:
                    return !MathTools.IsZero(val);
            }

            (double, double) GetTwoRefs() {
                if (_parsedRefs.Length < 2) {
                    return (0d, 0d);
                }

                var min = _parsedRefs[0];
                var max = _parsedRefs[1];
                if (min > max) {
                    (min, max) = (max, min);
                }

                return (min, max);
            }
        }

        private bool StringProcess(string str) {
            switch (CheckType) {
                case CheckTypeEnum.Boolean: return str.ToLower() is var lowered && !lowered.IsOneOf("n", "0", "-", "false");
                case CheckTypeEnum.EmptyNull: return string.IsNullOrEmpty(str);
                case CheckTypeEnum.Equal: return str == RefValue;
                case CheckTypeEnum.Greater: return str.CompareTo(RefValue) > 0;
                case CheckTypeEnum.Less: return str.CompareTo(RefValue) < 0;

                default:
                    return string.IsNullOrEmpty(str);
            }
        }

        public enum CheckTypeEnum {
            Boolean,
            Equal,
            Greater,
            Less,
            InRange,
            EmptyNull,
        }
    }
}