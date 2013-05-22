using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeviceManager;
using ScintillaNET;
using Microsoft.VisualBasic;

namespace DeviceManager
{
    public partial class ScriptForm : Form
    {
        protected Script _script = null;        // скрипт
        protected Scintilla editor = null;      // редактор кода

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public ScriptForm()
        {
            InitializeComponent();

            editor = new Scintilla();
            editor.Dock = DockStyle.Fill;
            editor.ConfigurationManager.Language = "cs";

            editor.Margins.Margin0.Width = 35;
            editor.Text = Script.TempladeScriptCode;

            panel1.Controls.Add(editor);
        }

        /// <summary>
        /// Скрипт
        /// </summary>
        public Script Script
        {
            get
            {
                return _script;
            }
        }

        /// <summary>
        /// комментарий
        /// </summary>
        public string Comment
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        /// <summary>
        /// Автиматически присвоить номер канала
        /// </summary>
        public bool AutoSetNumber
        {
            get { return checkBox1.Checked; }
        }

        /// <summary>
        /// Номер присваиваемого канала
        /// </summary>
        public int Number
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        private void Compile_Click(object sender, EventArgs e)
        {
            textBoxOutput.Text = string.Empty;
            if (editor.Text != string.Empty)
            {
                List<string> assemblies = new List<string>();
                assemblies.AddRange(listBoxAssembies.Items.Cast<string>());

                System.Collections.Specialized.StringCollection strs = Script.Compile(editor.Text, assemblies.ToArray());               
                
                if (strs != null && strs.Count > 0)
                {
                    foreach (String line in strs)
                    {
                        textBoxOutput.Text += line + Microsoft.VisualBasic.Constants.vbCrLf;
                    }
                }
                else if (strs != null && strs.Count == 0)
                {
                    textBoxOutput.Text = "Скрипт скомпилирован без ошибок";
                }
                else if (strs == null)
                {
                    textBoxOutput.Text = "Скрипт не прошел компиляцию";
                }
            }
            else
            {
                textBoxOutput.Text = "Не введен код скрипта.";
            }
        }

        /// <summary>
        /// загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            editor.Text = Script.TempladeScriptCode;

            textBoxClass.Text = Script.TemplateClassName;
            textBoxNamespace.Text = Script.TemplateScriptNamespace;

            string[] asses = Script.TemplateAssemblies;
            if (asses != null)
            {
                foreach (string assembly in asses)
                {
                    listBoxAssembies.Items.Add(assembly);
                }
            }
        }

        /// <summary>
        /// добавляем сборку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonInsertAssembly_Click(object sender, EventArgs e)
        {
            AddNamespaceForm frm = new AddNamespaceForm();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                listBoxAssembies.Items.Add(frm.AddedAssembly);
            }
        }

        /// <summary>
        /// Удалить сборку из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemoveAssembly_Click(object sender, EventArgs e)
        {
            if (listBoxAssembies.SelectedItem != null)
            {
                listBoxAssembies.Items.Remove(listBoxAssembies.SelectedItem);
            }
        }

        /// <summary>
        /// Проверяем и создаем новый скрипт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDatainForm() == true)
                {
                    _script = new Script();

                    _script.ScriptCode = editor.Text;
                    _script.ClassName = textBoxClass.Text;

                    _script.Namespace = textBoxNamespace.Text;

                    _script.Assemblies.Clear();
                    if (listBoxAssembies.Items.Count > 0)
                    {
                        _script.Assemblies.AddRange(listBoxAssembies.Items.Cast<string>());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// проверить введенные данные в форме
        /// </summary>
        /// <returns></returns>
        private bool CheckDatainForm()
        {
            if (textBoxNamespace.Text == string.Empty)
            {
                MessageBox.Show(this, "Не указано пространство имен для скрипта", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                textBoxNamespace.SelectAll();
                textBoxNamespace.Focus();

                return false;
            }

            if (textBoxClass.Text == string.Empty)
            {
                MessageBox.Show(this, "Не указан используемый в скрипте класс", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                textBoxClass.SelectAll();
                textBoxClass.Focus();

                return false;
            }

            return true;
        }
    }
}