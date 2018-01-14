using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Parent
    {
        public virtual Guid ParentId { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Child> Children { get; set; }
        public virtual bool IsForQuery { get; set; }
    }

    public class ParentMap : ClassMap<Parent>
    {
        public ParentMap()
        {
            #region Please modify the code to pass the test
            Not.LazyLoad(); // do not create proxy for this class
            Table("parent");
            Id(p => p.ParentId).GeneratedBy.GuidNative();
            Map(p => p.Name);
            Map(p => p.IsForQuery);
            HasMany(p => p.Children).KeyColumn("[ParentID]").Inverse().Cascade.All();

            #endregion
        }
    }
}