using System.Text;

namespace NetSerializer.V6.TypeSerializers.Serializers {

    /// <summary>
    /// Serialitzador d'arrays.
    /// </summary>
    /// 
    public sealed class ArraySerializer: TypeSerializer {

        /// <inheritdoc/>
        /// 
        public override bool CanProcess(Type type) =>
            type.IsArray;

        /// <inheritdoc/>
        /// 
        public override void Serialize(SerializationContext context, string name, object obj) {

            var array = (Array)obj;

            if (array.Length == 0)
                throw new InvalidOperationException("No se puede serializar un array vacio, ha de serializarse como 'null'.");

            var type = array.GetType();
            var elementType = type.GetElementType();
            if (elementType == null)
                throw new InvalidOperationException("No se puede obtener el tipo de elemento del array.");

            var index = new MultidimensionalIndex(array);
            do {
                var elementValue = array.GetValue(index.Current);
                var elementName = String.Format("{0}[{1}]", name, index);

                context.Write(elementName, elementValue);

            } while (index.Next());
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, string name, object obj) {

            var array = (Array)obj;
            var type = array.GetType();
            var elementType = type.GetElementType();
            if (elementType == null)
                throw new InvalidOperationException("No se puede obtener el tipo de elemento del array.");

            var index = new MultidimensionalIndex(array);
            do {

                var elementName = String.Format("{0}[{1}]", name, index);

                array.SetValue(context.Read(elementName, elementType), index.Current);

            } while (index.Next());
        }

        /// <summary>
        /// Clase per la gestio dels index dels arrays
        /// </summary>
        /// 
        private sealed class MultidimensionalIndex {

            private readonly int _dimensions;
            private readonly int[] _bounds;
            private readonly int[] _index;

            public MultidimensionalIndex(Array array) {

                _dimensions = array.Rank;

                _bounds = new int[_dimensions];
                for (int i = 0; i < _dimensions; i++)
                    _bounds[i] = array.GetUpperBound(i);

                _index = new int[_dimensions];
                for (int i = 0; i < _dimensions; i++)
                    _index[i] = 0;
            }

            public override string ToString() {

                var sb = new StringBuilder();

                bool first = true;
                for (int i = 0; i < _dimensions; i++) {
                    if (first)
                        first = false;
                    else
                        sb.Append(',');
                    sb.Append(_index[i]);
                }

                return sb.ToString();
            }

            public bool Next() {

                for (int i = _dimensions - 1; i >= 0; i--) {
                    _index[i] += 1;
                    if (_index[i] <= _bounds[i])
                        return true;
                    else
                        _index[i] = 0;
                }

                return false;
            }

            public int[] Current =>
                _index;
        }
    }
}
