namespace MikroPic.NetSerializer.v4.Storage.Xml {

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.Xml.Schema;
    using MikroPic.NetSerializer.v4.Storage.Xml.Infrastructure;

    /// <summary>
    /// Lector de dades en format XML.
    /// </summary>
    public sealed class XmlStorageReader: StorageReader {

        private const string schemaResourceName = "Media.NetSerializer.v4.Storage.Xml.DataSchema.xsd";

        private readonly ITypeNameConverter typeNameConverter = new TypeNameConverter();
        private readonly Stream stream;
        private readonly XmlStorageReaderSettings settings;
        private XmlReader reader;
        private int serializerVersion = 0;
        private int dataVersion = 0;
        private bool useNames = false;
        private bool useTypes = false;
        private bool encodedStrings = false;

        /// <summary>
        /// Contructor del objecte.
        /// </summary>
        /// <param name="stream">Stream d'entrada. Si es null es dispara una excepcio.</param>
        /// <param name="settings">Parametres de configuracio. Si es null, s'utilitza la configuracio per defecte.</param>
        public XmlStorageReader(Stream stream, XmlStorageReaderSettings settings) {

            if (stream == null)
                throw new ArgumentNullException("stream");

            if (settings == null)
                settings = new XmlStorageReaderSettings();

            this.stream = stream;
            this.settings = settings;
        }

