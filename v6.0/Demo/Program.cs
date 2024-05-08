using NetSerializer.V6;
using NetSerializer.V6.Formaters.Xml;

namespace Demo {

    internal class Program {

        class SimpleClass {
            public int Prop1 { get; set; } = 1;
            public int Prop2 { get; set; } = 2;
            public int Prop3 { get; set; } = 3;
            public int Prop4 { get; set; } = 4;
        }

        static void Main(string[] args) {

            SerializePrimitives();
        }

        private static void SerializePrimitives() {

            var simpleClass = new SimpleClass();

            using (var stream = new FileStream(@"c:\temp\serialize_primitivers.xml", FileMode.Create, FileAccess.Write, FileShare.None)) {

                using (var formatWriter = new XmlFormatWriter(stream, 100)) {
                    var serializer = new Serializer(formatWriter);
                    serializer.Serialize("Boolean", true);
                    serializer.Serialize("Integer", 100);
                    serializer.Serialize("Float", 10.0000);
                    serializer.Serialize("Double", 100000.5600000);
                    serializer.Serialize("SimpleClass1", simpleClass);
                    serializer.Serialize("SimpleClass2", simpleClass);
                }
            }
        }
    }
}
