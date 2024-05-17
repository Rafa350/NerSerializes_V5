namespace NetSerializer.V6 {

    public interface ISerializationWriter {

        /// <summary>
        /// Escriu un valor.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void Write(string name, object? value);

        /// <summary>
        /// Escriu un valor boolean.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueBool(string name, bool value);

        /// <summary>
        /// Escriu un valor short.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueShort(string name, short value);

        /// <summary>
        /// Escriu un valor int.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueInt(string name, int value);

        /// <summary>
        /// Escriu un valor long.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueLong(string name, long value);

        /// <summary>
        /// Escriu un valor float.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueSingle(string name, float value);

        /// <summary>
        /// Escriu un valor double.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueDouble(string name, double value);

        /// <summary>
        /// Escriu un valor decimal.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueDecimal(string name, decimal value);

        /// <summary>
        /// Escriu un valor char.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueChar(string name, char value);

        /// <summary>
        /// Escriu un valor string.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueString(string name, string? value);

        /// <summary>
        /// Escriu un valor datetime.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueDateTime(string name, DateTime value);

        /// <summary>
        /// Escriu un valor timespan
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueTimeSpan(string name, TimeSpan value);

        /// <summary>
        /// Escriu un valor guid.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueGuid(string name, Guid value);

        /// <summary>
        /// Escriu un valor enum.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// 
        void WriteValueEnum(string name, Enum value);

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
        void WriteArray(string name, Array? value);
    }
}