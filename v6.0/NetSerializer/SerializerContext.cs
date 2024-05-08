using NetSerializer.V6.Formaters;

namespace NetSerializer {

    internal class SerializerContext {

        private readonly IFormatWriter _writer;
        private readonly List<object> _items = [];

        public SerializerContext(IFormatWriter writer) {

            _writer = writer;
        }

        public bool GetObjectId(object obj, out int id) {

            id = _items.IndexOf(obj);
            if (id >= 0)
                return true;
            else {
                _items.Add(obj);
                id = _items.Count - 1;
                return false;
            }
        }

        public IFormatWriter Writer =>
            _writer;
    }
}
