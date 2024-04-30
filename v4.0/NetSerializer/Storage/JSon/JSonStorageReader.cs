namespace NetSerializer.v4.Storage.JSon {

    using System;
    using System.Collections.Generic;

    public sealed class JSonStorageReader: StorageReader {

        public override object ReadValue(string name, Type type) {
            throw new System.NotImplementedException();
        }

        public override void ReadObjectStart(string name, out Type type, out int id) {
            throw new System.NotImplementedException();
        }

        public override void ReadObjectEnd() {
            throw new System.NotImplementedException();
        }

        public override void ReadStructStart(string name, Type type) {
        }

        public override void ReadStructEnd() {
        }

        public override void ReadArrayStart(string name, out int count, out int[] bounds) {
            throw new System.NotImplementedException();
        }

        public override void ReadArrayEnd() {
            throw new System.NotImplementedException();
        }

        public override void Initialize() {
            throw new System.NotImplementedException();
        }

        public override void Close() {
            throw new System.NotImplementedException();
        }

        public override int Version {
            get { throw new System.NotImplementedException(); }
        }

    }
}
