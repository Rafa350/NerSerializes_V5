namespace MikroPic.NetSerializer.v4 {

    using System;
    using MikroPic.NetSerializer.v4.Storage;
    using MikroPic.NetSerializer.v4.TypeSerializers;

    /// <summary>
    /// Serializador.
    /// </summary>
    public sealed class Serializer {

        private readonly StorageWriter writer;
        private readonly TypeSerializer typeSerializer = new TypeSerializer();
        private readonly int version;

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        /// <param name="writer">Escriptor de dades.</param>
        /// <param name="version">Numero de versio.</param>
        /// <seealso cref="StorageWriter"/>
        public Serializer(StorageWriter writer, int version) {

            if (writer == null)
                throw new ArgumentNullException("writer");

            this.writer = writer;
            this.version = version;
        }

        /// <summary>
        /// Tanca el serializador.
        /// </summary>
        public void Close() {

            writer.Close();
        }

        /// <summary>
        /// Afegeix un serialitzador de tipus.
        /// </summary>
        /// <param name="serializer">El serialitzador a afeigir.</param>
        public void AddSerializer(ITypeSerializer serializer) {

            typeSerializer.AddSerializer(serializer);
        }

        /// <summary>
        /// Serialitza un objecte.
        /// </summary>
        /// <param name="obj">El objecte a serialitzar.</param>
        /// <param name="name">Identificador del objecte.</param>
        public void Serialize(object obj, string name) {

            writer.Initialize(version);
            typeSerializer.Serialize(writer, name, obj.GetType(), obj);
        }
    }
}
