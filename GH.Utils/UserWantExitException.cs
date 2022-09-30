using System;

namespace GH.Utils
{
    public class UserWantExitException : Exception
    {
        public UserWantExitException() : base("Пользователь отказался от входа в программу") { }
    }
}
