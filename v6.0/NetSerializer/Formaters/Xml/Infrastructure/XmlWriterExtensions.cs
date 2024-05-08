using System.Globalization;
using System.Xml;

namespace NetSerializer.V6.Formaters.Xml.Infrastructure {

    internal static class XmlWriterExtensions {

        public static void WriteAttributeBool(this XmlWriter writer, string localName, bool value) {

            writer.WriteAttributeString(localName, value.ToString());
        }

        public static void WriteAttributeInt(this XmlWriter writer, string localName, int value) {

            writer.WriteAttributeString(localName, value.ToString());
        }

        public static void WriteAttributeFloat(this XmlWriter writer, string localName, float value) {

            writer.WriteAttributeString(localName, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteAttributeDouble(this XmlWriter writer, string localName, double value) {

            writer.WriteAttributeString(localName, value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteValue(this XmlWriter writer, bool value) {

            writer.WriteValue(value.ToString());
        }

        public static void WriteValue(this XmlWriter writer, int value) {

            writer.WriteValue(value.ToString());
        }

        public static void WriteValue(this XmlWriter writer, float value) {

            writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
        }

        public static void WriteValue(this XmlWriter writer, double value) {

            writer.WriteValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
