namespace NetSerializer.V6 {

    public interface IDeserializationReader {

        /// <summary>
        /// Llegeix un valor del tipus especificat..
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El tipus.</param>
        /// <returns>EL valor.</returns>
        /// 
        object? Read(string name, Type type);

        /// <summary>
        /// Llegeix un valor bool.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        bool ReadValueBool(string name);

        /// <summary>
        /// Llegeix un valor int.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        int ReadValueInt(string name);

        /// <summary>
        /// Llegeix un valor float.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        float ReadValueSingle(string name);

        /// <summary>
        /// Llegeix un valor double.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        double ReadValueDouble(string name);

        /// <summary>
        /// Llegeix un valor decimal.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        decimal ReadValueDecimal(string name);

        /// <summary>
        /// Llegeix un valor enum.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        T ReadValueEnum<T>(string name) where T : struct;

        /// <summary>
        /// Llegeix un valor enum.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El tipus del enumerador</param>
        /// <returns>El valor.</returns>
        /// 
        object ReadValueEnum(string name, Type type);

        /// <summary>
        /// Llegeix un valor char
        /// </summary>
        /// <param name="name"></param>
        /// <returns>El valor.</returns>
        /// 
        char ReadValueChar(string name);

        /// <summary>
        /// Llegeix un valor string.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        string? ReadValueString(string name);

        /// <summary>
        /// Llegeix un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>L'objecte.</returns>
        /// 
        T? ReadObject<T>(string name);

        /// <summary>
        /// Llegeix un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El Tipus del objecte.</param>
        /// <returns>L'objecte.</returns>
        /// 
        object? ReadObject(string name, Type type);

        object ReadStruct(string name, Type type);

        Array? ReadArray(string name, Type type);

        /// <summary>
        /// La versio del contingut per deserialitzar.
        /// </summary>
        /// 
        int Version { get; }
    }

}