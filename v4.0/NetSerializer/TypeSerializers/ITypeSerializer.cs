namespace NetSerializer.v4.TypeSerializers {

    using System;
    using NetSerializer.v4.Storage;

    /// <summary>
    /// Interface que deben implementar todos los serializadores de tipos.
    /// </summary>
    public interface ITypeSerializer {

        /// <summary>
        /// Indica si es posible serializar un tipo determinado.
        /// </summary>
        /// <param name="type">El tipo a serializar o deserializar.</param>
        /// <returns>True si es posible serializar o deserializar. False en caso contrario</returns>
        bool CanSerialize(Type type);

        /// <summary>
        /// Serializa un objeto.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="writer">El objeto StorageWriter, que realizara la escritura del objeto serializado.</param>
        /// <param name="name">Nombre del nodo.</param>
        /// <param name="type">Tipo del objeto a serializar.</param>
        /// <param name="obj">El objeto a serializar.</param>
        void Serialize(TypeSerializer typeSerializer, StorageWriter writer, string name, Type type, object obj);

        /// <summary>
        /// Deserializa un objeto.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="reader">El objeto StorageReader, que realizara la lecture del objeto serializado.</param>
        /// <param name="name">Nombre del nodo.</param>
        /// <param name="type">Tipo del objeto a deserializar.</param>
        /// <param name="obj">El objeto deserializado.</param>
        void Deserialize(TypeSerializer typeSerializer, StorageReader reader, string name, Type type, out object obj);
    }
}
