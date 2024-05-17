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
        public object? Read(string name, Type type) {

            if (_reader.CanReadValue(type))
                return _reader.ReadValue(name, type);

            else if (type.IsPrimitive || type.IsSpecialType()) {
                switch (Type.GetTypeCode(type)) {
                    case TypeCode.Boolean:
                        return _reader.ReadBool(name);

                    case TypeCode.Int32:
                        return _reader.ReadInt(name);

                    case TypeCode.Single:
                        return _reader.ReadSingle(name);

                    case TypeCode.Double:
                        return _reader.ReadDouble(name);

                    case TypeCode.Decimal:
                        return _reader.ReadDecimal(name);

                    case TypeCode.String:
                        return _reader.ReadString(name);

                    case TypeCode.Char:
                        return _reader.ReadChar(name);

                    default:
                        throw new InvalidOperationException($"No es posible deserializar el valor '{name}'.");
                }
            }
           
            else if (type.IsEnum)
                return _reader.ReadEnum(name, type);

            else if (type.IsStructType())
                return ReadStruct(name, type);

            else if (type.IsClassType())
                return ReadObject(name, type);

            else if (type.IsArray)
                return ReadArray(name, type);

            else
                throw new InvalidOperationException($"No es posible deserializar el valor '{name}'.");
        }
           
        /// <inherited/>
        ///
        public bool ReadValueBool(string name) =>
            _reader.ReadBool(name);

        /// <inherited/>
        ///
        public int ReadValueInt(string name) =>
            _reader.ReadInt(name);

        /// <inherited/>
        ///
        public float ReadValueSingle(string name) =>
            _reader.ReadSingle(name);

        /// <inherited/>
        ///
        public double ReadValueDouble(string name) =>
            _reader.ReadDouble(name);

        /// <inherited/>
        ///
        public decimal ReadValueDecimal(string name) =>
            _reader.ReadDecimal(name);

        /// <inherited/>
        ///
        public T ReadValueEnum<T>(string name) where T : struct =>
            _reader.ReadEnum<T>(name);

        /// <inherited/>
        ///
        public object ReadValueEnum(string name, Type type) =>
            _reader.ReadEnum(name, type);

        /// <inherited/>
        ///
        public char ReadValueChar(string name) =>
            _reader.ReadChar(name);

        /// <inherited/>
        ///
        public string? ReadValueString(string name) =>
            _reader.ReadString(name);

        /// <inherited/>
        ///
        public T? ReadObject<T>(string name) {

            return (T?)ReadObject(name, typeof(T));
        }

        /// <inherited/>
        ///
        public object? ReadObject(string name, Type type) {

            object? obj = default;

            switch (_reader.ReadObjectHeader(name, out int id, out Type serializedType)) {
                case ObjectHeaderType.Reference:
                    obj = GetObject(id);
                    break;

                case ObjectHeaderType.Object:
                    if (!type.IsAssignableFrom(serializedType))
                        throw new InvalidOperationException($"El type '{serializedType}' no se puede asignar a una propiedad de tipo tipo '{type}'.");
                    obj = CreateObject(serializedType);
                    DeserializeObject(obj);
                    _reader.ReadObjectTail();
                    break;
            }

            return obj;
        }

        /// <inherited/>
        ///
        public object ReadStruct(string name, Type type) {

            object obj;

            _reader.ReadStructHeader(name);
            obj = CreateObject(type);
            DeserializeObject(obj);
            _reader.ReadStructTail();

            return obj;
        }

        public Array? ReadArray(string name, Type type) {

            Array? array = default(Array);

            if (_reader.ReadArrayHeader(name, out int[] bound, out int count) == ArrayHeaderType.Array) {

                var elementType = type.GetElementType();
                if (elementType == null)
                    throw new InvalidOperationException("No es posible obtener el tipo de elemento del array.");
                array = Array.CreateInstance(elementType, bound);

                var typeSerializer = TypeSerializerProvider.Instance.GetTypeSerializer(type);
                if (typeSerializer == null)
                    throw new InvalidOperationException($"No se ncontro un deserializador para el tipo '{type}'.");
                typeSerializer.Deserialize(this, array);

                _reader.ReadArrayTail();
            }

            return array;
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

        /// <inherited/>
        ///
        public int Version =>
            _reader.Version;
    }
}
