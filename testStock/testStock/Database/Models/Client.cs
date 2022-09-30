using FluentNHibernate.Mapping;
using GH.Database;

namespace Tester.Database
{
    public class Client : BaseEntity
    {
    }

    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Table("CLIENTS");
            Id(x => x.Id, "CLIENT_ID");
            Map(x => x.Name, "CLIENT_NAME");
        }
    }
}
