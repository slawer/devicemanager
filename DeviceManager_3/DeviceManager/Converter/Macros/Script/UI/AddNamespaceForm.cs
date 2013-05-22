using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceManager
{
    public partial class AddNamespaceForm : Form
    {
        public AddNamespaceForm()
        {
            InitializeComponent();
        }

        public string AddedAssembly
        {
            get
            {
                return textBoxAssembly.Text;
            }
        }

        private void accept_Click(object sender, EventArgs e)
        {
            if (textBoxAssembly.Text == string.Empty)
            {
                MessageBox.Show(this, "Не указана сборка", "Предупреждение", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DialogResult = DialogResult.None;
            }
        }
    }
}
