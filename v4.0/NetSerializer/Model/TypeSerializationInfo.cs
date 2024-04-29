namespace MikroPic.NetSerializer.v4.Model {

    using System;
    using System.Collections.Generic;

    internal sealed class TypeSerializationInfo {

        private Type type;
        private string alias;
        private List<PropertySerializationInfo> properties;

        public TypeSerializationInfo(Type type) {

            if (type == null)
                throw new ArgumentNullException("type");

            Initialize(type, null, null);
        }

        public TypeSerializationInfo(Type type, string alias) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (String.IsNullOrEmpty(alias))
                throw new ArgumentNullException("alias");

            Initialize(type, alias, null);
        }

        public TypeSerializationInfo(Type type, string alias, IEnumerable<PropertySerializationInfo> properties) {

            if (type == null)
                throw new ArgumentNullException("type");

            if (String.IsNullOrEmpty(alias))
                throw new ArgumentNullException("alias");

            if (properties == null)
                throw new ArgumentNullException("properties");

            Initialize(type, alias, properties);
        }

        private void Initialize(Type type, string alias, IEnumerable<PropertySerializationInfo> properties) {

            this.type = type;
            this.alias = alias;
            this.properties = properties == null ? null : new List<PropertySerializationInfo>(properties);
        }

        public void AddProperty(PropertySerializationInfo property) {

            if (property == null)
                throw new ArgumentNullException("property");

            if (properties == null)
                properties = new List<PropertySerializationInfo>();
            properties.Add(property);
        }

        public void AddProperties(IEnumerable<PropertySerializationInfo> properties) {

            if (properties == null)
                throw new ArgumentNullException("property");

            foreach (PropertySerializationInfo property in properties)
                AddProperty(property);
        }

        public Type Type {
            get {
                return type;
            }
        }
        
        public string Alias {
            get {
                return alias;
            }
        }

        public IEnumerable<PropertySerializationInfo> Properties {
            get {
                return properties;
            }
        }
    }
}
