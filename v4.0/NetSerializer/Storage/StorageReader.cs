namespace MikroPic.NetSerializer.v4.Storage {

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Clase base pels lectors de dades.
    /// </summary>
    public abstract class StorageReader {

        public abstract object ReadValue(string name, Type type);

        public abstract void ReadObjectStart(string name, out Type type, out int id);
        public abstract void ReadObjectEnd();

        public abstract void ReadArrayStart(string name, out int count, out int[] bounds);
        public abstract void ReadArrayEnd();

        public abstract void ReadStructStart(string name, Type type);
        public abstract void ReadStructEnd();

        public abstract void Initialize();
        public abstract void Close();

        public abstract int Version { get; }
    }
}
