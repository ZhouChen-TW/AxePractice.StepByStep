using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class StudentSetting
    {
        public virtual Guid Id { get; set; }
        public virtual bool IsOpen { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual Student Student { get; set; }
    }

    public class StudentSettingMap : ClassMap<StudentSetting>
    {
        public StudentSettingMap()
        {
            Table("dbo.student_setting");
            Id(s => s.Id).GeneratedBy.GuidNative();
            Map(s => s.IsOpen);
            Map(s => s.IsForQuery);
//            HasOne(s => s.Student).Constrained().Cascade.All();
            References(x => x.Student, "StudentId").Cascade.All().Unique();
        }
    }
}