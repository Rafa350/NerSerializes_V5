using NetSerializer.V6.TypeSerializers.Serializers;

namespace NetSerializer.V6.TypeSerializers {

    /// <summary>
    /// Gestiona els serialitzadors.
    /// </summary>
    /// 
    public sealed class TypeSerializerProvider {

        private static TypeSerializerProvider? _instance;
        private readonly List<ITypeSerializer> _serializerInstances = [];
        private readonly Dictionary<Type, ITypeSerializer> _serializerCache = [];

        /// <summary>
        /// Constructor de la clase. Es privat per implementar una clase singleton.
        /// </summary>
        /// 
        private TypeSerializerProvider() {

            AddSerializers();
        }

        /// <summary>
        /// Afegeix els serialitzadors que es trobin el domini de l'aplicacio.
        /// </summary>
        /// 
        private void AddSerializers() {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => (a.FullName != null) && !a.FullName.StartsWith("System.") && !a.FullName.StartsWith("Microsoft."));
            foreach (var assembly in assemblies) {

                var types = assembly.GetTypes();
                foreach (var type in types) {

                    // Afegeix si es una clase derivada de 'CustomTypeSerializer'.
                    //
                    if (type.IsClass && !type.IsAbstract && typeof(CustomClassSerializer).IsAssignableFrom(type)) {
                        var serializer = (ITypeSerializer?) Activator.CreateInstance(type);
                        if (serializer != null) 
                            _serializerInstances.Add(serializer);
                    }
                }
            }

            _serializerInstances.Add(new ArraySerializer());
            _serializerInstances.Add(new StructSerializer());
            //_serializerInstances.Add(new ListSerializer());
            _serializerInstances.Add(new ClassSerializer());  // Cal que sigui l'ultima de la llista
        }

        /// <summary>
        /// Obte el serialitzador per un tipus especificat.
        /// </summary>
        /// <param name="type">El tipus.</param>
        /// <param name="throwOnError">True si llança una excepcio en cas d'error.</param>
        /// <returns>El serialitzador o null si no el troba.</returns>
        /// <exception cref="InvalidOperationException">No s'ha trobat cap serialitzador.</exception>
        /// 
        public ITypeSerializer? GetTypeSerializer(Type type, bool throwOnError = true) {

            if (!_serializerCache.TryGetValue(type, out ITypeSerializer? serializer)) {

                serializer = _serializerInstances.Find(item => item.CanProcess(type));
                if (serializer != null)
                    _serializerCache.Add(type, serializer);

                else if (throwOnError)
                    throw new InvalidOperationException($"No se registró el serializador para el tipo '{type}'.");
            }

            return serializer;
        }

        /// <summary>
        /// Obte una instancia unica a la clase.
        /// </summary>
        /// 
        public static TypeSerializerProvider Instance {
            get {
                _instance ??= new TypeSerializerProvider();
                return _instance;
            }
        }
    }
}
