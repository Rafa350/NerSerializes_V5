namespace NetSerializer.v4.TypeSerializers {

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NetSerializer.v4;
    using NetSerializer.v4.Infrastructure;
    using NetSerializer.v4.Storage;
    
    /// <summary>
    /// Serializador d'objetes.
    /// </summary>
    public class ClassSerializer: SerializerBase {

        private readonly List<object> objList = new List<object>();

        /// <summary>
        /// Comprova si pot serialitzar el tipus d'objecte especificat.
        /// </summary>
        /// <param name="type">El tipus d'objecte.</param>
        /// <returns>True si es posible serialitzar. False en cas contrari.</returns>
        public override bool CanSerialize(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsClass;
        }

        /// <summary>
        /// Serialitza l'objecte.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="writer">El objecte per escriure en el magatzem.</param>
        /// <param name="name">El nom del objecte.</param>
        /// <param name="type">El tipus del objecte.</param>
        /// <param name="obj">El objecte. Pot ser null.</param>
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

            if (obj == null)
                writer.WriteNull(name);

            else {
                if (!type.IsAssignableFrom(obj.GetType()))
                    throw new InvalidOperationException(
                        String.Format("El objeto a serializar no hereda del tipo '{0}'.", type.ToString()));

                int id = objList.IndexOf(obj);
                if (id == -1) {
                    objList.Add(obj);
                    id = objList.Count - 1;

                    writer.WriteObjectStart(name, obj.GetType(), id);

                    // Serialitza l'objecte
                    //
                    SerializationContext context = new SerializationContext(writer, typeSerializer);
                    TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);
                    if (descriptor.SerializationMethod != null)
                        descriptor.SerializationMethod.Invoke(obj, new object[] { context });                    
                    else
                        SerializeObject(context, obj);

                    writer.WriteObjectEnd();
                }

                else
                    writer.WriteObjectReference(name, id);
            }
        }

        /// <summary>
        /// Deserialitza el objecte.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="reader">El objecte per lleigir del magatzem.</param>
        /// <param name="name">El nom del objecte</param>
        /// <param name="type">El tipus d'objecte</param>
        /// <param name="obj">El objecte deserialitzat.</param>
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

            Type objectType;
            int objectId;
            reader.ReadObjectStart(name, out objectType, out objectId);

            // El objecte lleigit es null
            //
            if (objectId == -1)
                obj = null;

            // El objecte esta serialitzat
            //
            else if (objectType != null) {

                if (!type.IsAssignableFrom(objectType))
                    throw new InvalidOperationException(
                        String.Format("El objecto de tipo '{0}', a deserializar no hereda del tipo '{1}'.", objectType.ToString(), type.ToString()));

                TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(objectType);
                DeserializationContext context = new DeserializationContext(reader, typeSerializer);

                // Crea el objecte
                //
                if (descriptor.DeserializationCtor != null)
                    obj = descriptor.DeserializationCtor.Invoke(new object[] { context });
                else
                    obj = Activator.CreateInstance(objectType);

                // Porta la instancia al cache, per properes referencies.
                //
                objList.Add(obj);

                // Deserialitza l'objecte
                //
                if (descriptor.DeserializationMethod != null)
                    descriptor.DeserializationMethod.Invoke(obj, new object[] { context });
                else
                    DeserializeObject(context, obj, reader.Version);

                reader.ReadObjectEnd();
            }

            // El objecte es una referencia
            //
            else
                obj = objList[objectId];
        }

        /// <summary>
        /// Serialitzacio per defecte del objecte.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj">El objecte a serialitzar.</param>
        protected virtual void SerializeObject(SerializationContext context, object obj) {

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);

            foreach (PropertyInfo propertyInfo in descriptor.PropertyInfos) {
                object value = propertyInfo.GetValue(obj, null);
                context.Write(propertyInfo.Name, value);
            }
        }

        /// <summary>
        /// Deserialitzacio per defecte del objecte.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="obj">El objecte a deserialitzar.</param>
        /// <param name="version">Numero de versio.</param>
        protected virtual void DeserializeObject(DeserializationContext context, object obj, int version) {

            TypeDescriptor descriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj);

            foreach (PropertyInfo propertyInfo in descriptor.PropertyInfos) {
                object value;
                context.Read(propertyInfo.Name, out value, propertyInfo.PropertyType);
                propertyInfo.SetValue(obj, value, null);
            }
        }    
    }
}
