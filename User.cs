using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime RegistrationDate { get; private set; }
        public DateTime LastSessionDate { get; set; }
        private string _password;

        private static int ID;

        public User(string username, string password)
        {
            Username = username;
            _password = password;
            Id = ++ID;
        }

        static User() // статический конструктор иницализирует статические данные класса
        {
            ID = 0;
        }

        public bool LogIn(string username, string password) // метод входа пользователя
        {
            if(Username == username && _password == password)
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
