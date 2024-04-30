namespace NetSerializer.v4.TypeSerializers {

    using System;
    using System.Collections;

    /// <summary>
    /// Serialitza coleccions tipus diccionari. Aquesta clase es una especialitzacio
    /// de ObjectSerializer pels objectes que implementen IDictionary.
    /// </summary>
    public sealed class DictionarySerializer: ClassSerializer  {

        /// <summary>
        /// Comproba si pot serialitzar el tipus d'objecte especificat.
        /// </summary>
        /// <param name="type">El tipus d'objecte.</param>
        /// <returns>True si es posible serialitzar o deserialitzar l'objete. False en cas contrari.</returns>
        public override bool CanSerialize(Type type) {

            return typeof(IDictionary).IsAssignableFrom(type);
        }

        /// <summary>
        /// Serialitza un objecte.
        /// </summary>
        /// <param name="typeSerializer">Serialitzador de tipus.</param>
        /// <param name="writer">Escriptor de dades.</param>
        /// <param name="obj">L'objecte a serialitzar.</param>
        protected override void SerializeObject(SerializationContext context, object obj) {

            IDictionary dict = obj as IDictionary;

            context.Write("$count", dict.Count);
            foreach (DictionaryEntry entry in dict) {
                context.Write(null, entry.Key);
                context.Write(null, entry.Value);
            }
        }

        /// <summary>
        /// Deserialitza un objecte.
        /// </summary>
        /// <param name="typeSerializer">Serialitzador de tipus.</param>
        /// <param name="reader">Lector de dades.</param>
        /// <param name="obj">El objecte a deserialitzar.</param>
        /// <param name="version">Numero de versio.</param>
        protected override void DeserializeObject(DeserializationContext context, object obj, int version) {

            IDictionary dict = obj as IDictionary;
            Type keyType = dict.GetType().GetGenericArguments()[0];
            Type valueType = dict.GetType().GetGenericArguments()[1];

            int count;
            context.Read("$count", out count);

            while (count-- > 0) {
                object key, value;
                context.Read(null, out key, keyType);
                context.Read(null, out value, valueType);
                dict.Add(key, value);
            }
        }
    }
}
