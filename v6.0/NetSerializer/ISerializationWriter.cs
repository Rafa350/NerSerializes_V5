using System;

namespace NetSerializer.V6.Formaters {
    
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
        void WriteFloat(string name, float value);

        /// <summary>
        /// Escriu un valor double.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteDouble(string name, double value);

        /// <summary>
        /// Escriu un objecte.
        /// </summary>
        /// <param name="name">El nom del valor.</param>
        /// <param name="value">El valor.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// 
        void WriteObject(string name, object obj);
    }
}