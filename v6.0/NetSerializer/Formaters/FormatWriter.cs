namespace NetSerializer.V6.Formaters {

    public abstract class FormatWriter: IFormatWriter, IDisposable {

        /// <summary>
        /// Finalitza l'operacio d'escriptura.
        /// </summary>
        /// 
        public abstract void Close();

        /// <inheritdoc/>
        /// 
        public abstract void WriteBool(string name, bool value);

        /// <inheritdoc/>
        /// 
        public abstract void WriteInt(string name, int value);

        /// <inheritdoc/>
        /// 
        public abstract void WriteFloat(string name, float value);

        /// <inheritdoc/>
        /// 
        public abstract void WriteDouble(string name, double value);

        /// <inheritdoc/>
        /// 
        public abstract void WriteObjectNull(string name);

        /// <inheritdoc/>
        /// 
        public abstract void WriteObjectReference(string name, int id);

        /// <inheritdoc/>
        /// 
        public abstract void WriteObjectHeader(string name, Type type, int id);

        /// <inheritdoc/>
        /// 
        public abstract void WriteObjectTail();

        /// <summary>
        /// Disposa l'objecte.
        /// </summary>
        /// 
        public abstract void Dispose();
    }
}