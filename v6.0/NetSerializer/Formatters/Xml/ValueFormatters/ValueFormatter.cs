using System.Xml;

namespace NetSerializer.V6.Formatters.Xml.ValueFormatters {

    public abstract class ValueFormatter {

        /// <summary>
        /// Comprova si pot formatejar el tipus especificat.
        /// </summary>
        /// <param name="type">El tipus.</param>
        /// <returns>True si es posible, false en cas contrari.</returns>
        /// 
        public abstract bool CanFormat(Type type);

        /// <summary>
        /// Escriu el valor.
        /// </summary>
        /// <param name="obj">El valor.</param>
        /// 
        public abstract void Write(XmlWriter writer, object obj);

        /// <summary>
        /// Llegeix un valor.
        /// </summary>
        /// <returns>El valor lleigit..</returns>
        /// 
        public abstract object Read(XmlReader reader);
    }
}
