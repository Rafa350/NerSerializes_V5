using NetSerializer.V6;
using NetSerializer.V6.Formaters.Xml;

namespace Demo {

    internal class Program {

        class BaseClass {
            public int Prop1 { get; set; } = 123;
            public int Prop2 { get; set; } = 234;
            public int Prop3 { get; set; } = 345;
            public int Prop4 { get; set; } = 456;
            public string PropString { get; set; } = "Hola que tal";
        }

        class DerivedClass: BaseClass {
            public double Prop11 { get; set; } = 1234.5;
            public double Prop12 { get; set; } = 2345.6;
            public double Prop13 { get; set; } = 3456.7;
            public double Prop14 { get; set; } = 4567.8;
            public BaseClass? Prop15 { get; set; } = null;
        }

        static void Main(string[] args) {

            SerializePrimitives();
        }

        private static void SerializePrimitives() {

            var baseClass = new BaseClass();
            var derivedClass = new DerivedClass();
            derivedClass.Prop15 = new DerivedClass();

            using (var stream = new FileStream(@"c:\temp\serialize_primitivers.xml", FileMode.Create, FileAccess.Write, FileShare.None)) {
                using (var formatWriter = new XmlFormatWriter(stream, 100)) {
                    var serializer = new Serializer(formatWriter);
                    var ctx = serializer.Context;
                    ctx.WriteBool("Boolean", true);
                    ctx.WriteInt("Integer", 100);
                    ctx.WriteSingle("Single", 10.0000f);
                    ctx.WriteDouble("Double", 100000.5600000);
                    ctx.WriteObject("BaseClass1", baseClass);
                    ctx.WriteObject("BaseClass2", baseClass);
                    ctx.WriteObject("DerivedClass2", derivedClass);
                    ctx.WriteObject("NullObject", null);
                }
            }
        }
    }
}
