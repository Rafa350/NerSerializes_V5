namespace NetSerializer.V6 {

    public interface IDeserializationReader {
        
        bool ReadBool(string name);
        
        int ReadInt(string name);
        
        float ReadSingle(string name);
        
        double ReadDouble(string name);
        
        object? ReadObject(string name);
    }

}