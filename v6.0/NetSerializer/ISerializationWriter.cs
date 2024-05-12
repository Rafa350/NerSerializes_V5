namespace NetSerializer.V6 {

    public interface ISerializationWriter {

        /// <summary>
        /// Escriu un valor boolean.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteBool(string name, bool value);

        /// <summary>
        /// Escriu un valor int.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteInt(string name, int value);

        /// <summary>
        /// Escriu un valor float.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteSingle(string name, float value);

        /// <summary>
        /// Escriu un valor double.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteDouble(string name, double value);

        /// <summary>
        /// Escriu un valor decimal.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteDecimal(string name, decimal value);

        /// <summary>
        /// Escriu un valor string.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteString(string name, string? value);

        /// <summary>
        /// Escriu un valor enum.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteEnum(string name, Enum value);

        /// <summary>
        /// Escriu un valor nul.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteNull(string name);

        /// <summary>
        /// Escriu un objecte.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteObject(string name, object? obj);

        /// <summary>
        /// Escriu un struct.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteStruct<T>(string name, T value) where T: struct;
    }
}