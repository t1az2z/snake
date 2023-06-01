using System.Text;
using Binding.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Bindings.Components {
    public class TextBinding : BaseBinding<IProperty> {
        [SerializeField] private string FormatString = "";
        [SerializeField] private bool ForceUpperCase = false;        
        [SerializeField] private uint CharacterLimit = 0;
        [SerializeField] private CharacterLimitModeEnum CharacterLimitMode;

        private Text _text = null;
        private TMP_Text _sdfText = null;
        private InputField _inputField = null;

        protected override void Awake() {
            if ((_text = GetComponent<Text>()) == null && (_sdfText = GetComponent<TMP_Text>()) == null)
                _inputField = GetComponent<InputField>();
        
            FormatString = FormatString?.Trim();
            base.Awake();
        }

        protected override void OnValueUpdated() {
            var val = Property.ToString();
            if (!string.IsNullOrEmpty(val) && ForceUpperCase) {
                val = val.ToUpper();
            }

            if (!string.IsNullOrEmpty(FormatString)) {
                val = string.Format(FormatString, val);
            }

            if (!string.IsNullOrEmpty(val) && CharacterLimit > 0 && val.Length > CharacterLimit) {
                var sb = new StringBuilder(val);
                if (CharacterLimitMode == CharacterLimitModeEnum.StripLeft) {
                    var startIndex = val.Length - (int) CharacterLimit;
                    val = sb.ToString(startIndex, val.Length - startIndex);
                }
                else {
                    val = sb.ToString(0, (int) CharacterLimit);
                }
            }

            if (_text != null) {
                _text.text = val;
            }
            else if (_sdfText != null) {
                _sdfText.text = val;
            }
            else if (_inputField != null) {
                _inputField.text = val;
            }
        }

        private enum CharacterLimitModeEnum {
            StripRight = 0,
            StripLeft = 1
        }
    }
}