using System;
using System.Linq;
using NHibernate;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class OneToOneModifyFacts : OrmFactBase
    {
        public OneToOneModifyFacts(ITestOutputHelper output) : base(output)
        {
            ExecuteNonQuery("DELETE FROM [dbo].[student] WHERE IsForQuery=0");
            ExecuteNonQuery("DELETE FROM [dbo].[student_setting] WHERE IsForQuery=0");
        }

        [Fact]
        public void should_get_all_students_with_settings()
        {
            var students = Session.Query<Student>()
                .Where(s => s.IsForQuery)
                .OrderBy(s => s.Name).ToList();

            Assert.Equal(2, students.Count);
            Assert.Equal("Nancy", students.First().Name);
            Assert.Equal("Zoe", students.Last().Name);
            Assert.Equal(true, students.First().StudentSetting.IsOpen);
            Assert.Equal(false, students.Last().StudentSetting.IsOpen);
        }

        [Fact]
        public void should_insert_student_with_setting()
        {
            var student = new Student
            {
                Name = "student",
                IsForQuery = false
            };

            var studentSetting = new StudentSetting
            {
                IsOpen = true,
                IsForQuery = false,
                Student = student
            };

            student.StudentSetting = studentSetting;
            Session.Save(student);
            Session.Flush();

            Assert.Equal(true, Session.Query<Student>().Single(s => s.Name == "student").StudentSetting.IsOpen);
            
        }

        [Fact]
        public void should_insert_student_setting()
        {
            var student = new Student
            {
                Name = "student-1",
                StudentSetting = new StudentSetting()
            };

            var studentSetting = new StudentSetting
            {
                IsOpen = true,
                IsForQuery = false,
                Student = student
            };

            student.StudentSetting = studentSetting;

            Session.Save(studentSetting);
            Session.Flush();

            Assert.Equal(true, Session.Query<Student>().Single(s => s.Name == "student-1").StudentSetting.IsOpen);
        }

        [Fact]
        public void should_delete_student_with_setting()
        {
            var student = new Student
            {
                Name = "student",
                IsForQuery = false
            };

            var studentSetting = new StudentSetting
            {
                IsOpen = true,
                IsForQuery = false,
                Student = student
            };

            student.StudentSetting = studentSetting;
            Session.Save(student);
            Session.Flush();

            var students = Session.Query<Student>().Where(s => s.Name == "student").ToList();
            students.ForEach(parent => Session.Delete(parent));
            Session.Flush();

            Assert.False(Session.Query<StudentSetting>().Any(c => !c.IsForQuery));
        }

        [Fact]
        public void should_get_type_for_student_and_setting()
        {
            var student = Session.Query<Student>().First();
            Console.WriteLine(student.StudentSetting.GetType());
        }

        void ExecuteNonQuery(string sql)
        {
            ISQLQuery query = StatelessSession.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
    }
}