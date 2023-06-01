using Binding.Data;

namespace Binding.Elements {
    public class MultiClickListElement : BaseListElement {
        private MultiClickListElementData _data = null;

        protected override void OnInit() {
            _data = Data as MultiClickListElementData;
        }

        public virtual void Button1ClickHandler() {
            if (_data != null) {
                _data.Click1();
            }
        }

        public virtual void Button2ClickHandler() {
            if (_data != null) {
                _data.Click2();
            }
        }

        public virtual void Button3ClickHandler() {
            if (_data != null) {
                _data.Click3();
            }
        }

        public virtual void Button4ClickHandler() {
            if (_data != null) {
                _data.Click4();
            }
        }

        public virtual void Button5ClickHandler() {
            if (_data != null) {
                _data.Click5();
            }
        }
    }
}