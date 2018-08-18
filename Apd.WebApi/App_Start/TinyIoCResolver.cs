using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Apd.Common.Container;

namespace Apd.WebApi {
    public class TinyIoCResolver : IDependencyResolver {
        protected TinyIoCContainer container;
        
        public TinyIoCResolver(TinyIoCContainer container) {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public object GetService(Type serviceType) {
            try {
                return container.Resolve(serviceType);
            }
            catch (Exception) {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            try{
                return this.container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return new List<object>();
            }
        }

        public IDependencyScope BeginScope() {
            var child = this.container.GetChildContainer();
            return new TinyIoCResolver(child);
        }
        
        public void Dispose(){
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing){
            container.Dispose();
        }
    }
}