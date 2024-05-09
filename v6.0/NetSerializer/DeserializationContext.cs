using NetSerializer.V6.Formaters;

namespace NetSerializer.V6 {

    public sealed class DeserializationContext: IDeserializationReader {
        
        private readonly FormatReader _reader;
        private readonly List<object> _items = [];
        
        public DeserializationContext(FormatReader reader) {
            
            _reader = reader;
        }
        
        /// <inherited/>
        ///
        public bool ReadBool(string name) {
            
            return _reader.ReadBool(name);
        }
        
        /// <inherited/>
        ///
        public int ReadInt(string name) {
            
            return _reader.ReadInt(name);
        }
        
        /// <inherited/>
        ///
        public float ReadSingle(string name) {
            
            return _reader.ReadSingle(name);
        }
        
        /// <inherited/>
        ///
        public double ReadDouble(string name) {
            
            return _reader.ReadDouble(name);
        }
        
        /// <inherited/>
        ///
        public object? ReadObject(string name) {

            return null;
        }
        
        private object GetObject(int id) {
            
            return _items[id];
        }
    }
}
