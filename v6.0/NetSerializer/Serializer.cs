using NetSerializer.V6.Formaters;

namespace NetSerializer.V6 {

    public sealed class Serializer {
        
        private readonly SerializerContext _context;
        
        public Serializer(FormatWriter writer) {

            _context = new SerializerContext(writer);
        }
        
        public Serialize(string name, object obj) {
            
            _context.WriteObject(name, obj);
        }
            
        public ISerializationWriter Context =>
            _context;
    }
}