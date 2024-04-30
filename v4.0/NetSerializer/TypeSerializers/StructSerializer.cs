namespace NetSerializer.v4.TypeSerializers {

    using System;
    using System.Reflection;
    using NetSerializer.v4.Infrastructure;
    using NetSerializer.v4.Storage;
    
    public class StructSerializer: SerializerBase {

        public override bool CanSerialize(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        public override void Serialize(TypeSerializer typeSerializer, StorageWriter writer, string name, Type type, object obj) {

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            if (writer == null)
                throw new ArgumentNullException("writer");

            if (type == null)
                throw new ArgumentNullException("type");

            if (!CanSerialize(type))
                throw new InvalidOperationException(
                    String.Format("No es posible serializar el tipo '{0}'.", type.ToString()));

            writer.WriteStructStart(name, type, obj);

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);

            if (descriptor.SerializationMethod != null) {
                SerializationContext context = new SerializationContext(writer, typeSerializer);
                descriptor.SerializationMethod.Invoke(obj, new object[] { context });
            }
            else 
                SerializeStruct(typeSerializer, writer, obj);
            
            writer.WriteStructEnd();
        }

        public override void Deserialize(TypeSerializer typeSerializer, StorageReader reader, string name, Type type, out object obj) {

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            if (reader == null)
                throw new ArgumentNullException("reader");

            if (type == null)
                throw new ArgumentNullException("type");

            if (!CanSerialize(type))
                throw new InvalidOperationException(
                    String.Format("No es posible deserializar el tipo '{0}'.", type.ToString()));

            reader.ReadStructStart(name, type);

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);
            DeserializationContext context = new DeserializationContext(reader, typeSerializer);

            if (descriptor.CreatorMethod != null)
                obj = descriptor.CreatorMethod.Invoke(null, new object[] { context });

            else {
                obj = Activator.CreateInstance(type);

                if (descriptor.DeserializationMethod != null)
                    descriptor.DeserializationMethod.Invoke(obj, new object[] { context });
                else
                    DeserializeStruct(typeSerializer, reader, obj);
            }
            
            reader.ReadStructEnd();
        }

        protected virtual void SerializeStruct(TypeSerializer typeSerializer, StorageWriter writer, object obj) {

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);

            foreach (PropertyInfo propertyInfo in descriptor.PropertyInfos) {
                object value = propertyInfo.GetValue(obj, null);
                typeSerializer.Serialize(writer, propertyInfo.Name, propertyInfo.PropertyType, value);
            }
        }

        protected virtual void DeserializeStruct(TypeSerializer typeSerializer, StorageReader reader, object obj) {

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);

            foreach (PropertyInfo propertyInfo in descriptor.PropertyInfos) {
                object value;
                typeSerializer.Deserialize(reader, propertyInfo.Name, propertyInfo.PropertyType, out value);
                propertyInfo.SetValue(obj, value, null);
            }
        }
    }
}
