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
                    var ctx = serializer.Context;
                    ctx.WriteBool("Boolean", true);
                    ctx.WriteInt("Integer", 100);
                    ctx.WriteFloat("Float", 10.0000);
                    ctx.WriteDouble("Double", 100000.5600000);
                    ctx.WriteObject("SimpleClass1", simpleClass);
                    ctx.WriteObject("SimpleClass2", simpleClass);
                }
            }
        }
    }
}
