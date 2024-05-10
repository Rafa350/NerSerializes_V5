using System;
using System.Diagnostics;
using System.Text;
using NetSerializer.V6.Formatters;
/*
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
        public override void Serialize(SerializationContext context, string name, Type type, object? obj) {

            Debug.Assert(CanProcess(type));


            if (obj == null)
                writer.WriteNull(name);

            else {
                Debug.Assert(obj is Array);
                var array = (Array)obj;

                writer.WriteArrayHeader(name, array);

                var index = new MultidimensionalIndex(array);
                do {

                    var value = array.GetValue(index.Current);
                    var elementType = type.GetElementType();
                    if (elementType == null)
                        throw new InvalidOperationException("No se puede obtener el tipo de elemento del array.");
                    var elementName = String.Format("{0}[{1}]", name, index);

                    var typeSerializer = context.GetTypeSerializer(elementType);
                    Debug.Assert(typeSerializer != null);

                    typeSerializer.Serialize(context, elementName, elementType, value);

                } while (index.Next());

                writer.WriteArrayTail();
            }
        }

        /// <inheritdoc/>
        /// 
        public override void Deserialize(DeserializationContext context, string name, Type type, out object? obj) {

            Debug.Assert(CanProcess(type));

            var reader = context.Reader;

            ReadArrayResult result = reader.ReadArrayHeader(name);
            if (result.ResultType == ReadArrayResultType.Null)
                obj = null;

            else {
                var elementType = type.GetElementType();
                if (elementType == null)
                    throw new InvalidOperationException("No se puede obtener el tipo de elemento del array.");

                var array = Array.CreateInstance(elementType, result.Bounds);

                var index = new MultidimensionalIndex(array);
                for (int i = 0; i < result.Count; i++) {

                    var elementName = String.Format("{0}[{1}]", name, index);

                    var typeSerializer = context.GetTypeSerializer(elementType);
                    Debug.Assert(typeSerializer != null);

                    typeSerializer.Deserialize(context, elementName, elementType, out object? elementValue);

                    array.SetValue(elementValue, index.Current);

                    index.Next();
                }

                reader.ReadArrayTail();

                obj = array;
            }

        }

        /// <summary>
        /// Clase per la gestio dels index dels arrays
        /// </summary>
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
*/