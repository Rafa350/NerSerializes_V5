namespace NetSerializer.v4 {

    using System;
    using NetSerializer.v4.Storage;
    using NetSerializer.v4.TypeSerializers;

    /// <summary>
    /// Context de serialitzacio.
    /// </summary>
    public sealed class SerializationContext {

        private readonly StorageWriter writer;
        private readonly TypeSerializer typeSerializer;

        /// <summary>
        /// Constructor. Es intern per que no el pugui crear el client.
        /// </summary>
        /// <param name="writer">Escriptor de dades.</param>
        /// <param name="typeSerializer">Serialitzador de tipus.</param>
        internal SerializationContext(StorageWriter writer, TypeSerializer typeSerializer) {

            if (writer == null)
                throw new ArgumentNullException("writer");

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            this.writer = writer;
            this.typeSerializer = typeSerializer;
        }

        /// <summary>
        /// Serialitza un valor 'string'.
        /// </summary>
        /// <param name="name">El identificado del valor.</param>
        /// <param name="value">Valor a serializar.</param>
        /// <returns>El contexto.</returns>
        public SerializationContext Write(string name, string value) {

            typeSerializer.Serialize(writer, name, typeof(string), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'char'
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serializar.</param>
        /// <returns>El context.</returns>
        public SerializationContext Write(string name, char value) {

            typeSerializer.Serialize(writer, name, typeof(char), value);
            return this;
        }

        /// <summary>
        /// Excriu un valor 'bool'.
        /// </summary>
        /// <param name="name">El identificador del valor..</param>
        /// <param name="value">El valor a serializar.</param>
        /// <returns>El context.</returns>
        public SerializationContext Write(string name, bool value) {

            typeSerializer.Serialize(writer, name, typeof(bool), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'byte'.
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns>El context.</returns>
        public SerializationContext Write(string name, byte value) {

            typeSerializer.Serialize(writer, name, typeof(byte), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'sbyte'. 
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns>El context.</returns>
        public SerializationContext Write(string name, sbyte value) {

            typeSerializer.Serialize(writer, name, typeof(sbyte), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'short'
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, short value) {

            typeSerializer.Serialize(writer, name, typeof(short), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'int'
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, int value) {

            typeSerializer.Serialize(writer, name, typeof(int), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'long'.
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar</param>
        /// <returns></returns>
        public SerializationContext Write(string name, long value) {

            typeSerializer.Serialize(writer, name, typeof(long), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'float'.
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, float value) {

            typeSerializer.Serialize(writer, name, typeof(float), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'double'.
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, double value) {

            typeSerializer.Serialize(writer, name, typeof(double), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'DateTime'
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, DateTime value) {

            typeSerializer.Serialize(writer, name, typeof(DateTime), value);
            return this;
        }

        /// <summary>
        /// Serialitza un valor 'TimeSpan'
        /// </summary>
        /// <param name="name">El identificador del valor.</param>
        /// <param name="value">El valor a serialitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, TimeSpan value) {

            typeSerializer.Serialize(writer, name, typeof(TimeSpan), value);
            return this;
        }

        /// <summary>
        /// Serialitza un objecte.
        /// </summary>
        /// <param name="name">El identificador del objecte.</param>
        /// <param name="obj">El objecte a seriualitzar.</param>
        /// <returns></returns>
        public SerializationContext Write(string name, object obj) {

            Type type = obj == null ? typeof(object) : obj.GetType();
            typeSerializer.Serialize(writer, name, type, obj);
            return this;
        }
    }
}
