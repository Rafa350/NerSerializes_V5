using System.Xml;

namespace NetSerializer.V6.Formatters.Xml.Infrastructure {

    internal static class XmlReaderExtensions {

        /// <summary>
        /// Comprova si un atribut existeic.
        /// </summary>
        /// <param name="reader">El lector xml</param>
        /// <param name="name">El nom del atribut.</param>
        /// <returns>True si existeix, false en cas contrari.</returns>
        /// 
        public static bool AttributeExist(this XmlReader reader, string name) {

            return reader.GetAttribute(name) != null;
        }

        /// <summary>
        /// Obte el valor d'un atribut com valor boolean.
        /// </summary>
        /// <param name="reader">El lector xml.</param>
        /// <param name="name">El nom del atribut.</param>
        /// <returns>El resultat.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// 
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
