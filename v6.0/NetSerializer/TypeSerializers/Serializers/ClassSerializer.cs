using NetSerializer.V6.TypeDescriptors;

namespace NetSerializer.V6.TypeSerializers.Serializers {

    /// <summary>
    /// Serializador de clases.
    /// </summary>
    /// 
    public class ClassSerializer: TypeSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsClassType();

        /// <summary>
        /// Indica si es pot procesar la propietat.
        /// </summary>
        /// <param name="propertyDescriptor">Descriptor de la propietat.</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        protected virtual bool CanProcessProperty(PropertyDescriptor propertyDescriptor) {

            return propertyDescriptor.CanGetValue && propertyDescriptor.CanSetValue;
        }

        /// <inheritdoc/>
        /// 
        public override void Serialize(SerializationContext context, object obj) {

            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj.GetType());
            SerializeObject(context, obj, typeDescriptor);
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, object obj) {

            var typeDescriptor = TypeDescriptorProvider.Instance.GetDescriptor(obj.GetType());
            DeserializeObject(context, obj, typeDescriptor);
        }

        /// <summary>
        /// Serialitzacio del objecte.
        /// </summary>
        /// <param name="context">El context.</param>
        /// <param name="obj">L'objecte.</param>
        /// <param name="typeDescriptor">El descriptor del objecte.</param>
        /// 
        protected virtual void SerializeObject(SerializationContext context, object obj, TypeDescriptor typeDescriptor) {

            foreach (var propertyDescriptor in typeDescriptor.PropertyDescriptors)
                if (CanProcessProperty(propertyDescriptor) && CanSerializeProperty(context, propertyDescriptor))
                    SerializeProperty(context, obj, propertyDescriptor);
        }

        /// <summary>
        /// Serialitzacio d'una propietat. Nomes pot serialitzar les propietats amb 'getter'
        /// </summary>
        /// <param name="context">El context.</param>
        /// <param name="obj">L'objecte.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// 
        protected virtual void SerializeProperty(SerializationContext context, object obj, PropertyDescriptor propertyDescriptor) {

            if (propertyDescriptor.CanGetValue) {

                var name = propertyDescriptor.Name;
                var value = propertyDescriptor.GetValue(obj);

                context.Write(name, value);
            }
        }

        /// <summary>
        /// Comprova si es pot serialitzar una propietat.
        /// </summary>
        /// <param name="context">El context de serialitzacio.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        protected virtual bool CanSerializeProperty(SerializationContext context, PropertyDescriptor propertyDescriptor) {

            return true;
        }

        /// <summary>
        /// Deserialitzacio del objecte.
        /// </summary>
        /// <param name="context">El context.</param>
        /// <param name="obj">L'objecte.</param>
        /// <param name="typeDescriptor">El descriptor del objecte.</param>
        /// 
        protected virtual void DeserializeObject(DeserializationContext context, object obj, TypeDescriptor typeDescriptor) {

            foreach (var propertyDescriptor in typeDescriptor.PropertyDescriptors)
                if (CanProcessProperty(propertyDescriptor) && CanDeserializeProperty(context, propertyDescriptor))
                    DeserializeProperty(context, obj, propertyDescriptor);
        }

        /// <summary>
        /// Deserialitzacio d'una propietat. Nomes es pot deserialitzar si te un 'setter'.
        /// </summary>
        /// <param name="context">El context.</param>
        /// <param name="obj">L'objecte.</param>
        /// <param name="propertyDescriptor">El descriptor de la propietat.</param>
        /// 
        protected virtual void DeserializeProperty(DeserializationContext context, object obj, PropertyDescriptor propertyDescriptor) {

            if (propertyDescriptor.CanSetValue) {

                var name = propertyDescriptor.Name;
                var type = propertyDescriptor.Type;

                propertyDescriptor.SetValue(obj, context.Read(name, type));
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
