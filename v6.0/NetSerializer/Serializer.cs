using NetSerializer.V6.Formatters;

namespace NetSerializer.V6 {

    public sealed class Serializer {

        private readonly SerializationContext _context;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="writer">El escriptor de dades.</param>
        /// 
        public Serializer(FormatWriter writer) {

            _context = new SerializationContext(writer);
        }

        /// <summary>
        /// Serialitza un objecte.
        /// </summary>
        /// <param name="name">El nom.</param>
        /// <param name="obj">L'objecte.</param>
        /// 
        public void Serialize(string name, object obj) {

            _context.WriteObject(name, obj);
        }

        /// <summary>
        /// El context de serialitzacio.
        /// </summary>
        /// 
        public ISerializationWriter Context =>
            _context;
    }
}