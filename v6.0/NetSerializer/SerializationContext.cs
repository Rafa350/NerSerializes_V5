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
        public void Write(string name, object? value) {

            if (value == null)
                _writer.WriteNull(name);

            else {
                var type = value.GetType();

                if (_writer.CanWriteValue(type))
                    _writer.WriteValue(name, value);

                else if (type.IsPrimitive || type.IsSpecialType()) {
                    switch (Type.GetTypeCode(type)) {
                        case TypeCode.Boolean:
                            _writer.WriteBool(name, (bool)value);
                            break;

                        case TypeCode.Int32:
                            _writer.WriteInt(name, (int)value);
                            break;

                        case TypeCode.Single:
                            WriteValueSingle(name, (float)value);
                            break;

                        case TypeCode.Double:
                            _writer.WriteDouble(name, (double)value);
                            break;

                        case TypeCode.Decimal:
                            _writer.WriteDecimal(name, (decimal)value);
                            break;

                        case TypeCode.Char:
                            _writer.WriteChar(name, (char)value);
                            break;

                        case TypeCode.String:
                            _writer.WriteString(name, (string)value);
                            break;

                        default:
                            throw new InvalidOperationException("No es posible serializar el valor '{name}'.");
                    }
                }

                else if (type.IsEnum)
                    _writer.WriteEnum(name, (Enum)value);

                else if (type.IsStructType())
                    WriteStruct(name, value);

                else if (type.IsClassType())
                    WriteObject(name, value);

                else if (type.IsArray)
                    WriteArray(name, (Array)value);

                else
                    throw new InvalidOperationException("No es posible serializar el valor '{name}'.");
            }
        }
    

        /// <inherited/>
        ///
        public void WriteValueBool(string name, bool value) {

            _writer.WriteBool(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueShort(string name, short value) {

            throw new NotFiniteNumberException();
        }

        /// <inherited/>
        ///
        public void WriteValueInt(string name, int value) {

            _writer.WriteInt(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueLong(string name, long value) {

            throw new NotFiniteNumberException();
        }

        /// <inherited/>
        ///
        public void WriteValueSingle(string name, float value) {

            _writer.WriteSingle(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueDouble(string name, double value) {

            _writer.WriteDouble(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueDecimal(string name, decimal value) {

            _writer.WriteDecimal(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueChar(string name, char value) {

            _writer.WriteChar(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueString(string name, string? value) {

            _writer.WriteString(name, value);
        }

        /// <inherited/>
        ///
        public void WriteValueDateTime(string name, DateTime value) {

            throw new NotFiniteNumberException();
        }

        /// <inherited/>
        ///
        public void WriteValueTimeSpan(string name, TimeSpan value) {

            throw new NotFiniteNumberException();
        }

        /// <inherited/>
        ///
        public void WriteValueGuid(string name, Guid value) {

            throw new NotFiniteNumberException();
        }

        /// <inherited/>
        ///
        public void WriteValueEnum(string name, Enum value) {

            _writer.WriteEnum(name, value);
        }

        /// <inherited/>
        ///
        public void WriteNull(string name) =>
            _writer.WriteNull(name);

        /// <inherited/>
        ///
        public void WriteObject(string name, object? obj) {

            if (obj == null)
                _writer.WriteNull(name);

            else {
                var type = obj.GetType();
                if (!type.IsClassType())
                    throw new InvalidOperationException($"Se esperaba un tipo 'class', no '{type}'.");

                if (GetObjectId(obj, out int id))
                    _writer.WriteObjectReference(name, id);

                else {
                    _writer.WriteObjectHeader(name, type, id);
                    SerializeObject(obj);
                    _writer.WriteObjectTail();
                }
            }
        }

        /// <inherited/>
        ///
        public void WriteStruct(string name, object value) {

            // Comprova que sigui el tipus correcte.
            //
            var type = value.GetType();
            if (!type.IsStructType())
                throw new InvalidOperationException($"Se esperaba un tipo 'struct', no '{type}'.");

            _writer.WriteStructHeader(name);
            SerializeObject(value);
            _writer.WriteStructTail();
        }

        /// <inherited/>
        ///
        public void WriteArray(string name, Array value) {

            int[] bound = new int[value.Rank];
            for (int i = 0; i < bound.Length; i++)
                bound[i] = value.GetUpperBound(i) + 1;

            _writer.WriteArrayHeader(name, bound, value.Length);
            SerializeArray(value);
            _writer.WriteArrayTail();
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

            // Si no pot, ho intenta amb el serialitzador generic.
            //
            else {
                var typeSerializer = TypeSerializerProvider.Instance.GetTypeSerializer(type);
                if (typeSerializer == null)
                    throw new InvalidOperationException($"No se encontro un serializador para el objeto de tipo '{type}'.");
                typeSerializer.Serialize(this, obj);
            }
        }

        /// <summary>
        /// Serialitza un array.
        /// </summary>
        /// <param name="array">El array.</param>
        /// 
        private void SerializeArray(Array array) {

            var type = array.GetType();
            var typeSerializer = TypeSerializerProvider.Instance.GetTypeSerializer(type);
            if (typeSerializer == null)
                throw new InvalidOperationException($"No se encontro un serializador para el array de tipo '{type}'.");
            typeSerializer.Serialize(this, array);
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
