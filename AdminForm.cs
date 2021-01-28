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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "sportStore_dbDataSet.User". При необходимости она может быть перемещена или удалена.
            this.userTableAdapter.Fill(this.sportStore_dbDataSet.User);

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Validate();
            userBindingSource.EndEdit();
            tableAdapterManager.UpdateAll(sportStore_dbDataSet);
        }
    }
}
