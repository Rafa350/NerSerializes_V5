namespace NetSerializer.V6 {

    public interface ISerializationWriter {

        /// <summary>
        /// Escriu un valor boolean.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteBool(string name, bool value);

        /// <summary>
        /// Escriu un valor int.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteInt(string name, int value);

        /// <summary>
        /// Escriu un valor float.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteSingle(string name, float value);

        /// <summary>
        /// Escriu un valor double.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteDouble(string name, double value);

        /// <summary>
        /// Escriu un valor decimal.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteDecimal(string name, decimal value);

        /// <summary>
        /// Escriu un valor string.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteString(string name, string? value);

        /// <summary>
        /// Escriu un valor enum.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteEnum(string name, Enum value);

        /// <summary>
        /// Escriu un valor nul.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// 
        void WriteNull(string name);

        /// <summary>
        /// Escriu un objecte.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteObject(string name, object? obj);

        /// <summary>
        /// Escriu un struct.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteStruct(string name, object value);

        /// <summary>
        /// Escriu un array.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteArray(string name, Array value);
    }
}