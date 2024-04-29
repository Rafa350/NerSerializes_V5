namespace MikroPic.NetSerializer.v4.Model {

    using System;

    internal sealed class PropertySerializationInfo {

        private readonly string name;

        public PropertySerializationInfo(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.name = name;
        }

        public string Name {
            get {
                return name;
            }
        }
    }
}
