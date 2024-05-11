using System.Xml;

namespace NetSerializer.V6.Formatters.Xml.Infrastructure {

    internal static class XmlReaderExtensions {

        public static bool AttributeExist(this XmlReader reader, string name) {

            return reader.GetAttribute(name) != null;
        }

        public static bool GetAttributeAsBool(this XmlReader reader, string name) {

            var value = reader.GetAttribute(name);
            if (value == null)
                throw new InvalidOperationException($"No se encontro el valor del atributo '{name}'.");

            return Boolean.Parse(value);
        }

        public static int GetAttributeAsInt(this XmlReader reader, string name) {

            var value = reader.GetAttribute(name);
            if (value == null)
                throw new InvalidOperationException($"No se encontro el valor del atributo '{name}'.");

            return Int32.Parse(value);
        }

        public static string GetAttributeAsString(this XmlReader reader, string name) {

            var value = reader.GetAttribute(name);
            if (value == null)
                throw new InvalidOperationException($"No se encontro el valor del atributo '{name}'.");

            return value;
        }
    }
}
