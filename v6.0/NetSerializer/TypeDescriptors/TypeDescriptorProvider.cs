namespace NetSerializer.V6.TypeDescriptors {

    public sealed class TypeDescriptorProvider {

        private static TypeDescriptorProvider? _instance;
        private readonly Dictionary<Type, TypeDescriptor> _cache = [];

        /// <summary>
        /// Constructor de la clase. Es privat per gestionar la creacio
        /// en forma singleton.
        /// </summary>
        /// 
        private TypeDescriptorProvider() {

        }

        /// <summary>
        /// Obte el descriptor d'una clase.
        /// </summary>
        /// <param name="type">El tipus de la clase.</param>
        /// <returns></returns>
        /// 
        public TypeDescriptor GetDescriptor(Type type) {

            ArgumentNullException.ThrowIfNull(type, nameof(type));

            if (!_cache.TryGetValue(type, out TypeDescriptor? typeDescriptor)) {
                typeDescriptor = new TypeDescriptor(type);
                _cache.Add(type, typeDescriptor);
            }

            return typeDescriptor;
        }

        /// <summary>
        /// Obte una instancia unica a la clase.
        /// </summary>
        /// 
        public static TypeDescriptorProvider Instance {
            get {
                _instance ??= new TypeDescriptorProvider();
                return _instance;
            }
        }
    }
}
