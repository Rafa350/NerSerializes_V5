using NetSerializer.V6;
using NetSerializer.V6.Formatters.Xml;
using System.Globalization;
using NetSerializer.V6.Formatters.Xml.ValueFormatters;
using System.Xml;

namespace Demo
{

    internal class Program {

        enum EnumType {
            Hola,
            Adios
        }

        struct AStruct {
            public int IntegerProp { get; set; }
            public float SingleProp { get; set; }
            public double DoubleProp { get; set; }

            public AStruct() {
                IntegerProp = 12345;
                SingleProp = 12.345f;
                DoubleProp = 123.45;
            }
        }

        class BaseClass {
            public int PropInt1 { get; set; } = 123;
            public int PropInt2 { get; set; } = 234;
            public int PropInt3 { get; set; } = 345;
            public int PropInt4 { get; set; } = 456;
            public EnumType PropEnum { get; set; } = EnumType.Hola;
            public string PropString { get; set; } = "Hola que tal";
        }

        class DerivedClass: BaseClass {
            public double PropDouble1 { get; set; } = 1234.5;
            public double PropDouble2 { get; set; } = 2345.6;
            public double PropDouble3 { get; set; } = 3456.7;
            public double PropDouble4 { get; set; } = 4567.8;
            public char PropChar { get; set; } = 'A';
            public AStruct PropStruct { get; set; }
            public BaseClass? PropObject1 { get; set; } = null;
            public BaseClass? PropObject2 { get; set; } = null;
            public int[,] PropIntArray {  get; set; } = { { 0, 1 }, { 10, 11 }, { 20, 21 } };
        }

        static void Main(string[] args) {

            var obj1 = new DerivedClass();
            obj1.PropObject1 = new DerivedClass();
            obj1.PropObject2 = new BaseClass();
            obj1.PropStruct = new AStruct();

            Serialize(@"c:\temp\serialize_primitive1.xml", obj1);
            var obj2 = Deserialize<DerivedClass>(@"c:\temp\serialize_primitive1.xml");

            Serialize(@"c:\temp\serialize_primitive2.xml", obj2);
        }

        private static void Serialize(string fileName, object? obj) {

            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)) {
                using (var formatWriter = new XmlFormatWriter(stream, 100)) {
                    var serializer = new Serializer(formatWriter);
                    var ctx = serializer.Context;
                    ctx.Write("STR", new AStruct());
                    ctx.WriteObject("root", obj);
                }
            }
        }

        private static T? Deserialize<T>(string fileName) {
 
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                using (var formatReader = new XmlFormatReader(stream)) {
                    var deserializer = new Deserializer(formatReader);
                    var ctx = deserializer.Context;
                    var s = ctx.Read("STR", typeof(AStruct));
                    return ctx.ReadObject<T>("root");
                }
            }
        }

        public class MyValueFormatter: ValueFormatter {

            public override bool CanFormat(Type type) =>
                type == typeof(AStruct);

            public override void Write(XmlWriter writer, object obj) {

                AStruct s = (AStruct)obj;
                var value = String.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}", s.IntegerProp, s.SingleProp, s.DoubleProp);
                writer.WriteValue(value);
            }

            public override object Read(XmlReader reader) {
                reader.Read(); // Llegeix el valor
                reader.Read(); // Llegeix el tail
                return new AStruct();
            }
        }
    }
}