namespace Test {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using NetSerializer.v4;
    using NetSerializer.v4.Storage;
    using NetSerializer.v4.Storage.Bin;
    using NetSerializer.v4.Storage.Xml;
    using NetSerializer.v4.TypeSerializers;

    public enum Numbers {
        Zero,
        One,
        Two,
        Three
    }

    public class ClassOfValues {

        private Boolean boolValue = true;
        private DateTime dateTimeValue = DateTime.Now;
        private TimeSpan timeSpanValue = TimeSpan.FromSeconds(123456);
    }

    public class ClassOfIntegers {

        private byte byteValue = 12;
        private sbyte sbyteValue = -34;
        private short int16Value = -1234;
        private int int32Value = -12345678;
        private long int64Value = -1234567890987654321;
        private ushort uint16Value = 1234;
        private uint uint32Value = 12345678;
        private ulong uint64Value = 1234567890987654321;

        public byte ByteValue { get { return byteValue; } set { byteValue = value; } }
        public sbyte SByteValue { get { return sbyteValue; } set { sbyteValue = value; } }
        public short Int16Value { get { return int16Value; } set { int16Value = value; } }
        public int Int32Value { get { return int32Value; } set { int32Value = value; } }
        public long Int64Value { get { return int64Value; } set { int64Value = value; } }
        public ushort UInt16Value { get { return uint16Value; } set { uint16Value = value; } }
        public uint UInt32Value { get { return uint32Value; } set { uint32Value = value; } }
        public ulong UInt64Value { get { return uint64Value; } set { uint64Value = value; } }
    }

    public sealed class ClassOfNumbers {

        private float singleValue = -13.0e10f;
        private double doubleValue = -52.0e34;
        private decimal decimalValue = 1234523.45m;

        public float SingleValue { get { return singleValue; } set { singleValue = value; } }
        public double DoubleValue { get { return doubleValue; } set { doubleValue = value; } }
        public decimal DecimalValue { get { return decimalValue; } set { decimalValue = value; } }
    }

    public sealed class ClassOfEnums {

        private Numbers numbers = Numbers.Zero;

        public Numbers Numbers { get { return numbers; } set { numbers = value; } }
    }

    public sealed class ClassOfArrays {

        private int[] valueArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        private Numbers[] enumArray = new Numbers[] { Numbers.Zero, Numbers.One, Numbers.Three };
        private string[] stringArray = new string[] { "StringOne", "StringTwo", "StringThree" };

        public int[] ValueArray { get { return valueArray; } set { valueArray = value; } }
        public Numbers[] EnumArray { get { return enumArray; } set { enumArray = value; } }
        public string[] StringArray { get { return stringArray; } set { stringArray = value; } }
        public int[] NullValueArray { get; set; }
        public Numbers[] NullEnumArray { get; set; }
        public string[] NullStringArray { get; set; }
    }

    public sealed class ClassOfCollections {

        private List<int> intList = new List<int>(new int[] { 12, 34, 56, 78 });
        private List<string> stringList = new List<string>(new string[] { "****String1", "****String2", "****String3", "****String4" });
        private List<object> objectList = new List<object>(new object[] { new ClassOfNumbers(), new ClassOfIntegers() });

        public List<int> IntegerList { get { return intList; } set { intList = value; } }
        public List<string> StringList { get { return stringList; } set { stringList = value; } }
        public List<object> ObjectList { get { return objectList; } set { objectList = value; } }
    }

    public class TestClassBase {

        private string className = "TestClassBase";

        public string ClassName { get { return className; } set { className = value; } }
    }

    public class TestClass: TestClassBase {

        public DateTime Now { get; set; }
        public TimeSpan Interval { get; set; }
        public int IntegerValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }
        public char Caracter1 { get; set; }
        public char Caracter2 { get; set; }
        public char NullCharacter { get; set; }
        //public object[] Cosas { get; set; }

