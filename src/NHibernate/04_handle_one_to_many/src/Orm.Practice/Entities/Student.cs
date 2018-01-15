using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Student
    {
        public virtual Guid StudentId { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual StudentSetting StudentSetting { get; set; }
    }

    public class StudentMap : ClassMap<Student>
    {
        /*I thought this would be as easy as created a one-to-one mapping with lazy="proxy" set on the one-to-one on the Document class but this was not the case. 
You NEED to set constrained="true" on the mapping, basically going from this post I found: http://www.hibernate.org/162.html#A5. Say we have A->B where this is a 1-1 relationship,
now without a constraint from A-B this means A can exist without B, so there is a possiblity that B is null, a Proxy to B will be not null and won't work here.
         */
        public StudentMap()
        {
            Table("dbo.student");
            Id(s => s.StudentId).GeneratedBy.GuidNative();
            Map(s => s.Name);
            Map(s => s.IsForQuery);
            HasOne(s => s.StudentSetting).PropertyRef(s => s.Student).Cascade.All();
//            HasOne(s => s.StudentSetting).Constrained().Cascade.All();
        }
    }
}