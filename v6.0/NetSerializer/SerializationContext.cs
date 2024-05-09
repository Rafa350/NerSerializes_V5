using NetSerializer.V6.Formaters;
using NetSerializer.V6.TypeSerializers;

namespace NetSerializer.V6 {

    public sealed class SerializationContext: ISerializationWriter {

        private readonly FormatWriter _writer;
        private readonly List<object> _items = [];

        public SerializationContext(FormatWriter writer) {

            _writer = writer;
        }

        /// <inherited/>
        ///
        public void WriteBool(string name, bool value) {
         
            _writer.WriteBool(name, value);
        }

        /// <inherited/>
        ///
        public void WriteInt(string name, int value) {
            
            _writer.WriteInt(name, value);
        }

        /// <inherited/>
        ///
        public void WriteSingle(string name, float value) {
            
            _writer.WriteSingle(name, value);
        }

        /// <inherited/>
        ///
        public void WriteDouble(string name, double value) {
            
            _writer.WriteDouble(name, value);
        }

        public void WriteString(string name, string? value) {

            _writer.WriteString(name, value);
        }

        /// <inherited/>
        ///
        public void WriteNull(string name) {

            _writer.WriteNull(name);
        }

        /// <inherited/>
        ///
        public void WriteObject(string name, object? obj) {
            
            if (obj == null)
                _writer.WriteObjectNull(name);
            
            else {                
                if (GetObjectId(obj, out int id))
                    _writer.WriteObjectReference(name, id);
            
                else {                
                    var type = obj.GetType();
                    _writer.WriteObjectHeader(name, type, id);                

                    var typeSerializer = GetTypeSerializer(type);
                    typeSerializer?.Serialize(this, obj);

                    _writer.WriteObjectTail();
                }
            }
        }

        /// <summary>
        /// Obte el identificador del objecte.
        /// </summary>
        /// <param name="obj">El objecte.</param>
        /// <param name="id">El identificador obtingut.</param>
        /// <returns>True si el identificador es reutilitzat, false en cas contrari.</returns>
        /// 
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
        
        /// <summary>
        /// Obte el serialitzador pel objecte especificat.
        /// </summary>
        /// <param name="obj">L'objecte.</param>
        /// <returns>El seu serialitzador.</returns>
        /// 
        private static ITypeSerializer? GetTypeSerializer(object obj) {
            
            var typeSerializerProvider = TypeSerializerProvider.Instance;  
            var type = obj.GetType();
            return typeSerializerProvider.GetTypeSerializer(type);
        }
    }
}
