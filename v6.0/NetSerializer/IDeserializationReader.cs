namespace NetSerializer.V6 {

    public interface IDeserializationReader {
        
        /// <summary>
        /// Llegeix un valor bool.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        bool ReadBool(string name);

        /// <summary>
        /// Llegeix un valor int.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        int ReadInt(string name);

        /// <summary>
        /// Llegeix un valor float.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        float ReadSingle(string name);

        /// <summary>
        /// Llegeix un valor double.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        double ReadDouble(string name);

        /// <summary>
        /// Llegeix un valor decimal.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        decimal ReadDecimal(string name);

        /// <summary>
        /// Llegeix un valor enum.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        T ReadEnum<T>(string name) where T: struct;

        /// <summary>
        /// Llegeix un valor enum.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="type">El tipus del enumerador</param>
        /// <returns>El valor.</returns>
        /// 
        object ReadEnum(string name, Type type);

        /// <summary>
        /// Llegeix un valor string.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>El valor.</returns>
        /// 
        string? ReadString(string name);

        /// <summary>
        /// Llegeix un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>L'objecte.</returns>
        /// 
        object? ReadObject(string name);
    }

}