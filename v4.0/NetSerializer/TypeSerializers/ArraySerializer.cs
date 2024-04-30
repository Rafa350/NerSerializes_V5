namespace NetSerializer.v4.TypeSerializers {

    using System;
    using NetSerializer.v4.Infrastructure;
    using NetSerializer.v4.Storage;

    /// <summary>
    /// Serialitzador d'arrays CLI.
    /// </summary>
    public sealed class ArraySerializer: SerializerBase {

        /// <summary>
        /// Comprova si pot serialitzar un tipus.
        /// </summary>
        /// <param name="type">El tipus a comprovar.</param>
        /// <returns>True si es posible serialitzar o deserialitzar el tipus especificat.</returns>
        public override bool CanSerialize(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            return type.IsArray;
        }

        /// <summary>
        /// Serialitza l'array.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="writer">Objeto StorageWriter. Si es nulo, dispara una excepcion.</param>
        /// <param name="name">El nomb del node.</param>
        /// <param name="type">El tipus d'objete a serializar.</param>
        /// <param name="obj">L'array a serialitzar.</param>
        /// <exception cref="InvalidOperationException">No es positle serialitzar l'objecte.</exception>
        /// <exception cref="ArgumentnullException">Algun argument es nul.</exception>
        public override void Serialize(TypeSerializer typeSerializer, StorageWriter writer, string name, Type type, object obj) {

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            if (writer == null)
                throw new ArgumentNullException("writer");

            if (type == null)
                throw new ArgumentNullException("type");

            if (!CanSerialize(type))
                throw new InvalidOperationException(
                    String.Format("No es posible serializar el tipo '{0}'.", type.ToString()));

            if (obj == null)
                writer.WriteNull(name);

            else {
                Array array = obj as Array;
                if (array == null)
                    throw new InvalidOperationException("'obj' no es 'Array'.");

                int[] bound = new int[array.Rank];
                for (int i = 0; i < bound.Length; i++)
                    bound[i] = array.GetUpperBound(i) + 1;
                Type elementType = type.GetElementType();
                int count = array.Length;

                writer.WriteArrayStart(name, type, bound, count);

                MultidimensionalIndex index = new MultidimensionalIndex(array);
                do {
                    object value = array.GetValue(index.Current);
                    typeSerializer.Serialize(writer, null, elementType, value);
                } while (index.Next());

                writer.WriteArrayEnd();
            }
        }

        /// <summary>
        /// Deserializa un array.
        /// </summary>
        /// <param name="typeSerializer">El serialitzador de tipus.</param>
        /// <param name="reader">Objeto StorageReader.</param>
        /// <param name="name">El nom del node.</param>
        /// <param name="type">El tipus d'objecte a deserialitzar.</param>
        /// <param name="obj">L'objete deserialitzat.</param>
        public override void Deserialize(TypeSerializer typeSerializer, StorageReader reader, string name, Type type, out object obj) {

            if (typeSerializer == null)
                throw new ArgumentNullException("typeSerializer");

            if (reader == null)
                throw new ArgumentNullException("reader");

            if (type == null)
                throw new ArgumentNullException("type");

            if (!CanSerialize(type))
                throw new InvalidOperationException(
                    String.Format("No es posible deserializar el tipo '{0}'.", type.ToString()));

            int count;
            int[] bound;
            reader.ReadArrayStart(name, out count, out bound);
            
            if (bound == null)
                obj = null;

            else {
                Array array = Array.CreateInstance(type.GetElementType(), bound);

                MultidimensionalIndex index = new MultidimensionalIndex(array);
                for (int i = 0; i < count; i++) {
                    object elementValue;
                    typeSerializer.Deserialize(reader, null, type.GetElementType(), out elementValue);
                    array.SetValue(elementValue, index.Current);
                    index.Next();
                }

                reader.ReadArrayEnd();

                obj = array;
            }
        }
    }
}
