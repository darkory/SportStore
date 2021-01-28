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
    public partial class SaleForm : Form
    {
        List<Sale> sales;
        public SaleForm()
        {
            InitializeComponent();
        }

        private void SortButton_Click(object sender, EventArgs e) // Сортирует данные по указанным параметрам
        {
            sales = DGVToList(); // Данные из DataGridView  преобразуются в List<Sale>
            // Выбор поля сортировки и упорядочивание по этому параметру
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    sales = sales.OrderBy(p => p.Id).ToList();
                    break;
                case 1:
                    sales = sales.OrderBy(p => p.Name).ToList();
                    break;
                case 2:
                    sales = sales.OrderBy(p => p.Category).ToList();
                    break;
                case 3:
                    sales = sales.OrderBy(p => p.Price).ToList();
                    break;
                case 4:
                    sales = sales.OrderBy(p => p.Amount).ToList();
                    break;
                default:
                    MessageBox.Show("Wrong Input");
                    return;
            }
            if (DecreaseSortRadioButton.Checked) // Если кнопка "по убыванию" - "переворачиваем" список
            {
                sales.Reverse();
            }
            saleBindingSource.DataSource = sales;
        }

        private void отчётToolStripMenuItem_Click(object sender, EventArgs e) // Переход к отчёту
        {
            new ReportForm(DGVToList()).Show();
        }

        private void продуктыToolStripMenuItem_Click(object sender, EventArgs e) // Переход к ProductForm
        {
            new ProductsForm().Show();
            Close();
        }

        private void SaleForm_Load(object sender, EventArgs e) 
        {            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sportStore_dbDataSet.Sale". При необходимости она может быть перемещена или удалена.
            this.saleTableAdapter.Fill(this.sportStore_dbDataSet.Sale);
        }

        private void срхранитьДанныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.saleBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.sportStore_dbDataSet);
        }

        public List<Sale> DGVToList() // метод получающий List<Sale> исходя из данных DataGridView
        {
            List<Sale> lst = new List<Sale>();
            
            for(int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                var name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                var description = dataGridView1.Rows[i].Cells[3].Value.ToString();
                var category = dataGridView1.Rows[i].Cells[2].Value.ToString();
                var price = Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value.ToString());
                var amount = Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value.ToString());
                var saleDate = Convert.ToDateTime(dataGridView1.Rows[i].Cells[6].Value.ToString());
                lst.Add(new Sale(name, description, category, price, amount, saleDate));
            }
            return lst;
        }

        private void toolStripButton1_Click(object sender, EventArgs e) // Сохранение данных
        {
            this.Validate();
            this.saleBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.sportStore_dbDataSet);
        }
    }
}
