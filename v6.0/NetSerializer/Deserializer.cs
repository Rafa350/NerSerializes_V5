using NetSerializer.V6.Formatters;

namespace NetSerializer.V6 {

    public sealed class Deserializer {
        
        private readonly DeserializationContext _context;
        
        public Deserializer(FormatReader reader) {

            _context = new DeserializationContext(reader);
        }
        
        public object Deserialize(string name) {

            return _context.ReadObject(name);
        }
        
        public IDeserializationReader Context =>
            _context;
    }
}
