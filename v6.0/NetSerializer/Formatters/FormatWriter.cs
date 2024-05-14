namespace NetSerializer.V6.Formatters {

    public abstract class FormatWriter: IDisposable {

        /// <summary>
        /// Finalitza l'operacio d'escriptura.
        /// </summary>
        /// 
        public abstract void Close();

        /// <summary>
        /// Escriu un valor bool
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteBool(string name, bool value);

        /// <summary>
        /// Escriu un valor int
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteInt(string name, int value);

        /// <summary>
        /// Escriu un valor float
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteSingle(string name, float value);

        /// <summary>
        /// Escriu un valor double
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteDouble(string name, double value);

        /// <summary>
        /// Escriu un valor decimal
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteDecimal(string name, decimal value);

        /// <summary>
        /// Escriu un valor string
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteString(string name, string? value);

        /// <summary>
        /// Escriu un valor enum
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="value">El valor.</param>
        /// 
        public abstract void WriteEnum(string name, Enum value);

        /// <summary>
        /// Escriu un valor null
        /// </summary>
        /// <param name="name">El nom.</param>
        /// 
        public abstract void WriteNull(string name);

        /// <summary>
        /// Escriu una referencia
        /// </summary>
        /// <param name="name">El num.</param>
        /// <param name="id">El identificador.</param>
        /// 
        public abstract void WriteObjectReference(string name, int id);

        /// <summary>
        /// Escriu la capcelera d'un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El tipus.</param>
        /// <param name="id">El identificador.</param>
        /// 
        public abstract void WriteObjectHeader(string name, Type type, int id);

        /// <summary>
        /// Escriu el peu d'un objecte.
        /// </summary>
        /// 
        public abstract void WriteObjectTail();

        /// <summary>
        /// Escriu la capcelera d'un struct.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// 
        public abstract void WriteStructHeader(string name);

        /// <summary>
        /// Escriu el peu d'un struct.
        /// </summary>
        /// 
        public abstract void WriteStructTail();

        /// <summary>
        /// Escriu la capcelera d'un array.
        /// </summary>
        /// <param name="name">El nom.</param
        /// <param name="bound">Nombre de dimensions</param>
        /// <param name="count">Nombre d'elements.</param>
        /// 
        public abstract void WriteArrayHeader(string name, int[] bound, int count);

        /// <summary>
        /// Escriu el peu d'un array.
        /// </summary>
        /// 
        public abstract void WriteArrayTail();

        /// <summary>
        /// Disposa l'objecte.
        /// </summary>
        /// 
        public abstract void Dispose();
    }
}