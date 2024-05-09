namespace NetSerializer.V6.Attributes {

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class NetSerializerAttribute: Attribute {

        private readonly string? _aliasName = null;

        public string? AliasName {
            get => _aliasName;
            init => _aliasName = value;
        }
    }
}
