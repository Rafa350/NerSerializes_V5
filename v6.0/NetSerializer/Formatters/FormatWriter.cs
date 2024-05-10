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

        public abstract void WriteNull(string name);

        public abstract void WriteObjectNull(string name);

        public abstract void WriteObjectReference(string name, int id);

        public abstract void WriteObjectHeader(string name, Type type, int id);

        public abstract void WriteObjectTail();

        /// <summary>
        /// Disposa l'objecte.
        /// </summary>
        /// 
        public abstract void Dispose();
    }
}