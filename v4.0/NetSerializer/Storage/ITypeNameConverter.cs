namespace MikroPic.NetSerializer.v4.Storage {

    using System;

    public interface ITypeNameConverter {

        string ToString(Type type);
        Type ToType(string typeName);
    }
}
