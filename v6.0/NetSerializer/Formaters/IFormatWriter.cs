using System;

namespace NetSerializer.V6.Formaters {
    
    public interface IFormatWriter {
        
        void WriteInt(string name, int value);
        
        void WriteFloat(string name, float value);
     
        void WriteDouble(string name, double value);
       
        void WriteObjectHeader(string name, Type type, int id);
        
        void WriteObjectTail(string name, object obj);
    }
}