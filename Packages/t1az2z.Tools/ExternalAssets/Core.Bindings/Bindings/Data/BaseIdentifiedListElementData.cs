namespace Binding {
    public abstract class BaseIdentifiedListElementData : BaseListElementData {
        public virtual int Id {
            get { return _id; }
            set { _id = value; }
        }

        private int _id;
    }
}