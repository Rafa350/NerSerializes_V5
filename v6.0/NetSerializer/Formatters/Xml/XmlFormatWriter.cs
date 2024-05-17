using System.Globalization;
using System.Text;
using System.Xml;
using NetSerializer.V6.Formatters.Xml.Infrastructure;
using NetSerializer.V6.Formatters.Xml.ValueFormatters;

namespace NetSerializer.V6.Formatters.Xml {

    public sealed class XmlFormatWriter: FormatWriter {

        private const int _serializerVersion = 500;

        private static readonly CultureInfo _ci = CultureInfo.InvariantCulture;

        private XmlWriter _writer;
        private bool _isClosed = false;

        private Encoding _encoding = Encoding.UTF8;
        private int _indentation = 4;
        private bool _useNames = true;
        private bool _useMeta = false;
        private bool _compactMode = false;
        private bool _encodedStrings = true;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="stream">El stream de escriptura.</param>
        /// <param name="version">La versio del contingut.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// 
        public XmlFormatWriter(Stream stream, int version) {

            ArgumentOutOfRangeException.ThrowIfNegative(version, nameof(version));

            if (!stream.CanWrite)
                throw new InvalidOperationException("El stream especificado no es de escritura.");

            var writerSettings = new XmlWriterSettings {
                Encoding = _encoding,
                Indent = _indentation > 0,
                IndentChars = new String(' ', _indentation),
                CheckCharacters = true,
                CloseOutput = false
            };
            _writer = XmlWriter.Create(stream, writerSettings);
            _writer.WriteStartDocument();
            _writer.WriteStartElement("document");
            _writer.WriteAttributeInt("version", _serializerVersion);
            _writer.WriteAttributeBool("encodeStrings", _encodedStrings);
            _writer.WriteAttributeBool("useNames", _useNames);
            _writer.WriteAttributeBool("compactMode", _compactMode);
            _writer.WriteAttributeBool("useMeta", _useMeta);
            _writer.WriteStartElement("data");
            _writer.WriteAttributeInt("version", version);
        }

        /// <inheritdoc/>
        /// 
        public override void Dispose() {

            Close();
        }

        /// <inheritdoc/>
        /// 
        public override void Close() {

            if (!_isClosed) {
                _isClosed = true;

                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndDocument();
                _writer.Close();
            }
        }

        /// <summary>
        /// Escriu la capcelera els valors.
        /// </summary>
        /// <param name="name">El no,.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        private void WriteValueHeader(string name) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "v" : "value");
            if (_useNames)
                _writer.WriteAttributeString("name", name);
        }

        /// <summary>
        /// Escriu el final del valor.
        /// </summary>
        /// 
        private void WriteValueTail() {

            _writer.WriteEndElement();
        }

        /// <inheritdoc/>
        /// 
        public override bool CanWriteValue(Type type) {

            return ValueFormatterProvider.Instance.GetValueFormatter(type, false) != null;
        }

        /// <inheritdoc/>
        /// 
        public override void WriteValue(string name, object? value) {

            if (value == null)
                WriteNull(name);

            else {
                var valueFormatter = ValueFormatterProvider.Instance.GetValueFormatter(value.GetType(), false);
                if (valueFormatter != null) {
                    WriteValueHeader(name);
                    valueFormatter.Write(_writer, value);
                    WriteValueTail();
                }
                else
                    throw new InvalidOperationException($"No es posible escribir el valor '{name}' del tipo '{value.GetType()}'.");
            }
        }

        /// <inheritdoc/>
        /// 
        public override void WriteBool(string name, bool value) {

            WriteValueHeader(name);
            _writer.WriteValue(value ? "True" : "False");
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteInt(string name, int value) {

            WriteValueHeader(name);
            _writer.WriteValue(value.ToString());
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteSingle(string name, float value) {

            WriteValueHeader(name);
            _writer.WriteValue(value.ToString(_ci));
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteDouble(string name, double value) {

            WriteValueHeader(name);
            _writer.WriteValue(value.ToString(_ci));
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteDecimal(string name, decimal value) {

            WriteValueHeader(name);
            _writer.WriteValue(value.ToString(_ci));
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteChar(string name, char value) {

            WriteValueHeader(name);
            _writer.WriteValue(((int)value).ToString());
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteString(string name, string? value) {

            if (_encodedStrings) {
                var bytes = Encoding.UTF8.GetBytes(value);
                value = Convert.ToBase64String(bytes);
            }

            WriteValueHeader(name);
            _writer.WriteValue(value);
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteEnum(string name, Enum value) {

            WriteValueHeader(name);
            _writer.WriteValue(value.ToString());
            WriteValueTail();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteNull(string name) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "n" : "null");
            if (_useNames)
                _writer.WriteAttributeString("name", name);
            _writer.WriteEndElement();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteObjectReference(string name, int id) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "r" : "reference");
            if (_useNames)
                _writer.WriteAttributeString("name", name);
            _writer.WriteAttributeInt("id", id);

            _writer.WriteEndElement();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteObjectHeader(string name, Type type, int id) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "o" : "object");
            if (_useNames)
                _writer.WriteAttributeString("name", name);

            var typeName = $"{type}, {type.Assembly.GetName().Name}";
            _writer.WriteAttributeString("type", typeName);
            _writer.WriteAttributeInt("id", id);
        }

        /// <inheritdoc/>
        /// 
        public override void WriteObjectTail() {

            _writer.WriteEndElement();
        }

        public override void WriteStructHeader(string name) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "s" : "struct");
            if (_useNames)
                _writer.WriteAttributeString("name", name);
        }

        /// <inheritdoc/>
        /// 
        public override void WriteStructTail() {

            _writer.WriteEndElement();
        }

        /// <inheritdoc/>
        /// 
        public override void WriteArrayHeader(string name, int[] bound, int count) {

            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "a" : "array");
            if (_useNames)
                _writer.WriteAttributeString("name", name);
            _writer.WriteAttributeInt("bound", bound);
            _writer.WriteAttributeInt("count", count);
        }

        /// <inheritdoc/>
        /// 
        public override void WriteArrayTail() {

            _writer.WriteEndElement();
        }
    }

}