namespace NetSerializer.V6.Formatters {

    public enum ObjectHeaderType {
        Null,
        Reference,
        Object
    }

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

        public abstract object ReadEnum(string name, Type type);

        public abstract string? ReadString(string name);

        public abstract ObjectHeaderType ReadObjectHeader(string name, out int id, out Type type);

        public abstract void ReadObjectTail();

        public abstract void Dispose();
    }
}
