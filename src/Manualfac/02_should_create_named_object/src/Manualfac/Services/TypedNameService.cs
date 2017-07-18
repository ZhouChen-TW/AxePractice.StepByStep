using System;

namespace Manualfac.Services
{
    class TypedNameService : Service, IEquatable<TypedNameService>
    {
        #region Please modify the following code to pass the test

        /*
         * This class is used as a key for registration by both type and name.
         */
        readonly Type serviceType;
        readonly string name;
        public TypedNameService(Type serviceType, string name)
        {
            this.serviceType = serviceType;
            this.name = name;
        }

        public bool Equals(TypedNameService other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            return serviceType == other.serviceType && name.Equals(other.name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as TypedNameService);
        }

        public override int GetHashCode()
        {
            return serviceType.GetHashCode() ^ name.GetHashCode();
        }

        #endregion
    }
}