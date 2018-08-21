using System;
using System.Runtime.CompilerServices;

namespace Apd.Common.Container {
    public interface IContainer {
        ResolveType Resolve<ResolveType>() where ResolveType : class;
    }
}