using NetSerializer.V6.Formaters;

namespace NetSerializer.V6 {

    public sealed class Serializer {
        
        private readonly SerializerContext _context;
        
        public Serializer(IFormatWriter writer) {

            _context = new SerializerContext(writer);
        }
        
        public Serializer Serialize(string name, bool value) {
            
            _context.Writer.WriteBool(name, value);
            
            return this;
        }

        public Serializer Serialize(string name, int value) {

            _context.Writer.WriteInt(name, value);

            return this;
        }

        public Serializer Serialize(string name, float value) {
            
            _context.Writer.WriteFloat(name, value);
            
            return this;
        }

        public Serializer Serialize(string name, double value) {
            
            _context.Writer.WriteDouble(name, value);
            
            return this;
        }
        
        public Serializer Serialize(string name, object obj) {
            
            if (obj == null)
                _context.Writer.WriteObjectNull(name);
            
            else {                
                if (_context.GetObjectId(obj, out int id))
                    _context.Writer.WriteObjectReference(name, id);
            
                else {                
                    var type = obj.GetType();
                    _context.Writer.WriteObjectHeader(name, type, id);                

                    // TODO Serialitzar l'objecte

                    _context.Writer.WriteObjectTail();
                }
            }
            
            return this;
        }
    }

}