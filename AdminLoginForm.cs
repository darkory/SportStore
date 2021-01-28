using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportStore
{
    public partial class AdminLoginForm : Form
    {
        public AdminLoginForm()
        {
            InitializeComponent();
        }

        private void LogInButton_Click(object sender, EventArgs e) // метод обработки события нажатия на кнопку войти
        {
            string username = UsernameTextbox.Text;
            string password = PasswordTextBox.Text;
            if(Admin.Login(username, password)) // попытка входа в систему как админ
            {
                new AdminForm().Show();
                Close();
                return;
            }
            MessageBox.Show("Incorrect username or password"); // Сообщение о том, что данные ведены неверно
        }
    }
}
