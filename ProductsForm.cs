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
    public partial class ProductsForm : Form
    {
        List<Product> products;

        public ProductsForm()
        {
            InitializeComponent();
            products = new List<Product>();
        }

        private void SortButton_Click(object sender, EventArgs e)
        {
            products = DGVToList(); // преобразование DataGridView в List
            // Выбор поля сортировки и упорядочивание
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    products = products.OrderBy(p => p.Id).ToList();
                    break;
                case 1:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case 2:
                    products = products.OrderBy(p => p.Category).ToList();
                    break;
                case 3:
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case 4:
                    products = products.OrderBy(p => p.Amount).ToList();
                    break;
                default:
                    MessageBox.Show("Wrong Input");
                    return;
            }
            if(DecreaseSortRadioButton.Checked) // Проверка возрастание/убывание
            {
                products.Reverse();
            }
            productBindingSource.DataSource = products;
        }

        private void ProductsForm_Load(object sender, EventArgs e) // событие, возникающее при загрузке формы
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sportStore_dbDataSet.Product". При необходимости она может быть перемещена или удалена.
            this.productTableAdapter.Fill(this.sportStore_dbDataSet.Product);
        }


        private void оПрограммеToolStripMenuItem4_Click(object sender, EventArgs e) // переход к форме о программе
        {
            new AboutForm().Show();
        }

        private void перейтиКПродажамToolStripMenuItem3_Click(object sender, EventArgs e) // переход к форме проданных товаров
        {
            new SaleForm().Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) // сохранение данных в базу данных
        {
            this.Validate();
            this.productBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.sportStore_dbDataSet);
        }

        public List<Product> DGVToList() // метод получающий List<Product> исходя из данных DataGridView
        {
            List<Product> lst = products;

            for (int i = 0; i < productDataGridView.RowCount - 1; i++)
            {
                var name = productDataGridView.Rows[i].Cells[1].Value.ToString();
                var description = productDataGridView.Rows[i].Cells[3].Value.ToString();
                var category = productDataGridView.Rows[i].Cells[2].Value.ToString();
                var price = Convert.ToDecimal(productDataGridView.Rows[i].Cells[4].Value.ToString());
                var amount = Convert.ToInt32(productDataGridView.Rows[i].Cells[5].Value.ToString());
                if (!Contains(lst, name, description, category, price, amount))
                {
                    lst.Add(new Product(name, description, category, price, amount));
                }                
            }
            return lst;
        }

        private bool Contains(List<Product> lst, string name, string description, string category, decimal price, int amount)
        {
            foreach (var i in lst)
            {
                if (i.Name == name && i.Description == description && i.Category == category && i.Price == price && i.Amount == amount)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
