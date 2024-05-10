namespace NetSerializer.V6.Formatters {

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

        public abstract decimal ReadDecimal(string name);

        public abstract T ReadEnum<T>(string name) where T: struct;

        public abstract string? ReadString(string name);

        public abstract object? ReadObject(string name);

        public abstract void Dispose();
    }
}
