namespace GH.Database
{
    public enum Category { Connection, Security, User }
    public class DbConnectionProperty : UpdatablePropertyAttribute
    {
        public Category Category { get; set; }
        public override string Group
        {
            get
            {
                switch (Category)
                {
                    case Category.Connection:
                        return "Соединение";
                    case Category.Security:
                        return "Безопасность";
                    case Category.User:
                        return "Логин";
                    default:
                        break;
                }
                return base.Group;
            }

            set
            {
                base.Group = value;
            }
        }
    }
}
