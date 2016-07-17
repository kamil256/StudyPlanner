using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudyPlanner.Domain.Concrete;
using StudyPlanner.Domain.Abstract;
using StudyPlanner.Infrastructure.Abstract;
using StudyPlanner.Infrastructure.Concrete;

namespace StudyPlanner.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void AddBindings()
        {
            kernel.Bind<IRepository>().To<Repository>();
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}