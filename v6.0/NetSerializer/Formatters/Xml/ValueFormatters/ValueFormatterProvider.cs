namespace NetSerializer.V6.Formatters.Xml.ValueFormatters {

    public sealed class ValueFormatterProvider {

        private static ValueFormatterProvider? _instance;
        private readonly List<ValueFormatter> _formatters = [];
        private readonly Dictionary<Type, ValueFormatter> _cache = [];

        /// <summary>
        /// Constructor privat.
        /// </summary>
        /// 
        private ValueFormatterProvider() {

            AddFormatters();
        }

        /// <summary>
        /// Afegeix els formatadors que es trobin el domini de l'aplicacio.
        /// </summary>
        /// 
        private void AddFormatters() {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName != null && !a.FullName.StartsWith("System.") && !a.FullName.StartsWith("Microsoft."));
            foreach (var assembly in assemblies) {

                var types = assembly.GetTypes();
                foreach (var type in types) {

                    // Afegeix si es una clase derivada de 'ValueFormatter'.
                    //
                    if (type.IsClass && !type.IsAbstract && typeof(ValueFormatter).IsAssignableFrom(type)) {
                        var formatter = (ValueFormatter?)Activator.CreateInstance(type);
                        if (formatter != null)
                            _formatters.Add(formatter);
                    }
                }
            }
        }

        /// <summary>
        /// Obte el formatador per un tipus especificat.
        /// </summary>
        /// <param name="type">El tipus.</param>
        /// <param name="throwOnError">True si llança una excepcio en cas d'error.</param>
        /// <returns>El formatador o null si no el troba.</returns>
        /// <exception cref="InvalidOperationException">No s'ha trobat cap.</exception>
        /// 
        public ValueFormatter? GetValueFormatter(Type type, bool throwOnError = true) {

            if (!_cache.TryGetValue(type, out ValueFormatter? formatter)) {

                formatter = _formatters.Find(item => item.CanFormat(type));
                if (formatter != null)
                    _cache.Add(type, formatter);

                else if (throwOnError)
                    throw new InvalidOperationException($"No se registró el formateador para el tipo '{type}'.");
            }

            return formatter;
        }

        /// <summary>
        /// Obte una instancia unica del objecte.
        /// </summary>
        /// 
        public static ValueFormatterProvider Instance {
            get {
                _instance ??= new ValueFormatterProvider();
                return _instance;
            }
        }
    }
}
