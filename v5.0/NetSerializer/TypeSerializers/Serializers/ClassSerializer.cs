using System;
using NetSerializer.V5.Descriptors;
using NetSerializer.V5.Formatters;

namespace NetSerializer.V5.TypeSerializers.Serializers {

    /// <summary>
    /// Serializador de clases.
    /// </summary>
    /// 
    public class ClassSerializer: TypeSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsClass && !type.IsArray && !type.IsSpecialClass();

        /// <summary>
        /// Indica si es pot procesar la propietat.
        /// </summary>
        /// <param name="propertyDescriptor">Descreiptor de la propietat.</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        protected virtual bool CanProcessProperty(PropertyDescriptor propertyDescriptor) {

            return propertyDescriptor.CanGetValue && propertyDescriptor.CanSetValue;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Aqui el tipus del objecte 'obj' ja es conegut i per tant es posible que 'this' sigui una clase
        /// derivada de 'CustomClassSerializer'.
        /// </remarks>
        /// 
        public override void Serialize(SerializationContext context, string name, Type type, object obj) {

            if (!CanProcess(type))
                throw new InvalidOperationException(
                    String.Format("No es posible serializar el tipo '{0}'.", type.ToString()));

            var writer = context.Writer;

            if (obj == null)
                writer.WriteNull(name);

            else {
                if (!type.IsAssignableFrom(obj.GetType()))
                    throw new InvalidOperationException(
                        String.Format("El objeto a serializar no hereda del tipo '{0}'.", type.ToString()));

                int id = context.GetObjectId(obj);
                if (id == -1) {
                    id = context.RegisterObject(obj);
                    writer.WriteObjectHeader(name, obj, id);

                    SerializeObject(context, obj);

                    writer.WriteObjectTail();
                }
                else
                    writer.WriteObjectReference(name, id);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Com el objecte 'obj' a carregar no es conegut, el seu tipus sera el de la seva clase base, per tant
        /// un cop es conegui el tipus cal cridar al serialitzador adecuat si en te un propi.
        /// </remarks>
        /// 
        public override void Deserialize(DeserializationContext context, string name, Type type, out object obj) {

            if (!CanProcess(type))
                throw new InvalidOperationException(
                    String.Format("No es posible deserializar el tipo '{0}'.", type.ToString()));

            var reader = context.Reader;

            ReadObjectResult result = reader.ReadObjectHeader(name);
            if (result.ResultType == ReadObjectResultType.Object) {

                var objectType = result.ObjectType;

                if (!type.IsAssignableFrom(objectType))
                    throw new InvalidOperationException(
                        String.Format("El objecto de tipo '{0}', a deserializar no hereda del tipo '{1}'.", objectType.ToString(), type.ToString()));

                obj = CreateObject(context, objectType);
                context.Register(obj);

                // Es deserialitza en dos pasos, un amb ClassSerializer i despres amb CustomClassSerializer.
                // Un cop conegut el tipus del objecte, si te un serialitzador especific, l'utilitza
                //
                var serializer = context.GetTypeSerializer(objectType);
                if (serializer is CustomClassSerializer customSerializer) {
                    customSerializer.DeserializeObject(context, obj);
                }
                else
                    DeserializeObject(context, obj);

                reader.ReadObjectTail();
            }

            else if (result.ResultType == ReadObjectResultType.Reference)
                obj = context.GetObject(result.ObjectId);

            else
                obj = null;
        }

        /// <summary>
        /// Crea una instancia del objecte. Necesita un constructor sense parametres.
        /// </summary>
        /// <param name="context">El context de deserialitzacio.</param>
        /// <param name="type">El tipus d'objecte.</param>
        /// <returns>El objecte.</returns>
        /// 
        protected virtual object CreateObject(DeserializationContext context, Type type) {

            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);
            if (typeDescriptor.CanCreate)
                return typeDescriptor.Create(context);
            else
                return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Serialitzacio per defecte del objecte.
        /// </summary>
        /// <param name="context">El context de resialitzacio.</param>
        /// <param name="obj">El objecte a serialitzar.</param>
        /// 
        protected virtual void SerializeObject(SerializationContext context, object obj) {

            var type = obj.GetType();
            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);

            // Si pot, es serialitza ell mateix.
            //
            if (typeDescriptor.CanSerialize)
                typeDescriptor.Serialize(context, obj);

            // En cas contrari es serialitzen totes les propietats.
            //
            else {
                foreach (var propertyDescriptor in typeDescriptor.PropertyDescriptors)
                    if (CanProcessProperty(propertyDescriptor) && CanSerializeProperty(context, propertyDescriptor))
                        SerializeProperty(context, obj, propertyDescriptor);
            }
        }

        /// <summary>
        /// Serialitzacio per defecte d'una propietat. Nomes pot serialitzar les
        /// propietats amb 'getter'
        /// </summary>
        /// <param name="context">El context de serialitzacio.</param>
        /// <param name="obj">El objecte a serialitzar.</param>
        /// <param name="propertyDescriptor">La propietat a serialitzar</param>
        /// 
        protected virtual void SerializeProperty(SerializationContext context, object obj, PropertyDescriptor propertyDescriptor) {

            if (propertyDescriptor.CanGetValue) {

                // Obte el tipus de la propietat del valor, que pot ser una clase derivada. Si es null,
                // obte el tipus base de la propietat.
                //
                var value = propertyDescriptor.GetValue(obj);
                var type = value == null ? propertyDescriptor.Type : value.GetType();

                var serializer = context.GetTypeSerializer(type);
                serializer.Serialize(context, propertyDescriptor.Name, type, value);
            }
        }

        /// <summary>
        /// Comprova si es pot serialitzar una propietat.
        /// </summary>
        /// <param name="context">El context de serialitzacio.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// <returns>True en cas afitmatiu.</returns>
        /// 
        protected virtual bool CanSerializeProperty(SerializationContext context, PropertyDescriptor propertyDescriptor) {

            return true;
        }

        /// <summary>
        /// Deserialitzacio per defecte del objecte.
        /// </summary>
        /// <param name="reader">Objecte per la lectura de dades.</param>
        /// <param name="obj">El objecte a deserialitzar.</param>
        /// 
        protected virtual void DeserializeObject(DeserializationContext context, object obj) {

            var type = obj.GetType();
            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(type);

            // Si pot, es deserialitza ell mateix.
            //
            if (typeDescriptor.CanDeserialize)
                typeDescriptor.Deserialize(context, obj);

            // En cas contrari, es deserialitzen totes les propietats.
            //
            else {
                foreach (var propertyDescriptor in typeDescriptor.PropertyDescriptors)
                    if (CanProcessProperty(propertyDescriptor) && CanDeserializeProperty(context, propertyDescriptor))
                        DeserializeProperty(context, obj, propertyDescriptor);
            }
        }

        /// <summary>
        /// Deserialitzacio per defecte d'una propietat. Nomes es pot deserialitzar si te un 'setter'.
        /// </summary>
        /// <param name="reader">Objecte per la lectura de dades.</param>
        /// <param name="obj">L'objecte.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// 
        protected virtual void DeserializeProperty(DeserializationContext context, object obj, PropertyDescriptor propertyDescriptor) {

            if (propertyDescriptor.CanSetValue) {
                var serializer = context.GetTypeSerializer(propertyDescriptor.Type);
                serializer.Deserialize(context, propertyDescriptor.Name, propertyDescriptor.Type, out object value);
                propertyDescriptor.SetValue(obj, value);
            }
        }

        /// <summary>
        /// Comprova si es pot deserialitzar una propietat.
        /// </summary>
        /// <param name="context">El context de deserialitzacio.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        protected virtual bool CanDeserializeProperty(DeserializationContext context, PropertyDescriptor propertyDescriptor) {

            return true;
        }
    }
}
