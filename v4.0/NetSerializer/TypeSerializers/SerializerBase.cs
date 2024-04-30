namespace NetSerializer.v4.TypeSerializers {

    using System;
    using NetSerializer.v4.Storage;
    
    /// <summary>
    /// Clase abstracta de la que derivan todos los serializadores de tipos.
    /// </summary>
    public abstract class SerializerBase: ITypeSerializer {

        /// <summary>
        /// Indica si es posible serializar un tipo determinado. Es una metodo
        /// abstracto y debe sobreescribirse en la clase derivada.
        /// </summary>
        /// <param name="type">El tipo a serializar o deserializar.</param>
        /// <returns>True si es posible serializar o deserializar. False en caso contrario</returns>
        public abstract bool CanSerialize(Type type);

        /// <summary>
        /// Serializa un objeto. Es un metodo abstracto y debe de sobreescribirse
        /// en la clase derivada
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="writer">El objeto StorageWriter, que realizara la 
        /// escritura del objeto serializado.</param>
        /// <param name="name">Nombre del nodo.</param>
        /// <param name="type">Tipo del objeto a serializar.</param>
        /// <param name="obj">El objeto a serializar.</param>
        public abstract void Serialize(TypeSerializer typeSerializer, StorageWriter writer, string name, Type type, object obj);

        /// <summary>
        /// Deserializa un objeto. Es un metodo abstracto y debe de sobreescribirse en 
        /// la clase derivada.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="reader">El objeto StorageReader, que realizara la lectura
        /// del objeto serializado.</param>
        /// <param name="name">Nombre del nodo.</param>
        /// <param name="type">Tipo del objeto a deserializar.</param>
        /// <param name="obj">El objeto deserializado.</param>
        public abstract void Deserialize(TypeSerializer typeSerializer, StorageReader reader, string name, Type type, out object obj);
    }
}
