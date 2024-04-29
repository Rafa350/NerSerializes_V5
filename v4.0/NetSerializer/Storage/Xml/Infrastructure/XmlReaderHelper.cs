namespace MikroPic.NetSerializer.v4.Storage.Xml.Infrastructure {

    using System.Collections.Generic;
    using System.Xml;

    internal static class XmlReaderHelper {

        public static IDictionary<string, string> GetAttributes(this XmlReader reader) {

            Dictionary<string, string> attributes = new Dictionary<string, string>();
            if (reader.HasAttributes) {
                while (reader.MoveToNextAttribute())
                    attributes.Add(reader.Name, reader.Value);
                reader.MoveToElement();
            }

            return attributes;
        }
    }
}
