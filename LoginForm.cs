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
    public partial class LoginForm : Form
    {
        bool isCtrlClicked; // проверка был ли нажат ctrl
        bool isAltClicked; // проверка был ли нажат alt
        List<User> users;
        
        public LoginForm()
        {
            InitializeComponent();
            isCtrlClicked = false;
            isAltClicked = false;
            KeyPreview = true; // необходимо для обработки события KeyDown
        }

        private void LogInButton_Click(object sender, EventArgs e) // обработчик события нажатия на кнопку "Войти"
        {
            string username = UsernameTextbox.Text;
            string password = PasswordTextBox.Text;
            foreach(var u in users)
            {
                if(u.LogIn(username, password)) // Вызов метода войти у пользователя
                {
                    new ProductsForm().Show();
                    Hide(); // Прячем форму от пользователя
                    return;
                }
            }
            MessageBox.Show("Incorrect username or password"); // Сообщение о том, что данные введены некорректно.
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e) // Обработчик события нажатия на клавиши клавиатуры
        {
            // проверка последовательности клавиш
            // комбинация ctrl + alt + A
            // если ctrl не был нажат последним, а alt нажали, это не засчитывается как часть комбинации
            // тоже самое касается клавиши А без нажатия последовательно ctrl и alt прямо перед этим
            switch (e.KeyValue)
            {
                case 17:  //CtrlKey code
                    isCtrlClicked = true;
                    isAltClicked = false;
                    break;
                case 18:  //Alt key code                    
                    if (isCtrlClicked)
                    {
                        isAltClicked = true;
                        break;
                    }
                    else
                    {
                        goto default;
                    }
                case 65:  //A code                    
                    if (isCtrlClicked && isAltClicked)
                    {
                        new AdminLoginForm().Show();
                    }
                    goto default;
                default:
                    isCtrlClicked = false;
                    isAltClicked = false;
                    break;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e) // обработчик события загрузки формы
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sportStore_dbDataSet.User". При необходимости она может быть перемещена или удалена.
            this.userTableAdapter.Fill(this.sportStore_dbDataSet.User);
            users = DGVToList(); // запись пользователей из БД

        }

        // метод преобразования DataGridView в List<User>
        public List<User> DGVToList()
        {
            List<User> lst = new List<User>();

            for (int i = 0; i < userDataGridView.RowCount - 1; i++)
            {
                var name = userDataGridView.Rows[i].Cells[1].Value.ToString();
                var password = userDataGridView.Rows[i].Cells[2].Value.ToString();

                lst.Add(new User(name, password));
            }
            return lst;
        }
    }
}
