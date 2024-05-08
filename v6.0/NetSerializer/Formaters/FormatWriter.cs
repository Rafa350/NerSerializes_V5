namespace NetSerializer.V6.Formaters {
    
    public abstract class: IFormatWriter {
        
        public abstract void WriteInt(string name, int value);
        
        public abstract void WriteFloat(string name, float value);
     
        public abstract void WriteDouble(string name, double value);
       
        public abstract void WriteObjectHeader(string name, Type type, int id);
        
        public abstract void WriteObjectTail(string name);
    }
}