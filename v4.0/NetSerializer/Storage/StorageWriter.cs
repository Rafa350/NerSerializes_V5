namespace MikroPic.NetSerializer.v4.Storage {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase base pels escriptors de dades.
    /// </summary>
    public abstract class StorageWriter {

        public abstract void WriteValue(string name, object value);
        public abstract void WriteNull(string name);

        public abstract void WriteArrayStart(string name, Type type, int[] bound, int count);
        public abstract void WriteArrayEnd();

        public abstract void WriteStructStart(string name, Type type, object value);
        public abstract void WriteStructEnd();

        public abstract void WriteObjectStart(string name, Type type, int id);
        public abstract void WriteObjectEnd();
        public abstract void WriteObjectReference(string name, int id);

        public abstract void Initialize(int version);
        public abstract void Close();
    }
}
