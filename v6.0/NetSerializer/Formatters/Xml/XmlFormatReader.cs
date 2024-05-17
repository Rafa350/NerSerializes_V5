using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using NetSerializer.V6.Formatters.Xml.Infrastructure;
using NetSerializer.V6.Formatters.Xml.ValueFormatters;

namespace NetSerializer.V6.Formatters.Xml {

    public sealed class XmlFormatReader: FormatReader {

        private static readonly CultureInfo _ci = CultureInfo.InvariantCulture;
        private const string _schemaResourceName = "NetSerializer.V6.Formatters.Xml.Schemas.DataSchema.xsd";

        private readonly XmlReader _reader;
        private bool _isClosed = false;

        private int _serializerVersion;
        private int _dataVersion;
        private bool _checkNames = true;
        private bool _useNames;
        private bool _useMeta;
        private bool _compactMode;
        private bool _encodedStrings;

        public XmlFormatReader(Stream stream) {

            if (!stream.CanRead)
                throw new InvalidOperationException("Es stream especificado no es de lectura.");

            /*if (_settings.Preprocesor != null) {
                var inputStream = new MemoryStream();
                _settings.Preprocesor.Process(stream, inputStream, false);
                inputStream.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
                stream = inputStream;
            }
            */

            var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_schemaResourceName);
            if (resourceStream == null)
                throw new Exception(string.Format("No se encontro el recurso '{0}'", _schemaResourceName));
            var schema = XmlSchema.Read(resourceStream, null);
            if (schema == null)
                throw new Exception(string.Format("No se pudo cargar el esquema '{0}'", _schemaResourceName));

            var readerSettings = new XmlReaderSettings {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true,
                CheckCharacters = true,
                CloseInput = true,
                ValidationType = ValidationType.Schema,
                MaxCharactersInDocument = 1000000,
                ConformanceLevel = ConformanceLevel.Document
            };
            readerSettings.Schemas.Add(schema);

            _reader = XmlReader.Create(stream, readerSettings);

            _reader.Read();

            // Processa la seccio <document>
            //
            _reader.Read();
            if (_reader.HasAttributes) {
                if (_reader.AttributeExist("version"))
                    _serializerVersion = _reader.GetAttributeAsInt("version");
                if (_reader.AttributeExist("useNames"))
                    _useNames = _reader.GetAttributeAsBool("useNames");
                if (_reader.AttributeExist("compactMode"))
                    _compactMode = _reader.GetAttributeAsBool("compactMode");
                if (_reader.AttributeExist("useMeta"))
                    _useMeta = _reader.GetAttributeAsBool("useMeta");
                if (_reader.AttributeExist("encodeStrings"))
                    _encodedStrings = _reader.GetAttributeAsBool("encodeStrings");
            }

            if (_checkNames && !_useNames)
                throw new InvalidOperationException("No es posible verificar los nombres de los elementos. La aplicacion requiere 'useNames=true'.");

            // Processa la seccio <data>
            //
            _reader.Read();
            if (_reader.HasAttributes) {
                if (_reader.AttributeExist("version"))
                    _dataVersion = _reader.GetAttributeAsInt("version");
            }

            // Es posiciona en el seguent element despres de <data>
            //
            _reader.Read();
        }

        /// <inheritdoc/>
        /// 
        public override void Close() {

            if (!_isClosed) {
                _reader.Close();
                _isClosed = true;
            }
        }

        /// <inheritdoc/>
        /// 
        public override void Dispose() {

            Close();
        }

        public override bool CanReadValue(Type type) {

            return ValueFormatterProvider.Instance.GetValueFormatter(type, false) != null;
        }

        public override object? ReadValue(string name, Type type) {

            object? result = null;

            if (!CheckNullNode(name)) {

                var valueFormatter = ValueFormatterProvider.Instance.GetValueFormatter(type, false);
                if (valueFormatter != null) {
                    if (!CheckValueNode(name))
                        throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");
                    result = valueFormatter.Read(_reader);
                }
                else
                    throw new InvalidOperationException($"No es posible leer el valor '{name}' del tipo '{type}'.");
            }

            return result;
        }

        /// <inheritdoc/>
        /// 
        public override bool ReadBool(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return bool.Parse(content);
        }

        /// <inheritdoc/>
        /// 
        public override int ReadInt(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return int.Parse(content);
        }

        /// <inheritdoc/>
        /// 
        public override float ReadSingle(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return float.Parse(content, _ci);
        }

        /// <inheritdoc/>
        /// 
        public override double ReadDouble(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return double.Parse(content, _ci);
        }

