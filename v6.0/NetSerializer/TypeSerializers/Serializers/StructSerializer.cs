namespace NetSerializer.V6.TypeSerializers.Serializers {

    public class StructSerializer: ClassSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsStructType();

        /// <inheritdoc/>
        /// 
        public override void Serialize(SerializationContext context, object obj) {

            // Si no es pot convertir a valor, es serializa com una clase
            //
            base.Serialize(context, obj);
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, object obj) {

            // Si no es pot convertir a valor, es deserializa com una clase
            //
            base.Deserialize(context, obj);
        }
    }
}
