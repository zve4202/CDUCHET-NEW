using FluentNHibernate.Mapping;
using GH.Database;

namespace Tester.Database
{
    public class User : BaseUser
    {
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("EMPLOYEE");
            Id(x => x.Id, "EMPLOYEE_ID");
            Map(x => x.Name, "EMP_NAME");
            Map(x => x.Login, "EMP_NAME");
            Map(x => x.Password, "PASSWRD");
            Map(x => x.Active, "WORKING");
        }
    }
}
