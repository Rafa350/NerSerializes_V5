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

                if (value == null)
                    context.WriteObject(name, null);

                else {
                    var type = value.GetType();

                    if (type == typeof(bool))
                        context.WriteBool(name, (bool)value);

                    else if (type == typeof(int))
                        context.WriteInt(name, (int)value);

                    else if (type == typeof(float))
                        context.WriteSingle(name, (float)value);

                    else if (type == typeof(double))
                        context.WriteDouble(name, (double)value);

                    else if (type == typeof(decimal))
                        context.WriteDecimal(name, (decimal)value);

                    else if (type == typeof(string))
                        context.WriteString(name, (string)value);

                    else if (type.IsEnum) 
                        context.WriteEnum(name, (Enum)value);

                    else
                        context.WriteObject(name, value);
                }
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

                if (type == typeof(bool))
                    propertyDescriptor.SetValue(obj, context.ReadBool(name));

                else if (type == typeof(int))
                    propertyDescriptor.SetValue(obj, context.ReadInt(name));

                else if (type == typeof(float))
                    propertyDescriptor.SetValue(obj, context.ReadSingle(name));

                else if (type == typeof(double))
                    propertyDescriptor.SetValue(obj, context.ReadDouble(name));

                else if (type == typeof(decimal))
                    propertyDescriptor.SetValue(obj, context.ReadDecimal(name));

                else if (type == typeof(string))
                    propertyDescriptor.SetValue(obj, context.ReadString(name));

                else if (type.IsEnum)
                    propertyDescriptor.SetValue(obj, context.ReadEnum(name, propertyDescriptor.Type));

                else
                    propertyDescriptor.SetValue(obj, context.ReadObject(name));
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
