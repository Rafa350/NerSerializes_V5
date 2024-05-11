using NetSerializer.V6.Formatters;
using NetSerializer.V6.TypeDescriptors;
using NetSerializer.V6.TypeSerializers;

namespace NetSerializer.V6 {

    public sealed class DeserializationContext: IDeserializationReader {
        
        private readonly FormatReader _reader;
        private readonly List<object> _items = [];
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">El lector de dades.</param>
        /// 
        public DeserializationContext(FormatReader reader) {
            
            _reader = reader;
        }
        
        /// <inherited/>
        ///
        public bool ReadBool(string name) =>
            _reader.ReadBool(name);
        
        /// <inherited/>
        ///
        public int ReadInt(string name) =>
            _reader.ReadInt(name);
        
        /// <inherited/>
        ///
        public float ReadSingle(string name) =>
            _reader.ReadSingle(name);
        
        /// <inherited/>
        ///
        public double ReadDouble(string name) =>
            _reader.ReadDouble(name);

        /// <inherited/>
        ///
        public decimal ReadDecimal(string name) =>
            _reader.ReadDecimal(name);

        /// <inherited/>
        ///
        public T ReadEnum<T>(string name) where T : struct =>
            _reader.ReadEnum<T>(name);

        /// <inherited/>
        ///
        public object ReadEnum(string name, Type type) =>
            _reader.ReadEnum(name, type);

        /// <inherited/>
        ///
        public string? ReadString(string name) =>
            _reader.ReadString(name);

        /// <inherited/>
        ///
        public object? ReadObject(string name) {

            object? obj = null;

            switch (_reader.ReadObjectHeader(name, out int id, out Type type)) {
                case ObjectHeaderType.Reference:
                    obj = GetObject(id);
                    break;

                case ObjectHeaderType.Object:
                    obj = CreateObject(type);
                    DeserializeObject(obj);
                    _reader.ReadObjectTail();
                    break;
            }

            return obj;
        }

        /// <summary>
        /// Crea un objecte del tipus especificat.
        /// </summary>
        /// <param name="type">El tipus.</param>
        /// <returns>L'objecte creat.</returns>
        /// 
        private object CreateObject(Type type) {

            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);
            var obj = typeDescriptor.CanCreate ? typeDescriptor.Create(this) : Activator.CreateInstance(type);
            if (obj == null)
                throw new InvalidCastException($"No se pudo crear una instancia del objeto de tipo '{type}'.");
            _items.Add(obj);

            return obj;
        }

        /// <summary>
        /// Deserialitza un objecte.
        /// </summary>
        /// <param name="obj">L'objecte a deserialitzar.</param>
        /// 
        private void DeserializeObject(object obj) {

            var type = obj.GetType();

            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);
            if (typeDescriptor.CanDeserialize)
                typeDescriptor.Deserialize(this, obj);
            
            else {
                var typeSerializer = TypeSerializerProvider.Instance.GetTypeSerializer(type);
                if (typeSerializer == null)
                    throw new InvalidOperationException($"No se ncontro un deserializador para el tipo '{type}'.");
                typeSerializer.Deserialize(this, obj);
            }
        }

        /// <summary>
        /// Obte l'objecte associat al identificador.
        /// </summary>
        /// <param name="id">El identificador.</param>
        /// <returns>L'objecte.</returns>
        /// 
        private object GetObject(int id) { 
            
            return _items[id];
        }
    }
}
