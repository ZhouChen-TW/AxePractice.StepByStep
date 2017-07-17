using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ComponentContext : IComponentContext
    {
        #region Please modify the following code to pass the test

        /*
         * A ComponentContext is used to resolve a component. Since the component
         * is created by the ContainerBuilder, it brings all the registration
         * information.
         *
         * You can add non-public member functions or member variables as you like.
         */
        readonly Dictionary<Type, object> registers;
        public ComponentContext(Dictionary<Type, object> registers)
        {
            this.registers = registers;
        }

        public object ResolveComponent(Type type)
        {
            if (registers.ContainsKey(type))
            {
                return ((Func<IComponentContext, object>)registers[type])(this);
            }

            throw new DependencyResolutionException();
        }

        #endregion
    }
}