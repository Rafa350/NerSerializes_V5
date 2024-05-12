using NetSerializer.V6.Formatters;

namespace NetSerializer.V6 {

    public sealed class Deserializer {
        
        private readonly DeserializationContext _context;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reader">El lector de dades.</param>
        /// 
        public Deserializer(FormatReader reader) {

            _context = new DeserializationContext(reader);
        }
        
        /// <summary>
        /// Deserialitza un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <returns>L'objecte.</returns>
        /// 
        public T? Deserialize<T>(string name) {

            return _context.ReadObject<T>(name);
        }
        
        /// <summary>
        /// El context de deserialitzacio.
        /// </summary>
        /// 
        public IDeserializationReader Context =>
            _context;
    }
}
