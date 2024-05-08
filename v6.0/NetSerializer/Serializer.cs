namespace NetSerializer.V6 {

    public sealed class Serializer {
        
        private readonly IFormatWriter _writer;
        
        public Serializer(IFormatWriter writer) {
            
            _writer = writer;
        }
        
        public Serializer Serialize(string name, int value) {
            
            _writer.WriteInt(name, value);
            
            return this;
        }

        public Serializer Serialize(string name, float value) {
            
            _writer.WriteFloat(name, value);
            
            return this;
        }

        public Serializer Serialize(string name, double value) {
            
            _writer.WriteDouble(name, value);
            
            return this;
        }
        
        public Serializer Serialize(string name, object obj) {
            
            _writer.WriteObjecteader(name, obj);
            
            _writer.WriteObjectTail();
        }
    }

}