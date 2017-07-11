using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LocalApi
{
    class DefaultHttpControllerTypeResolver : IHttpControllerTypeResolver
    {
        #region Please modify the following code to pass the test

        /*
         * This class is used to get all types that is non-abstract public controller
         * from given assemblies.
         */
        
        public ICollection<Type> GetControllerTypes(IEnumerable<Assembly> assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var collection = Assembly.Load(assembly.GetName()).GetTypes()
                    .Where(s => s.BaseType == typeof(HttpController)).ToList();

                types.AddRange(collection);
            }

            return types;
        }

        #endregion
    }
}