        /// <summary>
        /// Inicia la lectura de dades.
        /// </summary>
        public override void Initialize() {

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            
            readerSettings.IgnoreComments = true;
            readerSettings.IgnoreWhitespace = true;
            readerSettings.IgnoreProcessingInstructions = true;
            readerSettings.CheckCharacters = true;
            readerSettings.CloseInput = true;

            Stream inputStream;
            if (settings.Preprocesor != null) {
                inputStream = new MemoryStream();
                settings.Preprocesor.Process(stream, inputStream, false);
                inputStream.Flush();
                inputStream.Seek(0, SeekOrigin.Begin);
            }
            else
                inputStream = stream;

            if (settings.UseSchemaValidation) {
                Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(schemaResourceName);
                if (resourceStream == null)
                    throw new Exception(String.Format("No se encontro el recurso '{0}'", schemaResourceName));
                XmlSchema schema = XmlSchema.Read(resourceStream, null);
                readerSettings.ValidationType = ValidationType.Schema;
                readerSettings.Schemas.Add(schema);
            }
            else
                readerSettings.ValidationType = ValidationType.None;
            
            readerSettings.MaxCharactersInDocument = 1000000;
            readerSettings.ConformanceLevel = ConformanceLevel.Document;

            reader = XmlReader.Create(inputStream, readerSettings);

            reader.Read();
            
            // Processa la seccio <document>
            //
            reader.Read();
            if (reader.HasAttributes) {
                IDictionary<string, string> attributes = reader.GetAttributes();
                string value;
                if (attributes.TryGetValue("version", out value))
                    serializerVersion = Convert.ToInt32(value);
                if (attributes.TryGetValue("useNames", out value))
                    useNames =  Convert.ToBoolean(value);
                if (attributes.TryGetValue("useTypes", out value))
                    useTypes = Convert.ToBoolean(value);
                if (attributes.TryGetValue("encodeStrings", out value))
                    encodedStrings = Convert.ToBoolean(value);
            }

            if (settings.CheckNames && !useNames)
                throw new InvalidOperationException("No es posible verificar los nombres de los elementos. La aplicacion requiere 'useNames=true'.");

            // Processa la seccio <data>
            //
            reader.Read();
            if (reader.HasAttributes) {
                IDictionary<string, string> attributes = reader.GetAttributes();
                string value;
                if (attributes.TryGetValue("version", out value))
                    dataVersion = Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Finalitza la lectura de dades.
        /// </summary>
        public override void Close() {

            reader.Close();
        }

        /// <summary>
        /// Llegeix un valor, que pot ser null.
        /// </summary>
        /// <param name="name">Nom del node.</param>
        /// <param name="type">Tipus del valor a lleigir.</param>
        /// <returns>El valor lleigit.</returns>
        public override object ReadValue(string name, Type type) {

            reader.Read();

            if (reader.Name == "null")
                return null;

            else {

                IDictionary<string, string> attributes = reader.GetAttributes();

                if (useNames && settings.CheckNames) {
                    if (name != GetAttribute(attributes, "name"))
                        throw new InvalidOperationException(String.Format("Se esperaba un valor de nombre '{0}'.", name));
                }

                if (useTypes && settings.CheckTypes) {
                    string typeName = GetAttribute(attributes, "type");
                    if (type != (typeName == null ? null : TypeFromString(typeName)))
                        throw new InvalidOperationException(String.Format("Se esperaba un valor de tipo '{0}'.", type));
                }

                string content = GetContent();
                return ValueFromString(type, content);
            }
        }

        /// <summary>
        /// Llegeix l'inici d'un objecte/clase.
        /// </summary>
        /// <param name="name">Nom del node.</param>
        /// <param name="type">Tipus del objecte a lleigir.</param>
        /// <param name="id">Retorna l'identificador del objecte.</param>
        public override void ReadObjectStart(string name, out Type type, out int id) {

            reader.Read();

            if (reader.Name == "null") {

                type = null;
                id = -1;
            }

            else if (reader.Name == "object") {

                IDictionary<string, string> attributes = reader.GetAttributes();

                if (useNames && settings.CheckNames) {
                    if (name != GetAttribute(attributes, "name"))
                        throw new InvalidOperationException(String.Format("Se esperaba un objeto de nombre '{0}'.", name));
                }

                type = TypeFromString(GetAttribute(attributes, "type", true));
                id = Convert.ToInt32(GetAttribute(attributes, "id", true));
            }

            else if (reader.Name == "reference") {

                IDictionary<string, string> attributes = reader.GetAttributes();

                if (useNames && settings.CheckNames) {
                    if (name != GetAttribute(attributes, "name"))
                        throw new InvalidOperationException(String.Format("Se esperaba un objeto de nombre '{0}'.", name));
                }

                type = null;
                id = Convert.ToInt32(GetAttribute(attributes, "id", true));
            }

            else
                throw new InvalidDataException("Se esperaba un nodo 'null', 'object' o 'reference'.");
        }

        /// <summary>
        /// Llegeix el final d'un objecte.
        /// </summary>
        public override void ReadObjectEnd() {

            reader.Read();
        }

        /// <summary>
        /// Llegeix l'inici d'un struct.
        /// </summary>
        /// <param name="name">Nom del node.</param>
        /// <param name="type">Tipus del struct.</param>
        public override void ReadStructStart(string name, Type type) {

            reader.Read();

            if (reader.Name != "struct")
                throw new InvalidDataException("Se esperaba 'struct'.");
        }

        /// <summary>
        /// Llegeix el final d'un struct.
        /// </summary>
        public override void ReadStructEnd() {

            reader.Read();
        }

        public override void ReadArrayStart(string name, out int count, out int[] bound) {

            reader.Read();

            if (reader.Name == "null") {

                count = 0;
                bound = null;
            }

            else if (reader.Name == "array") {
                IDictionary<string, string> attributes = reader.GetAttributes();
                if (useNames && settings.CheckNames) {
                    if (name != GetAttribute(attributes, "name"))
                        throw new InvalidOperationException(String.Format("Se esperaba 'name={0}',", name));
                }

                string[] boundStr = attributes["bound"].Split(new char[] { ',' });
                bound = new int[boundStr.Length];
                for (int i = 0; i < boundStr.Length; i++)
                    bound[i] = Convert.ToInt32(boundStr[i]);

                count = Convert.ToInt32(GetAttribute(attributes, "count"));
            }

            else
                throw new InvalidDataException("Se esperaba 'null' o 'array'.");
        }

        /// <summary>
        /// Llegeix el final d'un array CLI
        /// </summary>
        public override void ReadArrayEnd() {

            reader.Read();
        }

        /// <summary>
        /// Obte el valor d'un atribut.
        /// </summary>
        /// <param name="attributes">Diccionari d'atributs.</param>
        /// <param name="name">Nom del atribut.</param>
        /// <param name="required">Indica si es obligatori i no pot ser nul.</param>
        /// <returns>El valor de l'atribut.</returns>
        private static string GetAttribute(IDictionary<string, string> attributes, string name, bool required = false) {

            if (attributes.ContainsKey(name))
                return attributes[name];
            else if (!required)
                return null;
            else
                throw new Exception(String.Format("El atributo obligatorio '{0}' no existe.", name));
        }

        /// <summary>
        /// Obte el contingut d'un node.
        /// </summary>
        /// <returns></returns>
        private string GetContent() {

            string value = String.Empty;

            if (!reader.IsEmptyElement) {
                reader.Read();
                if (reader.NodeType != XmlNodeType.EndElement) {
                    value = reader.Value;
                    reader.Read();
                }
            }

            return value;
        }

        private Type TypeFromString(string typeName) {

            return typeNameConverter.ToType(typeName);
        }

        /// <summary>
        /// Converteix una cadena a objecte.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        private object ValueFromString(Type type, string value) {

            if (value == null)
                return null;

            else {
                // Tipus 'char'
                //
                if (type == typeof(char))
                    return Convert.ToChar(UInt16.Parse(value));

                // Tipus 'string'
                //
                else if (type == typeof(string)) {
                    if (settings.EncodedStrings) {
                        byte[] bytes = Convert.FromBase64String(value);
                        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    }
                    else
                        return value;
                }

                // Altres tipus
                //
                else {
                    TypeConverter converter = TypeDescriptor.GetConverter(type);
                    if ((converter != null) && converter.CanConvertFrom(typeof(string)))
                        return converter.ConvertFromString(null, settings.Culture, value);
                    else
                        return Convert.ChangeType(value, type);
                }
            }
        }

        /// <summary>
        /// Obte el numero de versio.
        /// </summary>
        public override int Version {
            get {
                return dataVersion;
            }
        }
    }
}
