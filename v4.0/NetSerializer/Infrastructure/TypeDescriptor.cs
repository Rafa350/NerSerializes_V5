namespace NetSerializer.v4.Infrastructure {

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public sealed class TypeDescriptor {

        private const string serializationMethodName = "Serialize";
        private const string deserializationMethodName = "Deserialize";
        private const string creatorMethodName = "Create";

        private readonly Type type;
        private readonly ConstructorInfo deserializationCtor;
        private readonly MethodInfo serializationMethod;
        private readonly MethodInfo deserializationMethod;
        private readonly MethodInfo creatorMethod;
        private readonly List<PropertyInfo> propertyInfos = new List<PropertyInfo>();

        public TypeDescriptor(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            this.type = type;

            // Obte el constructor '.ctor(DeserializationContext)'
            //
            deserializationCtor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, new Type[] { typeof(DeserializationContext) }, null);

            // Obte el creador 'Create(DeserializationContext)'
            //
            creatorMethod = type.GetMethod(creatorMethodName,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null, new Type[] { typeof(DeserializationContext) }, null);

            // Obte el serialitzador 'Serialize(SerializationContext)'
            //
            serializationMethod = type.GetMethod(serializationMethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, new Type[] { typeof(SerializationContext) }, null);

            // Obte el deserialitzador 'Deserialize(DeserializationContext)'
            //
            deserializationMethod = type.GetMethod(deserializationMethodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                null, new Type[] { typeof(DeserializationContext) }, null);

            // Obte les propietats serializables
            //
            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)) {

                // Si no te 'getter' la descarta
                //
                if (!propertyInfo.CanRead)
                    continue;

                // Si no te 'setter' la descarta
                //
                if (!propertyInfo.CanWrite)
                    continue;

                // Si es una propietat indexada la descarta
                //
                if (propertyInfo.GetIndexParameters().Length > 0)
                    continue;

                propertyInfos.Add(propertyInfo);
            }

        }

        public ConstructorInfo DeserializationCtor {
            get {
                return deserializationCtor;
            }
        }

        public MethodInfo CreatorMethod {
            get {
                return creatorMethod;
            }
        }

        public MethodInfo SerializationMethod {
            get {
                return serializationMethod;
            }
        }

        public MethodInfo DeserializationMethod {
            get {
                return deserializationMethod;
            }
        }

        public IEnumerable<PropertyInfo> PropertyInfos {
            get {
                return propertyInfos;
            }
        }
    }
}

