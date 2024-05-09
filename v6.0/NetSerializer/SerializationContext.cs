using NetSerializer.V6.Formaters;
using NetSerializer.V6.TypeSerializers;

namespace NetSerializer {

    internal class SerializationContext: ISerializationWriter {

        private readonly FormatWriter _writer;
        private readonly List<object> _items = [];

        public SerializerContext(FormatWriter writer) {

            _writer = writer;
        }

        /// <inherited/>
        ///
        void WriteBool(string name, bool value) {
         
            _writer.WriteBool(name, value);
        }

        /// <inherited/>
        ///
        void WriteInt(string name, int value) {
            
            _writer.WriteInt(name, value);
        }

        /// <inherited/>
        ///
        void WriteFloat(string name, float value) {
            
            _writer.WriteFloat(name, value);
        }

        /// <inherited/>
        ///
        void WriteDouble(string name, double value) {
            
            _writer.WriteDouble(name, value);
        }

        /// <inherited/>
        ///
        public void WriteObject(string name, object obj) {
            
            _writer.WriteObject(name, obj);
            
            if (obj == null)
                _writer.WriteObjectNull(name);
            
            else {                
                if (GetObjectId(obj, out int id))
                    _writer.WriteObjectReference(name, id);
            
                else {                
                    var type = obj.GetType();
                    _writer.WriteObjectHeader(name, type, id);                

                    var typeSerializer = GetTypeSerializer(type);
                    typeSerializer.Serialize(obj);

                    _writer.WriteObjectTail();
                }
            }
        }

        private bool GetObjectId(object obj, out int id) {

            id = _items.IndexOf(obj);
            if (id >= 0)
                return true;

            else {
                _items.Add(obj);
                id = _items.Count - 1;
                return false;
            }
        }        
        
        private TypeSerializer GetTypeSerializer(object obj) {
            
            var typeSerializerProvider = TypeSerializerProvider.Instance;  
            var type = obj.GetType();
            return typeSerializerProvider.GetTypeSerializer(type);
        }
    }
}
