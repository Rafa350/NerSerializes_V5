namespace MikroPic.NetSerializer.v4.Infrastructure {

    using System;
    using System.Text;

    internal sealed class MultidimensionalIndex {

        private readonly int dimensions;
        private readonly int[] bounds;
        private readonly int[] index;

        public MultidimensionalIndex(Array array) {

            if (array == null)
                throw new ArgumentNullException("array");

            dimensions = array.Rank;

            bounds = new int[dimensions];
            for (int i = 0; i < dimensions; i++)
                bounds[i] = array.GetUpperBound(i);
            
            index = new int[dimensions];
            for (int i = 0; i < dimensions; i++)
                index[i] = 0;
        }

        public override string ToString() {

            StringBuilder sb = new StringBuilder();

            bool first = true;
            for (int i = 0; i < dimensions; i++) {
                if (first)
                    first = false;
                else
                    sb.Append(',');
                sb.Append(index[i]);
            }

            return sb.ToString();
        }

        public bool Next() {

            for (int i = dimensions - 1; i >= 0; i--) {

                index[i] += 1;
                if (index[i] <= bounds[i])
                    return true;
                else
                    index[i] = 0;
            }

            return false;
        }

        public int[] Current {
            get {
                return index;
            }
        }
    }
}
