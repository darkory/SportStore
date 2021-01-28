using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore
{
    public static class Admin
    {
        private static string login;
        private static string password;

        static Admin()
        {
            login = "Admin";
            password = "1234";
        }

        public static bool Login(string login, string password) // метод входа в систему
        {
            if(login == Admin.login && password == Admin.password)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
