namespace NetSerializer.V6.TypeSerializers.Serializers {

    public class StructSerializer: ClassSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsStructType();

        /// <inheritdoc/>
        /// 
        public override void Serialize(SerializationContext context, string name, object obj) {

            // Si no es pot convertir a valor, es serializa com una clase
            //
            base.Serialize(context, name, obj);
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, string name, object obj) {

            // Si no es pot convertir a valor, es deserializa com una clase
            //
            base.Deserialize(context, name, obj);
        }
    }
}
