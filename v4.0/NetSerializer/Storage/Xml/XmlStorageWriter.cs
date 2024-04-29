namespace MikroPic.NetSerializer.v4.Storage.Xml {

    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text;
    using System.Xml;
    using MikroPic.NetSerializer.v4.Storage.Xml.Infrastructure;

    /// <summary>
    /// Escriptor de dades en format XML.
    /// </summary>
    public sealed class XmlStorageWriter: StorageWriter {

        private const int serializerVersion = 400;
        private readonly XmlStorageWriterSettings settings;
        private readonly Stream stream;
        private XmlWriter writer;
        private readonly ITypeNameConverter typeNameConverter = new TypeNameConverter();

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="stream">El stream d'escriptura.</param>
        /// <param name="settings">Parametres de configuracio.</param>
        public XmlStorageWriter(Stream stream, XmlStorageWriterSettings settings) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (settings == null)
                settings = new XmlStorageWriterSettings();

            this.stream = stream;
            this.settings = settings;
        }

        /// <summary>
        /// Inicialitza l'operacio d'escriptura.
        /// </summary>
        /// <param name="version">Numero de versio de les dades.</param>
        public override void Initialize(int version) {

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Encoding = settings.Encoding;
            writerSettings.Indent = settings.Indentation > 0;
            writerSettings.IndentChars = new String(' ', settings.Indentation);
            writerSettings.CheckCharacters = true;
            writerSettings.CloseOutput = true;
            writer = XmlWriter.Create(stream, writerSettings);

            writer.WriteStartDocument();
            writer.WriteStartElement("document");
            writer.WriteAttribute("version", serializerVersion);
            writer.WriteAttribute("culture", settings.Culture.Name);
            writer.WriteAttribute("encodeStrings", settings.EncodedStrings);
            writer.WriteAttribute("useNames", settings.UseNames);
            writer.WriteAttribute("useTypes", settings.UseTypes);
            writer.WriteStartElement("data");
            writer.WriteAttribute("version", version);
        }

        /// <summary>
        /// Finalitza l'operacio d'escriptura.
        /// </summary>
        public override void Close() {

            writer.WriteEndElement();   
            writer.WriteEndElement();   
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Escriu un valor.
        /// </summary>
        /// <param name="name">Nom del node.</param>
        /// <param name="value">El valor.</param>
        public override void WriteValue(string name, object value) {

            if (value == null)
                throw new ArgumentNullException("value");

            Type type = value.GetType();

            writer.WriteStartElement("value");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);

            if (settings.UseTypes)
                writer.WriteAttribute("type", TypeToString(type));

            writer.WriteValue(ValueToString(value));

            writer.WriteEndElement();
        }

        /// <summary>
        /// Escriu un valor null.
        /// </summary>
        /// <param name="name">Nom del node.</param>
        public override void WriteNull(string name) {

            writer.WriteStartElement("null");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);
            writer.WriteEndElement();
        }

        public override void WriteObjectReference(string name, int id) {

            writer.WriteStartElement("reference");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);
            writer.WriteAttribute("id", id);
            writer.WriteEndElement();
        }

        public override void WriteObjectStart(string name, Type type, int id) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsClass)
                throw new InvalidOperationException(
                    String.Format("No se puede escribir el tipo '{0}'.", type));

            writer.WriteStartElement("object");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);
            writer.WriteAttribute("type", TypeToString(type));
            writer.WriteAttribute("id", id);
        }

        public override void WriteObjectEnd() {

            writer.WriteEndElement();
        }

        public override void WriteStructStart(string name, Type type, object value) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsValueType || type.IsPrimitive || type.IsEnum)
                throw new InvalidOperationException(
                    String.Format("No se puede escribir el tipo '{0}'.", type));

            writer.WriteStartElement("struct");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);
        }

        public override void WriteStructEnd() {

            writer.WriteEndElement();
        }

        public override void WriteArrayStart(string name, Type type, int[] bound, int count) {

            writer.WriteStartElement("array");
            if (settings.UseNames)
                writer.WriteAttribute("name", name);

            StringBuilder sb = new StringBuilder();
            bool first = true;
            foreach (int x in bound) {
                if (first)
                    first = false;
                else
                    sb.Append(", ");
                sb.Append(x);
            }

            writer.WriteAttribute("bound", sb.ToString());
            writer.WriteAttribute("count", count);
        }

        public override void WriteArrayEnd() {

            writer.WriteEndElement();
        }

        /// <summary>
        /// Converteix un tipus a string.
        /// </summary>
        /// <param name="type">El tipus a convertir.</param>
        /// <returns>El resultat de la converssio.</returns>
        private string TypeToString(Type type) {

            return typeNameConverter.ToString(type);
        }

        /// <summary>
        /// Converteix un valor a string.
        /// </summary>
        /// <param name="value">El valor a convertir.</param>
        /// <param name="culture">Informacio cultural.</param>
        /// <returns>El resultat de la converssio.</returns>
        private string ValueToString(object value) {

            if (value == null)
                return null;
            
            else {
                Type type = value.GetType();

                // Es tipus 'char'
                //
                if (type == typeof(char))
                    return Convert.ToUInt16(value).ToString();

                // Es tipus 'string'
                //
                else if (type == typeof(string)) {
                    string s = (string) value;
                    if (s.Length == 0)
                        return null;
                    else {
                        if (settings.EncodedStrings) {
                            byte[] bytes = Encoding.UTF8.GetBytes(s);
                            return Convert.ToBase64String(bytes);
                        }
                        else
                            return s;
                    }
                }

                // Altres tipus
                //
                else {
                    TypeConverter converter = TypeDescriptor.GetConverter(type);
                    if ((converter != null) && converter.CanConvertTo(typeof(string)))
                        return converter.ConvertToString(null, settings.Culture, value);
                    else
                        return value.ToString();
                }
            }            
        }
    }
}
