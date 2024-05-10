using System.Xml;

namespace NetSerializer.V6.Formatters.Xml.Infrastructure {

    internal static class XmlReaderExtensions {

        public static IDictionary<string, string> ReadAttributes(this XmlReader reader) {

            var attributes = new Dictionary<string, string>();
            if (reader.HasAttributes) {
                while (reader.MoveToNextAttribute())
                    attributes.Add(reader.Name, reader.Value);
                reader.MoveToElement();
            }

            return attributes;
        }
    }
}
