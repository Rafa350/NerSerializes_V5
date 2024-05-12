using NetSerializer.V6.Formatters;
using NetSerializer.V6.TypeDescriptors;
using NetSerializer.V6.TypeSerializers;

namespace NetSerializer.V6 {

    public sealed class SerializationContext: ISerializationWriter {

        private readonly FormatWriter _writer;
        private readonly List<object> _items = [];

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="writer">El escriptor de dades.</param>
        /// 
        public SerializationContext(FormatWriter writer) {

            _writer = writer;
        }

        /// <inherited/>
        ///
        public void WriteBool(string name, bool value) =>
            _writer.WriteBool(name, value);

        /// <inherited/>
        ///
        public void WriteInt(string name, int value) =>
            _writer.WriteInt(name, value);

        /// <inherited/>
        ///
        public void WriteSingle(string name, float value) =>
            _writer.WriteSingle(name, value);

        /// <inherited/>
        ///
        public void WriteDouble(string name, double value) =>
            _writer.WriteDouble(name, value);

        /// <inherited/>
        ///
        public void WriteDecimal(string name, decimal value) =>
            _writer.WriteDecimal(name, value);

        /// <inherited/>
        ///
        public void WriteString(string name, string? value) =>
            _writer.WriteString(name, value);

        /// <inherited/>
        ///
        public void WriteEnum(string name, Enum value) =>
            _writer.WriteEnum(name, value);

        /// <inherited/>
        ///
        public void WriteNull(string name) =>
            _writer.WriteNull(name);

        /// <inherited/>
        ///
        public void WriteObject(string name, object? obj) {
            
            if (obj == null)
                _writer.WriteObjectNull(name);
            
            else {                
                if (GetObjectId(obj, out int id))
                    _writer.WriteObjectReference(name, id);
            
                else {                
                    _writer.WriteObjectHeader(name, obj.GetType(), id);
                    SerializeObject(obj);
                    _writer.WriteObjectTail();
                }
            }
        }

        /// <inherited/>
        ///
        public void WriteStruct<T>(string name, T value) where T : struct {

            _writer.WriteStructHeader(name);
            SerializeObject(value);
            _writer.WriteStructTail();
        }

        /// <summary>
        /// Serialitza un objecte.
        /// </summary>
        /// <param name="obj">L'objecte a serialitzar.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// 
        private void SerializeObject(object obj) {

            var type = obj.GetType();

            // Intenta amb els serialitzadors especifics.
            //
            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);
            if (typeDescriptor.CanSerialize)
                typeDescriptor.Serialize(this, obj);

            // Si no por, ho intenta amb el serialitzador generic.
            //
            else {
                var typeSerializer = TypeSerializerProvider.Instance.GetTypeSerializer(type);
                if (typeSerializer == null)
                    throw new InvalidOperationException($"No se encontro un serializador para el tipo '{type}'.");
                typeSerializer.Serialize(this, obj);
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
    }
}
