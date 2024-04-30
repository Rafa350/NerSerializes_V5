namespace NetSerializer.v4 {

    using System;
    using System.Collections.Generic;
    using NetSerializer.v4.Model;

    public sealed class TypeRegister {

        private static TypeRegister instance;
        private Dictionary<Type, TypeSerializationInfo> itemsByType = new Dictionary<Type, TypeSerializationInfo>();
        private Dictionary<string, TypeSerializationInfo> itemsByAlias = new Dictionary<string, TypeSerializationInfo>();

        private TypeRegister() {

            // Registra els tipus primitius
            //
            Register(typeof(Boolean), "boolean");
            Register(typeof(Byte), "byte");
            Register(typeof(SByte), "sbyte");
            Register(typeof(Int16), "int16");
            Register(typeof(Int32), "int32");
            Register(typeof(Int64), "int64");
            Register(typeof(UInt16), "uint16");
            Register(typeof(UInt32), "uint32");
            Register(typeof(UInt64), "uint64");
            Register(typeof(Single), "single");
            Register(typeof(Double), "double");
            Register(typeof(Decimal), "decimal");
            Register(typeof(Char), "char");
            Register(typeof(String), "string");
            Register(typeof(DateTime), "datetime");
            Register(typeof(TimeSpan), "timespan");
        }

        public TypeRegister Register(Type type) {

            return Register(type, type.Name);
        }

        public TypeRegister Register(Type type, string alias) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (String.IsNullOrEmpty(alias))
                throw new ArgumentNullException("alias");

            if (itemsByType.ContainsKey(type))
                throw new InvalidOperationException(
                    String.Format("Ya se registro el tipo '{0}'.", type));

            if (itemsByAlias.ContainsKey(alias))
                throw new InvalidOperationException(
                    String.Format("Ya se registro el alias '{0}'.", alias));

            TypeSerializationInfo typeInfo = new TypeSerializationInfo(type, alias);
            itemsByType.Add(type, typeInfo);
            itemsByAlias.Add(alias, typeInfo);

            return this;
        }

        public string GetAlias(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (itemsByType.ContainsKey(type))
                return itemsByType[type].Alias;
            else
                return null;
        }

        public Type GetType(string alias) {

            if (String.IsNullOrEmpty(alias))
                throw new ArgumentNullException("alias");

            if (itemsByAlias.ContainsKey(alias))
                return itemsByAlias[alias].Type;
            else
                return null;
        }

        public static TypeRegister Instance {
            get {
                if (instance == null)
                    instance = new TypeRegister();
                return instance;
            }
        }
    }
}
