using NetSerializer.V6.Formatters;

namespace NetSerializer.V6 {

    public sealed class Serializer {
        
        private readonly SerializationContext _context;
        
        public Serializer(FormatWriter writer) {

            _context = new SerializationContext(writer);
        }
        
        public void Serialize(string name, object obj) {
            
            _context.WriteObject(name, obj);
        }
            
        public ISerializationWriter Context =>
            _context;
    }
}