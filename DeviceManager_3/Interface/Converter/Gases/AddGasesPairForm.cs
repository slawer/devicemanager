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
    public partial class AddGasesPairForm : Form
    {
        Application _app;

        Formula f, s;
        ArgumentPair pair;

        public AddGasesPairForm(Application app)
        {
            InitializeComponent();
            _app = app;
        }

        /// <summary>
        /// добавить сигнал конвертора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void first_btn_Click(object sender, EventArgs e)
        {
            ResultsForm frm = new ResultsForm(_app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                f = frm.SelectedParameter;
                textBox1.Text = f.Macros.Description;
            }
        }

        private void second_btn_Click(object sender, EventArgs e)
        {
            ResultsForm frm = new ResultsForm(_app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                s = frm.SelectedParameter;
                textBox2.Text = s.Macros.Description;
            }
        }

        /// <summary>
        /// Добавленная пара
        /// </summary>
        public ArgumentPair Pair
        {
            get
            {
                if (f != null && s != null)
                {
                    pair = new ArgumentPair();
                    
                    pair.First.Source = DataSource.Results;
                    
                    pair.First.Index = f.Position;
                    pair.First.IsActual = f.IsActual;

                    pair.Second.Source = DataSource.Results;

                    pair.Second.Index = s.Position;
                    pair.Second.IsActual = s.IsActual;

                    return pair;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    f = GetFormula(value.First.Index);
                    s = GetFormula(value.Second.Index);

                    textBox1.Text = f.Macros.Description;
                    textBox2.Text = s.Macros.Description;
                }
                else
                {
                    f = null;
                    s = null;
                }
            }
        }

        Formula GetFormula(int index)
        {
            foreach (Formula formula in _app.Converter.Formuls)
            {
                if (formula != null && formula.Position == index)
                {
                    return formula;
                }
            }

            return null;
        }
    }
}