using System.Xml;

namespace NetSerializer.V6.Formatters.Xml.Infrastructure {

    internal static class XmlWriterExtensions {

        public static void WriteAttributeBool(this XmlWriter writer, string localName, bool value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeInt(this XmlWriter writer, string localName, int value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeInt(this XmlWriter writer, string localName, int[] value) {

            writer.WriteStartAttribute(localName);

            bool first = true;
            foreach (var v in value) {
                if (first)
                    first = false;
                else
                    writer.WriteValue(", ");
                writer.WriteValue(v);
            }

            writer.WriteEndAttribute();
        }

        public static void WriteAttributeFloat(this XmlWriter writer, string localName, float value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeDouble(this XmlWriter writer, string localName, double value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeDouble(this XmlWriter writer, string localName, Decimal value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }

        public static void WriteAttributeDouble(this XmlWriter writer, string localName, DateTime value) {

            writer.WriteStartAttribute(localName);
            writer.WriteValue(value);
            writer.WriteEndAttribute();
        }
    }
}
