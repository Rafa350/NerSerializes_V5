namespace NetSerializer.V6.TypeSerializers {

    /// <summary>
    /// Extensions de la clase 'Type'.
    /// </summary>
    internal static class TypeExtensions {

        /// <summary>
        /// Identifica tipus especials, son valors pero s'implementan com clases.
        /// </summary>
        /// <param name="type">This</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        public static bool IsSpecialType(this Type type) =>
            (type == typeof(string)) ||
            (type == typeof(DateTime)) ||
            (type == typeof(TimeSpan)) ||
            (type == typeof(Guid)) ||
            (type == typeof(decimal));

        /// <summary>
        /// Identifica els tipus class.
        /// </summary>
        /// <param name="type">This</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        public static bool IsClassType(this Type type) =>
            type.IsClass && !type.IsArray && !type.IsSpecialType();


        /// <summary>
        /// Identifica els tipus struct.
        /// </summary>
        /// <param name="type">This</param>
        /// <returns>True en cas afirmatiu.</returns>
        /// 
        public static bool IsStructType(this Type type) =>
            type.IsValueType && !type.IsPrimitive && !type.IsEnum;
    }
}
