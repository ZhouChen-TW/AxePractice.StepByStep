using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalApi
{
    class DefaultControllerFactory : IControllerFactory
    {
        public HttpController CreateController(
            string controllerName,
            ICollection<Type> controllerTypes,
            IDependencyResolver resolver)
        {
            #region Please modify the following code to pass the test.

            /*
             * The controller factory will create controller by its name. It will search
             * form the controllerTypes collection to get the correct controller type,
             * then create instance from resolver.
             */
            var types = controllerTypes.Where(t => t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (types.Count > 1) throw new ArgumentException();
            return types.Count < 1 ? null : (HttpController) resolver.GetService(types.Single());
            #endregion
        }
    }
}