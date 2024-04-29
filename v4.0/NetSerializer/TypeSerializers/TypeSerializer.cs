namespace MikroPic.NetSerializer.v4.TypeSerializers {

    using System;
    using System.Collections.Generic;
    using MikroPic.NetSerializer.v4.Storage;

    /// <summary>
    /// Serializador de tipos. Serializa un objeto mediante la lista de 
    /// serializadores registrados. La clase busca entre los serializadores
    /// registrados, el que puede serializar un tipo dado. Una vez encontrado
    /// lo mantiene en una cache para su rapida localizacion.
    /// </summary>
    public sealed class TypeSerializer {

        //private static TypeSerializer instance;

        private readonly List<ITypeSerializer> serializerList = new List<ITypeSerializer>();
        private readonly Dictionary<Type, ITypeSerializer> serializerCache = new Dictionary<Type, ITypeSerializer>();

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        public TypeSerializer() {

            AddDefaultSerializers();
        }

        /// <summary>
        /// Serializa un objeto.
        /// </summary>
        /// <param name="writer">El objecte per escriure en el magatzem de dades.</param>
        /// <param name="name">El nombre del nodo.</param>
        /// <param name="type">El tipo del objeto</param>
        /// <param name="obj">El objeto a serializar.</param>
        public void Serialize(StorageWriter writer, string name, Type type, object obj) {

            if (writer == null)
                throw new ArgumentNullException("writer");

            if (type == null)
                throw new ArgumentNullException("type");

            ITypeSerializer serializer = GetSerializer(type);
            serializer.Serialize(this, writer, name, type, obj);
        }

        /// <summary>
        /// Deserializa un objeto.
        /// </summary>
        /// <param name="reader">El objecte lper lleigir del magatzem de dades.</param>
        /// <param name="name">El nombre del nodo.</param>
        /// <param name="type">El tipo del objeto.</param>
        /// <param name="obj">El objeto deserializado.</param>
        public void Deserialize(StorageReader reader, string name, Type type, out object obj) {

            if (reader == null)
                throw new ArgumentNullException("reader");

            if (type == null)
                throw new ArgumentNullException("type");

            ITypeSerializer serializer = GetSerializer(type);
            serializer.Deserialize(this, reader, name, type, out obj);
        }

        /// <summary>
        /// Registra los serializadores de tipos basicos.
        /// </summary>
        private void AddDefaultSerializers() {

            // Es important l'ordre en que es registren els serialitzadors
            //
            serializerList.Add(new ValueSerializer());    // Sempre el primer
            serializerList.Add(new ArraySerializer());
            serializerList.Add(new StructSerializer());
            serializerList.Add(new ListSerializer());
            serializerList.Add(new DictionarySerializer());
            serializerList.Add(new ClassSerializer());    // Sempre l'ultim
        }

        /// <summary>
        /// Registra un serializador de tipos.
        /// </summary>
        /// <param name="serializer">El serializador a registrar. Si es nulo dispara una excepcion.</param>
        public void AddSerializer(ITypeSerializer serializer) {

            if (serializer == null)
                throw new ArgumentNullException("serializer");

            if (serializerList.Contains(serializer))
                throw new InvalidOperationException("Ya se registró este serializador.");

            // S'insereixen abans dels serialitzadors estandard
            //
            serializerList.Insert(0, serializer);
        }

        /// <summary>
        /// Obtiene el serializador para un tipo de objeto determinado.
        /// </summary>
        /// <param name="type">El tipo de objeto.</param>
        /// <returns>El serializador correspondiente.</returns>
        /// <exception cref="InvalidOperationException">Se produce si no hay ningun 
        /// serializaor registrado para el tipo de objeto, o si no es posible 
        /// instanciar el serializador.</exception>
        private ITypeSerializer GetSerializer(Type type) {

            ITypeSerializer serializer;

            if (!serializerCache.TryGetValue(type, out serializer)) {
                foreach (ITypeSerializer s in serializerList) {
                    if (s.CanSerialize(type)) {
                        serializer = s;
                        serializerCache.Add(type, s);
                        break;
                    }
                }
            }

            if (serializer == null)
                throw new InvalidOperationException(
                    String.Format(
                        "No se registró el serializador para el tipo '{0}'.",
                        type));

            return serializer;
        }

        /// <summary>
        /// Retorna una instancia unica del objecte.
        /// </summary>
        /*public static TypeSerializer Instance {
            get {
                if (instance == null)
                    instance = new TypeSerializer();
                return instance;
            }
        }*/
    }
}
