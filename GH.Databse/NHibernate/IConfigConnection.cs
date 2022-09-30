namespace GH.Database
{
    public interface IConfigConnection
    {
        bool AutoEntering { get; set; }
        bool IsComplete { get; }
        bool UserIsValid { get; }
        string UserLogin { get; set; }
        string UserPassword { get; set; }

        void CheckIdentity();
        string ConnectionString();
        bool ConnectIsOK();
        string GetRemoteServer();
        BaseUser GetUser();
        bool IsRemoteDataBase();
        bool TestConnection();
    }
}
