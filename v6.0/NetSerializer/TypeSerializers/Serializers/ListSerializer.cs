using System;
using System.Collections;
using System.Diagnostics;
/*
namespace NetSerializer.V6.TypeSerializers.Serializers {

    public sealed class ListSerializer: ClassSerializer {

        public override bool CanProcess(Type type) =>
            base.CanProcess(type) && typeof(IList).IsAssignableFrom(type);

        protected override void SerializeObject(SerializationContext context, object obj) {

            var list = (IList) obj;
            var itemType = list.GetType().GetGenericArguments()[0];

            context.Writer.WriteValue("$C", list.Count);

            var typeSerializer = context.GetTypeSerializer(itemType);
            Debug.Assert(typeSerializer != null);

            for (int i = 0; i < list.Count; i++)
                typeSerializer.Serialize(context, $"${i}", itemType, list[i]);

            base.SerializeObject(context, obj);
        }

        protected override void DeserializeObject(DeserializationContext context, object obj) {

            var list = (IList) obj;
            Type itemType = list.GetType().GetGenericArguments()[0];

            int count = (int)context.Reader.ReadValue("$C", typeof(int));

            var typeSerializer = context.GetTypeSerializer(itemType);
            Debug.Assert(typeSerializer != null);

            for (int i = 0; i < count; i++) {
                typeSerializer.Deserialize(context, String.Format("${0}", i), itemType, out object? item);
                list.Add(item);
            }

            base.DeserializeObject(context, obj);
        }
    }
}
*/