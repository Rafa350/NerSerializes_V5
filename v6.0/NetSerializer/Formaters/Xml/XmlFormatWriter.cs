using System;
using System.Globalization;
using System.IO;
using System.Xml;
using NetSerializer.V6.Formaters;


namespace NetSerializer.V6.Formaters.Xml {
    
    public sealed class XmlFormatWriter: FormatWriter {
        
        private const int _serializerVersion = 500;

        private readonly Stream _stream;
        private XmlWriter _writer;
        private bool _closed = false;
        
        private Encoding _encoding = Encoding.UTF8;
        private int _indentation = 4;
        private bool _useNames = true;
        private bool _useMeta = false;
        private bool _compactMode = false;
        private bool _encodedStrings = true;

        
        public XmlFormatWriter(Stream stream, XmlFormatWriterSettings settings = null {
            
            if (!stream.CanWrite)
                throw new InvalidOperationException("El stream especificado no es de escritura.");

            _stream = stream;
        }
                
        /// <summary>
        /// Inicialitza l'operacio d'escriptura.
        /// </summary>
        /// <param name="version">Numero de versio de les dades.</param>
        /// 
        public override void Initialize(int version) {

            ArgumentOutOfRangeException.ThrowIfNegative(version, nameof(version));

            var writerSettings = new XmlWriterSettings {
                Encoding = _encoding,
                Indent = _indentation > 0,
                IndentChars = new String(' ', _indentation),
                CheckCharacters = true,
                CloseOutput = false
            };
            _writer = XmlWriter.Create(_stream, writerSettings);

            _writer.WriteStartDocument();
            _writer.WriteStartElement("document");
            _writer.WriteAttribute("version", _serializerVersion);
            _writer.WriteAttribute("encodeStrings", _encodedStrings);
            _writer.WriteAttribute("useNames", _useNames);
            _writer.WriteAttribute("compactMode", _compactMode);
            _writer.WriteAttribute("useMeta", _useMeta);
            _writer.WriteStartElement("data");
            _writer.WriteAttribute("version", version);
        }

        /// <summary>
        /// Finalitza l'operacio d'escriptura.
        /// </summary>
        /// 
        public override void Close() {

            if (!_closed) {
                _writer.WriteEndElement();
                _writer.WriteEndElement();
                _writer.WriteEndDocument();
                _writer.Close();
                _closed = true;
            }
        }
        
        public override void WriteInt(string name, int value) {
            
            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "v" : "value");
            if (_settings.UseNames)
                _writer.WriteAttribute("name", name);

            _writer.WriteValue(value.ToString());

            _writer.WriteEndElement();

        }
        
        public override void WriteFloat(string name, float value) {
            
            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "v" : "value");
            if (_settings.UseNames)
                _writer.WriteAttribute("name", name);

            _writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));

            _writer.WriteEndElement();
        }
     
        public override void WriteDouble(string name, double value) {
            
            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "v" : "value");
            if (_settings.UseNames)
                _writer.WriteAttribute("name", name);

            _writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));

            _writer.WriteEndElement();
        }
       
        public override void WriteObjectHeader(string name, Type type, int id) {
            
            if (_useNames && String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _writer.WriteStartElement(_compactMode ? "o" : "object");
            if (_settings.UseNames)
                _writer.WriteAttribute("name", name);

            _writer.WriteAttribute("type", Type.ToString());

        }
        
        public override void WriteObjectTail(string name) {
            
            _writer.WriteEndElement();
        }
    }
    
}