        /// <inheritdoc/>
        /// 
        public override decimal ReadDecimal(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return decimal.Parse(content, _ci);
        }

        /// <inheritdoc/>
        /// 
        public override T ReadEnum<T>(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            return Enum.Parse<T>(_reader.ReadElementContentAsString());
        }

        public override object ReadEnum(string name, Type type) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return Enum.Parse(type, content);
        }

        /// <inheritdoc/>
        /// 
        public override char ReadChar(string name) {

            if (!CheckValueNode(name))
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");

            var content = _reader.ReadElementContentAsString();
            return (char)int.Parse(content);
        }

        /// <inheritdoc/>
        /// 
        public override string? ReadString(string name) {

            if (CheckValueNode(name)) {
                var value = _reader.ReadElementContentAsString();

                if (_encodedStrings) {
                    var bytes = Convert.FromBase64String(value);
                    value = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                }

                return value;
            }

            else if (CheckNullNode(name)) {
                _reader.Read();
                return null;
            }

            else
                throw new InvalidOperationException($"Se esperaba un nodo '<value Name=\"{name}\">.");
        }

        /// <inheritdoc/>
        /// 
        public override ObjectHeaderType ReadObjectHeader(string name, out int id, out Type type) {

            ObjectHeaderType result;

            if (_reader.Name == (_compactMode ? "o" : "object")) {
                id = _reader.GetAttributeAsInt("id");
                type = Type.GetType(_reader.GetAttributeAsString("type"), true);
                result = ObjectHeaderType.Object;
            }
            else if (_reader.Name == (_compactMode ? "r" : "reference")) {
                id = _reader.GetAttributeAsInt("id");
                type = typeof(object);
                result = ObjectHeaderType.Reference;
            }
            else if (_reader.Name == (_compactMode ? "n" : "null")) {
                id = -1;
                type = typeof(object);
                result = ObjectHeaderType.Null;
            }
            else
                throw new InvalidOperationException($"Se esperaba un nodo '<object>', '<reference>' o '<null>'.");

            _reader.Read();

            return result;
        }

        /// <inheritdoc/>
        /// 
        public override void ReadObjectTail() {

            _reader.Read();
        }

        /// <inheritdoc/>
        /// 
        public override void ReadStructHeader(string name) {

            if (_reader.Name != (_compactMode ? "s" : "struct"))
                throw new InvalidOperationException();

            _reader.Read();
        }

        /// <inheritdoc/>
        /// 
        public override void ReadStructTail() {

            _reader.Read();
        }

        /// <inheritdoc/>
        /// 
        public override ArrayHeaderType ReadArrayHeader(string name, out int[] bound, out int count) {

            ArrayHeaderType result;

            if (_reader.Name == (_compactMode ? "a" : "array")) {

                var boundStr = _reader.GetAttributeAsString("bound").Split([',']);
                bound = new int[boundStr.Length];
                for (var i = 0; i < boundStr.Length; i++)
                    bound[i] = int.Parse(boundStr[i]);

                count = _reader.GetAttributeAsInt("count");
                result = ArrayHeaderType.Array;
            }

            else if (_reader.Name == (_compactMode ? "n" : "null")) {

                bound = [];
                count = 0;
                result = ArrayHeaderType.Null;
            }
            else
                throw new InvalidOperationException($"Se esperaba un nodo '<array>' o '<null>'.");

            _reader.Read();

            return result;
        }

        /// <inheritdoc/>
        /// 
        public override void ReadArrayTail() {

            _reader.Read();
        }

        /// <summary>
        /// Comprova si un node <value> es correcte.</value>
        /// </summary>
        /// <param name="name">El nom del node</param>
        /// <returns>True si tot es correcte.</returns>
        /// 
        private bool CheckValueNode(string name) {

            if (_reader.Name != (_compactMode ? "v" : "value"))
                return false;

            if (_useNames && _checkNames)
                if (name != _reader.GetAttributeAsString("name"))
                    return false;

            return true;
        }

        /// <summary>
        /// Comprova si un node <null> es correcte.</value>
        /// </summary>
        /// <param name="name">El nom del node</param>
        /// <returns>True si tot es correcte.</returns>
        /// 
        private bool CheckNullNode(string name) {

            if (_reader.Name != (_compactMode ? "n" : "null"))
                return false;

            if (_useNames && _checkNames)
                if (name != _reader.GetAttributeAsString("name"))
                    return false;

            return true;
        }

        /// <inheritdoc/>
        /// 
        public override int Version =>
            _dataVersion;
    }
}
