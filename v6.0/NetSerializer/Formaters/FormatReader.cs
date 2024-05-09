namespace NetSerializer.V6.Formaters {

    public abstract class FormatReader: IDisposable {

        /// <summary>
        /// Finalitza l'operacio de lectura.
        /// </summary>
        /// 
        public abstract void Close();

        public abstract bool ReadBool(string name);

        public abstract int ReadInt(string name);

        public abstract float ReadSingle(string name);

        public abstract double ReadDouble(string name);

        public abstract object? ReadObject(string name);

        public abstract void Dispose();
    }
}
