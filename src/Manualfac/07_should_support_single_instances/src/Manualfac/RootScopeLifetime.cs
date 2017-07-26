using System;

namespace Manualfac
{
    class RootScopeLifetime : IComponentLifetime
    {
        public ILifetimeScope FindLifetimeScope(ILifetimeScope mostNestedLifetimeScope)
        {
            #region Please implement this method

            /*
             * This class will always create and share instaces in root scope.
             */
            if (mostNestedLifetimeScope == null) throw new ArgumentNullException(nameof(mostNestedLifetimeScope));

            ILifetimeScope scope = mostNestedLifetimeScope;
            while (scope.RootScope != null)
            {
                scope = scope.RootScope;
            }
            return scope;

            #endregion
        }
    }
}