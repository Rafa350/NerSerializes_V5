namespace NetSerializer.v4 {

    using NetSerializer.v4.Storage;
    using NetSerializer.v4.TypeSerializers;
    using System;

    /// <summary>
    /// Contexto de deserialitzacion.
    /// </summary>
    public sealed class DeserializationContext {

        private readonly TypeSerializer typeSerializer;
        private readonly StorageReader reader;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">Lector de dades.</param>
        /// <param name="typeSerializer">Serialitzador de tipus.</param>
        internal DeserializationContext(StorageReader reader, TypeSerializer typeSerializer) {

            if (reader == null)
                throw new ArgumentNullException("reader");

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            this.typeSerializer = typeSerializer;
            this.reader = reader;
        }

        /// <summary>
        /// Deserialitza un valor 'bool'.
        /// </summary>
        /// <param name="name">Nom que identificador del valor.</param>
        /// <param name="value">El valor deserialitzat.</param>
        /// <returns></returns>
        public DeserializationContext Read(string name, out bool value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(bool), out obj);
            value = (bool) obj;

            return this;
        }

        /// <summary>
        /// Deserialitza un valor 'byte'.
        /// </summary>
        /// <param name="name">Nom que identifica el valor.</param>
        /// <param name="value">El valor deserialitzat.</param>
        /// <returns></returns>
        public DeserializationContext Read(string name, out byte value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(byte), out obj);
            value = (byte) obj;

            return this;
        }

        /// <summary>
        /// Deserializa un valor 'sbyte'.
        /// </summary>
        /// <param name="name">Nom que identifica el valor.</param>
        /// <param name="value">El valor deserializat.</param>
        /// <returns></returns>
        public DeserializationContext Read(string name, out sbyte value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(sbyte), out obj);
            value = (sbyte) obj;

            return this;
        }

        public DeserializationContext Read(string name, out short value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(short), out obj);
            value = (short) obj;

            return this;
        }

        public DeserializationContext Read(string name, out int value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(int), out obj);
            value = (int) obj;

            return this;
        }
        
        public DeserializationContext Read(string name, out long value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(long), out obj);
            value = (long) obj;

            return this;
        }

        public DeserializationContext Read(string name, out ushort value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(ushort), out obj);
            value = (ushort) obj;

            return this;
        }

        public DeserializationContext Read(string name, out uint value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(uint), out obj);
            value = (uint) obj;

            return this;
        }

        public DeserializationContext Read(string name, out ulong value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(ulong), out obj);
            value = (ulong) obj;

            return this;
        }

        public DeserializationContext Read(string name, out float value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(float), out obj);
            value = (float) obj;

            return this;
        }

        public DeserializationContext Read(string name, out double value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(double), out obj);
            value = (double) obj;

            return this;
        }

        public DeserializationContext Read(string name, out string value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(string), out obj);
            value = (string) obj;

            return this;
        }

        public DeserializationContext Read(string name, out char value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(char), out obj);
            value = (char) obj;

            return this;
        }

        public DeserializationContext Read(string name, out DateTime value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(DateTime), out obj);
            value = (DateTime) obj;

            return this;
        }
        
        public DeserializationContext Read(string name, out TimeSpan value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(TimeSpan), out obj);
            value = (TimeSpan) obj;

            return this;
        }

        public DeserializationContext Read(string name, out object value) {

            typeSerializer.Deserialize(reader, name, typeof(object), out value);

            return this;
        }

        public DeserializationContext Read(string name, out object value, Type type) {

            typeSerializer.Deserialize(reader, name, type, out value);

            return this;
        }

        public DeserializationContext Read<T>(string name, out T value) {

            object obj;
            typeSerializer.Deserialize(reader, name, typeof(T), out obj);
            value = (T) obj;

            return this;
        }

        /// <summary>
        /// Obte el numero de versio del fitxer.
        /// </summary>
        public int Version {
            get {
                return reader.Version;
            }
        }
    }
}
