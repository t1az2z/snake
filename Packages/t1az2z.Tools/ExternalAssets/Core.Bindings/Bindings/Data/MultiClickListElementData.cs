using System;

namespace Binding.Data {
    public class MultiClickListElementData : BaseListElementData {
        public event Action<MultiClickListElementData> OnClick1;

        public event Action<MultiClickListElementData> OnClick2;

        public event Action<MultiClickListElementData> OnClick3;

        public event Action<MultiClickListElementData> OnClick4;

        public event Action<MultiClickListElementData> OnClick5;

        public void Click1() {
            OnClick1?.Invoke(this);
        }

        public void Click2() {
            OnClick2?.Invoke(this);
        }

        public void Click3() {
            OnClick3?.Invoke(this);
        }

        public void Click4() {
            OnClick4?.Invoke(this);
        }

        public void Click5() {
            OnClick5?.Invoke(this);
        }
    }
}