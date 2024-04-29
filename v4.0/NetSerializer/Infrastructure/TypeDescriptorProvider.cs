namespace MikroPic.NetSerializer.v4.Infrastructure {

    using System;
    using System.Collections.Generic;

    public sealed class TypeDescriptorProvider {

        private readonly Dictionary<Type, TypeDescriptor> cache;
        private static TypeDescriptorProvider instance;

        /// <summary>
        /// Constructor de la clase. Es ocult per gestionar la creacio
        /// en forma singleton.
        /// </summary>
        private TypeDescriptorProvider() {

            cache = new Dictionary<Type, TypeDescriptor>();
        }

        /// <summary>
        /// Obte el descriptor d'una clase.
        /// </summary>
        /// <param name="obj">La instancia d'una clase.</param>
        /// <returns></returns>
        public TypeDescriptor GetDescriptor(object obj) {

            return GetDescriptor(obj.GetType());
        }

        /// <summary>
        ///  Obte el descriptor d'una clase.
        /// </summary>
        /// <param name="type">El tipus de la clase.</param>
        /// <returns></returns>
        public TypeDescriptor GetDescriptor(Type type) {

            TypeDescriptor descriptor;

            if (!cache.TryGetValue(type, out descriptor)) {
                descriptor = new TypeDescriptor(type);
                cache.Add(type, descriptor);
            }

            return descriptor;
        }

        /// <summary>
        /// Obte una instancia unica a la clase.
        /// </summary>
        public static TypeDescriptorProvider Instance {
            get {
                if (instance == null)
                    instance = new TypeDescriptorProvider();
                return instance;
            }
        }
    }
}

