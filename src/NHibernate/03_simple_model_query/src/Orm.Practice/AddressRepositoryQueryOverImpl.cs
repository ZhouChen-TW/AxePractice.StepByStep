using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

namespace Orm.Practice
{
    public class AddressRepositoryQueryOverImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryQueryOverImpl(ISession session) : base(session)
        {
        }

        public Address Get(int id)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.Id == id).SingleOrDefault();

            #endregion
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().WhereRestrictionOn(a => a.Id).IsIn(ids.ToList()).OrderBy(a => a.Id).Asc.List();

            #endregion
        }

        public IList<Address> GetByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.List();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.ListAsync();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.ListAsync(cancellationToken);

            #endregion
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(item => item.City == city).OrderBy(item => item.Id).Asc
                .SelectList(o => o.Select(a => a.Id).Select(a => a.AddressLine1)).List<object[]>()
                .Select(objects => new KeyValuePair<int, string> ((int) objects[0], (string) objects[1]))
                .ToList();

            #endregion
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).Select(
                Projections.Distinct(
                    Projections.Property<Address>(a => a.PostalCode))).List<string>();

            #endregion
        }
    }
}