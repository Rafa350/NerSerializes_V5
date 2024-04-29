namespace MikroPic.NetSerializer.v4.TypeSerializers {

    using System;
    using System.Collections;

    /// <summary>
    /// Serializador de colectiones. Aquesta clase es una especialitzacio
    /// de ObjectSerializer, per operar amb objectes que implememten IList
    /// </summary>
    public sealed class ListSerializer: ClassSerializer  {

        /// <summary>
        /// Comprova si es pot derialitzar un objecte del tipus especificat.
        /// </summary>
        /// <param name="type">El tipus del objecte.</param>
        /// <returns></returns>
        public override bool CanSerialize(Type type) {

            return typeof(IList).IsAssignableFrom(type);
        }

        /// <summary>
        /// Serialitza el objecte.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="writer">El escriptor de dades.</param>
        /// <param name="obj">El objecte a serialitzar.</param>
        protected override void SerializeObject(SerializationContext context, object obj) {

            IList list = obj as IList;

            context.Write("$count", list.Count);
            foreach (object item in list)
                context.Write(null, item);
        }

        /// <summary>
        /// Deserialitza el objecte.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="reader">El lector de dades.</param>
        /// <param name="obj">El objecte a deserialitzar.</param>
        /// <param name="version">El numero de versio.</param>
        protected override void DeserializeObject(DeserializationContext context, object obj, int version) {

            IList list = obj as IList;
            Type itemType = list.GetType().GetGenericArguments()[0];

            int count;
            context.Read("$count", out count);

            while (count-- > 0) {
                object item;
                context.Read(null, out item, itemType);
                list.Add(item);
            }
        }
    }
}
