namespace NetSerializer.V6.Formatters {

    public enum ObjectHeaderType {
        Null,
        Reference,
        Object
    }

    public enum ArrayHeaderType {
        Null,
        Array
    }

    public abstract class FormatReader: IDisposable {

        /// <summary>
        /// Finalitza l'operacio de lectura.
        /// </summary>
        /// 
        public abstract void Close();

        /// <summary>
        /// Comprova si pot lleigir el tipus d'objecte especificat.
        /// </summary>
        /// <param name="type">El tipus.</param>
        /// <returns>True si es posible, false en cas contrari.</returns>
        /// 
        public abstract bool CanReadValue(Type type);

        /// <summary>
        /// Llegeix un objecte del tipus especificat.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El tipus.</param>
        /// <returns>El resultat.</returns>
        /// 
        public abstract object? ReadValue(string name, Type type);

        /// <summary>
        /// Llegeix un valor boolean.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract bool ReadBool(string name);

        /// <summary>
        /// Llegeix un valor int.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract int ReadInt(string name);

        /// <summary>
        /// Llegeix un valor float.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract float ReadSingle(string name);

        /// <summary>
        /// Llegeix un valor double.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract double ReadDouble(string name);

        /// <summary>
        /// Llegeix un valor decimal.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract decimal ReadDecimal(string name);

        public abstract T ReadEnum<T>(string name) where T : struct;

        public abstract object ReadEnum(string name, Type type);

        /// <summary>
        /// Llegeix un valor char.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract char ReadChar(string name);

        /// <summary>
        /// Llegeix un valor string.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El resultat de la lectura.</returns>
        /// 
        public abstract string? ReadString(string name);

        public abstract ObjectHeaderType ReadObjectHeader(string name, out int id, out Type type);

        public abstract void ReadObjectTail();

        public abstract void ReadStructHeader(string name);

        public abstract void ReadStructTail();

        public abstract ArrayHeaderType ReadArrayHeader(string name, out int[] bound, out int count);

        public abstract void ReadArrayTail();

        public abstract void Dispose();

        /// <summary>
        /// La versio de les dades.
        /// </summary>
        /// 
        public abstract int Version { get; }
    }
}
