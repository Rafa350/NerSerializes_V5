namespace MikroPic.NetSerializer.v4 {

    using System;
    using MikroPic.NetSerializer.v4.Storage;
    using MikroPic.NetSerializer.v4.TypeSerializers;

    /// <summary>
    /// Deserializacion de objetos.
    /// </summary>
    public sealed class Deserializer {

        private readonly TypeSerializer typeSerializer = new TypeSerializer();
        private readonly StorageReader reader;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="reader">Objeto StorageReader. Si es nulo dispara una excepcion.</param>
        /// <exception cref="ArgumentNullException">Elgun argumento es nulo.</exception>
        public Deserializer(StorageReader reader) {

            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
        }

        /// <summary>
        /// Cierra el deserializador.
        /// </summary>
        public void Close() {

            reader.Close();
        }

        public void AddSerializer(ITypeSerializer serializer) {

            typeSerializer.AddSerializer(serializer);
        }

        /// <summary>
        /// Deserializa un objeto
        /// </summary>
        /// <param name="type">Tipo del objeto a deserializar. Si es nulo dispara una excepcion.</param>
        /// <param name="name">Nombre del nodo raiz.</param>
        /// <returns>El objeto deserializado.</returns>
        /// <exception cref="ArgumentNullException">Algun argumento es nulo.</exception>
        public object Deserialize(Type type, string name) {

            object obj;

            reader.Initialize();
            typeSerializer.Deserialize(reader, name, type, out obj);

            return obj;
        }

        /// <summary>
        /// Deserializa un objeto (Version generica).
        /// </summary>
        /// <param name="name">Nombre del nodo raiz.</param>
        /// <returns>El objeto deserializado.</returns>
        /// <exception cref="ArgumentNullException">Algun argumento es nulo.</exception>
        public T Deserialize<T>(string name) {

            return (T) Deserialize(typeof(T), name);
        }
    }
}
