using System;
using System.Diagnostics;

namespace NetSerializer.V6.TypeSerializers.Serializers {

    /// <summary>
    /// Serializa i deserializa tipos primitivos, enumeradores, DateTime y TimeStamp.
    /// </summary>
    /// 
    public class ValueSerializer: TypeSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsPrimitive || type.IsEnum || type.IsSpecialClass();

        /// <inheritdoc/>
        /// 
        public override void Serialize(SerializationContext context, string name, Type type, object? obj) {

            Debug.Assert(CanProcess(type));

            var writer = context.Writer;
            if (obj == null)
                writer.WriteNull(name);
            else
                writer.WriteValue(name, obj);
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, string name, Type type, out object obj) {

            Debug.Assert(CanProcess(type));

            var reader = context.Reader;
            obj = reader.ReadValue(name, type);
        }
    }
}