        public TestClass() {

            Now = DateTime.Now;
            IntegerValue = 23456;
            DoubleValue = 123.456;
            BoolValue = true;
            Caracter1 = 'A';
            Caracter2 = '\x10';
            NullCharacter = '\0';

            /*Cosas = new object[4];
            Cosas[0] = 123;
            Cosas[1] = true;
            Cosas[2] = 12.45;
            Cosas[3] = "capullo";*/
        }
    }

    public class TestBase {

        public string Titulo { get; set; }
    }

    public class TestCustomSerializer {

        private double vDouble = 123.456;
        private int vInt = 1234;
        private string vString = "Hola que tal";
        private object nullObject = null;
    
        public void Serialize(SerializationContext context) {

            context
                .Write("nullObject", nullObject)
                .Write("vDouble", vDouble)
                .Write("vInt", vInt)
                .Write("vString", vString);
        }

        public double VDouble { get { return vDouble; } }
        public int VInt { get { return vInt; } }
        public object NullObject { get { return nullObject; } }
        public string VString { get { return vString; } }

        public void Deserialize(DeserializationContext context) {

            context
                .Read("nullObject", out nullObject)
                .Read("vDouble", out vDouble)
                .Read("vInt", out vInt)
                .Read("vString", out vString);
        }
    }

    public static class TestBaseHelper {

        public static void Serializer(this TestBase x, SerializationContext context) {
        }
    }

    public class Test: TestBase {

        private ClassOfNumbers classOfNumbers = new ClassOfNumbers();
        private ClassOfIntegers classOfIntegers = new ClassOfIntegers();
        private ClassOfEnums classOfEnums = new ClassOfEnums();
        private ClassOfArrays classOfArrays = new ClassOfArrays();
        private ClassOfCollections classOfCollections = new ClassOfCollections();

        public ClassOfNumbers ClassOfNumbers { get { return classOfNumbers; } set { classOfNumbers = value; } }
        public ClassOfIntegers ClassOfIntegers { get { return classOfIntegers; } set { classOfIntegers = value; } }
        public ClassOfEnums ClassOfEnums { get { return classOfEnums; } set { classOfEnums = value; } }
        public ClassOfArrays ClassOfArrays { get { return classOfArrays; } set { classOfArrays = value; } }
        public ClassOfCollections ClassOfCollections { get { return classOfCollections; } set { classOfCollections = value; } }

        //public object IntObject { get; set; } No soportat encara
        public Dictionary<string, string> dict { get; set; }
        public TestCustomSerializer cs { get; set; }
        public string StringValue { get; set; }
        public string NullStringValue1 { get; set; }
        public string NullStringValue2 { get; set; }
        public string NullStringValue3 { get; set; }
        public string EmptyString { get; set; }
        public bool BoolValue2 { get; set; }
        public string[] StringValueArray { get; set; }
        public object[] ClassArray { get; set; }
        public Test[] TestArray { get; set; }
        public TestClass Class1 { get; set; }
        public TestClass Class2 { get; set; }
        public TestClass Class3 { get; set; }
        public TestClass Class4 { get; set; }
        public TestClass Class5 { get; set; }
        public Numbers Numbers { get; set; }
        public Type TypeValue { get; set; }
        public int[,] MultidimensionalArray { get; set; }
        //public List<TestClassBase> ObjectList { get; set; }

        public StructOfValues StructOfValues { get; set; }
        public StructOfReferences StructOfReferences { get; set; }
    }

    /// <summary>
    /// Estructura inmutable amb propietats ValueType
    /// </summary>
    public struct StructOfValues {
        private int integerValue;
        private double doubleValue;
        private string stringValue;
        public StructOfValues(int integerValue, double doubleValue, string stringValue) {
            this.integerValue = integerValue;
            this.doubleValue = doubleValue;
            this.stringValue = stringValue;
        }
        public int IntegerValue { get { return integerValue; } }
        public double DoubleValue { get { return doubleValue; } }
        public string StringValue { get { return stringValue; } }

        internal void Serialize(SerializationContext context) {

            context
                .Write("IntegerValue", integerValue)
                .Write("DoubleValue", doubleValue)
                .Write("StringValue", stringValue);
        }

        internal static StructOfValues Create(DeserializationContext context) {

            int integerValue;
            double doubleValue;
            string stringValue;

            context
                .Read("IntegerValue", out integerValue)
                .Read("DoubleValue", out doubleValue)
                .Read("StringValue", out stringValue);

            return new StructOfValues(integerValue, doubleValue, stringValue);
        }
    }

    /// <summary>
    /// Estructura no inmutable amb propietats ObjectType
    /// </summary>
    public struct StructOfReferences {
        public string StringValue { get; set; }
        public object Reference { get; set; }
    }

    public sealed class StructOfValuesSerializer: SerializerBase {

        public override bool CanSerialize(Type type) {

            return type == typeof(StructOfValues);
        }

        public override void Serialize(TypeSerializer typeSerializer, StorageWriter writer, string name, Type type, object obj) {

            StructOfValues s = (StructOfValues) obj;
            writer.WriteStructStart(name, type, obj);
            typeSerializer.Serialize(writer, "IntegerValue", typeof(int), s.IntegerValue);
            typeSerializer.Serialize(writer, "DoubleValue", typeof(double), s.DoubleValue);
            typeSerializer.Serialize(writer, "StringValue", typeof(string), s.StringValue);
            writer.WriteStructEnd();
        }

        public override void Deserialize(TypeSerializer typeSerializer, StorageReader reader, string name, Type type, out object obj) {

            object integerValue;
            object doubleValue;
            object stringValue;

            reader.ReadStructStart(name, type);
            typeSerializer.Deserialize(reader, "IntegerValue", typeof(int), out integerValue);
            typeSerializer.Deserialize(reader, "DoubleValue", typeof(double), out doubleValue);
            typeSerializer.Deserialize(reader, "StringValue", typeof(string), out stringValue);
            reader.ReadStructEnd();

            obj = new StructOfValues((int) integerValue, (double) doubleValue, (string) stringValue);
        }
    }
        
    class Program {

        static void Main(string[] args) {

            //TypeSerializer.Instance.AddSerializer(new StructOfValuesSerializer());

            TypeRegister tr = TypeRegister.Instance;
            tr.Register(typeof(Test), "TestType");
            tr.Register(typeof(Test[]), "ArrayTestType");
            tr.Register(typeof(TestClass), "TestClass");
            tr.Register(typeof(TestCustomSerializer));
            tr.Register(typeof(Dictionary<string, string>), "StringStringDictionary");
            tr.Register(typeof(List<int>), "IntegerList");

            Test t = new Test();
            t.cs = new TestCustomSerializer();

            t.Titulo = "Hola que tal";
            t.BoolValue2 = false;
            //t.IntObject = 12345;

            t.Numbers = Numbers.Three;

            t.StringValue = "KK de vaca";
            t.NullStringValue1 = null;
            t.NullStringValue2 = null;
            t.NullStringValue3 = null;
            t.EmptyString = String.Empty;

            t.Class1 = new TestClass();
            t.Class2 = t.Class1;
            t.Class5 = t.Class1;

            t.ClassArray = new TestClass[5];
            for (int i = 0; i < t.ClassArray.Length; i++)
                if (i < 3)
                    t.ClassArray[i] = new TestClass();
                else
                    t.ClassArray[i] = null;

            t.TestArray = new Test[2];
            t.TestArray[0] = new Test();
            t.TestArray[1] = new Test();

            t.MultidimensionalArray = new int[2, 3];
            t.MultidimensionalArray[0, 0] = 1;
            t.MultidimensionalArray[0, 1] = 2;
            t.MultidimensionalArray[0, 2] = 3;
            t.MultidimensionalArray[1, 0] = 4;
            t.MultidimensionalArray[1, 1] = 5;
            t.MultidimensionalArray[1, 2] = 6;

            //t.ObjectList = new List<TestClassBase>();
            //t.ObjectList.Add(new TestClass());
            //t.ObjectList.Add(new TestClass());

            t.dict = new Dictionary<string, string>();
            t.dict.Add("Hola", "Adios");
            t.dict.Add("Cerdo", "Capullo");

            t.StructOfValues = new StructOfValues(10, 24.56, "PericoPalotes");
            t.StructOfReferences = new StructOfReferences {
                StringValue = "hola",
                Reference = new TestBase()
            };


            /*Transform(
                @"c:\temp\test_input.xml",
                @"c:\temp\test_output.xml");
            */
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //for (int i = 0; i < 200; i++) {
                XmlSerialize(@"c:\temp\proves_v2.xml", t);
                object o1 = XmlDeserialize(@"c:\temp\proves_v2.xml");
                XmlSerialize(@"c:\temp\proves_v2_b.xml", o1);
            //}
            //sw.Stop();
            //TimeSpan elapsed = sw.Elapsed;

            /*BinSerialize(@"c:\temp\proves_v2.bin", t);
            object o2 = BinDeserialize(@"c:\temp\proves_v2.bin");
            //BinSerialize(@"c:\temp\proves_v2_b.bin", o2);
            XmlSerialize(@"c:\temp\proves_v2_b.xml", o2);*/
        }

        private static void Transform(string inputFilename, string outputFileName) {

            string xslFileName = "transform.xsl";

            using (Stream input = new FileStream(inputFilename, FileMode.Open, FileAccess.Read, FileShare.None)) {
                using (Stream output = new FileStream(outputFileName, FileMode.Create, FileAccess.Write, FileShare.None)) {

                    Stream xslInput = new FileStream(xslFileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    XmlReaderPreprocessor pp = new XmlReaderPreprocessor(xslInput);
                    pp.Process(input, output, true);
                }
            }
        }

        private static void BinSerialize(string fileName, object obj) {

            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            StorageWriter writer = new BinStorageWriter(stream, null);
            Serializer s = new Serializer(writer, 100);
            try {
                s.Serialize(obj, "root");
            }
            finally {
                s.Close();
            }
        }

        private static void XmlSerialize(string fileName, object obj) {

            Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            StorageWriter writer = new XmlStorageWriter(stream, null);
            Serializer s = new Serializer(writer, 100);
            try {
                s.Serialize(obj, "root");
            }
            finally {
                s.Close();
            }
        }

        private static object XmlDeserialize(string fileName) {

            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StorageReader reader = new XmlStorageReader(stream, null);
            Deserializer d = new Deserializer(reader);
            try {
                return d.Deserialize(typeof(Test), "root");
            }
            finally {
                d.Close();
            }
        }

        private static object BinDeserialize(string fileName) {

            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
            StorageReader reader = new BinStorageReader(stream, null);
            Deserializer d = new Deserializer(reader);
            try {
                return d.Deserialize(typeof(Test), "root");
            }
            finally {
                d.Close();
            }
        }
    }
}
