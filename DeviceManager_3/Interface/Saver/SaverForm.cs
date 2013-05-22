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
    public partial class SaverForm : Form
    {
        private Application app;
        private string uri = string.Empty;      // файл для сохранения данных

        public SaverForm(Application _app)
        {
            app = _app;

            app.Saver.OnStart += new EventHandler(Saver_OnStart);
            app.Saver.OnStop += new EventHandler(Saver_OnStop);

            InitializeComponent();
        }

        /// <summary>
        /// формула не актуальна
        /// </summary>
        /// <param name="formula"></param>
        private void Converter_OnNotActualFormula(Formula formula)
        {
            foreach (ListViewItem item in listViewParameters.Items)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Parameter)
                    {
                        Parameter parameter = item.Tag as Parameter;
                        if (!parameter.IsActual || parameter.Position == formula.Position)
                        {
                            item.BackColor = Color.Salmon;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Удалили формулу из конвертора
        /// </summary>
        /// <param name="sender">Источник события</param>
        /// <param name="args">Параметры события</param>
        private void Converter_OnRemoveFormula(object sender, ConverterEventArgs args)
        {
            foreach (ListViewItem item in listViewParameters.Items)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Parameter)
                    {
                        Parameter parameter = item.Tag as Parameter;
                        if (!parameter.IsActual || parameter.Position == args.Formula.Position)
                        {
                            item.BackColor = Color.Salmon;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Файл для создания
        /// </summary>
        public string URI { get { return uri; } }

        /// <summary>
        /// Запущен сохраняльщик, блокируем ввод
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Saver_OnStart(object sender, EventArgs e)
        {
            select.Enabled = false;
            remove.Enabled = false;

            back.Enabled = false;
            forward.Enabled = false;

            selectFile.Enabled = false;
            accept.Enabled = false;
        }

        /// <summary>
        /// Остановлен сохраняльщик, разрешаем ввод
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Saver_OnStop(object sender, EventArgs e)
        {
            select.Enabled = true;
            remove.Enabled = true;

            back.Enabled = true;
            forward.Enabled = true;

            selectFile.Enabled = true;
            accept.Enabled = true;
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaverForm_Load(object sender, EventArgs e)
        {
            Parameter[] parameters = app.Saver.Parameters;
            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    InsertParameter(item);
                }
            }

            if (app.Saver.State == State.Running)
            {
                Saver_OnStart(app.Saver, EventArgs.Empty);
            }

            if (app.Saver.File != null && app.Saver.File.IsLoaded)
            {
                textBoxFile.Text = app.Saver.File.URI;
            }
        }

        /// <summary>
        /// Добавить параметр в список
        /// </summary>
        /// <param name="parameter">Добавляемый параметр</param>
        private void InsertParameter(Parameter parameter)
        {
            ListViewItem item = new ListViewItem(listViewParameters.Items.Count.ToString());
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, parameter.Description);

            item.SubItems.Add(desc);
            item.Tag = parameter;

            if (!parameter.IsActual)
            {
                item.BackColor = Color.Salmon;
            }

            listViewParameters.Items.Add(item);

        }

        /// <summary>
        /// выгружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaverForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.Saver.OnStart -= Saver_OnStart;
            app.Saver.OnStop -= Saver_OnStop;
        }

        /// <summary>
        /// Выбираем файл для сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                uri = saveFileDialog.FileName;
                textBoxFile.Text = uri;
            }
        }

        /// <summary>
        /// Выбрать параметры для сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void select_Click(object sender, EventArgs e)
        {
            SelectForm frm = new SelectForm(app);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Parameter[] parameters = frm.Parameters;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        InsertParameter(parameter);
                    }
                }
            }
        }

        /// <summary>
        /// Удаляем параметр
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, EventArgs e)
        {
            if (listViewParameters.SelectedItems != null)
            {
                if (listViewParameters.SelectedItems.Count > 0)
                {
                    //foreach (int selectedIndex in listViewParameters.SelectedIndices)
                    {
                        listViewParameters.Items.Remove(listViewParameters.SelectedItems[0]);
                        CorrectIndexes();
                    }
                }
            }
        }

        /// <summary>
        /// Скорректировать индексы
        /// </summary>
        private void CorrectIndexes()
        {
            int index = 0;
            foreach (ListViewItem item in listViewParameters.Items)
            {
                item.SubItems[0].Text = index.ToString();
                index = index + 1;
            }
        }

        /// <summary>
        /// Проверяем на корректность и если все нормально, 
        /// то выполняем создание и разметку файла и добавляем параметры для сохранения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void accept_Click(object sender, EventArgs e)
        {
            if (listViewParameters.Items.Count > 0)
            {
                if (textBoxFile.Text != string.Empty)
                {
                    app.Saver.Clear();
                    int p_count = 0;

                    foreach (ListViewItem item in listViewParameters.Items)
                    {
                        if (item.Tag != null)
                        {
                            if (item.Tag is Parameter)
                            {
                                app.Saver.Insert(item.Tag as Parameter);
                                p_count = p_count + 1;
                            }
                        }
                    }

                    app.Saver.File.Close();
                    File.CreateNewFile(textBoxFile.Text, p_count);

                    app.Saver.File.Load(textBoxFile.Text);

                    Close();
                }
            }
        }

        /// <summary>
        /// сместить вниз
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void forward_Click(object sender, EventArgs e)
        {
            if (app.State == State.Running) return;
            if (listViewParameters.Items.Count > 1)
            {
                if (listViewParameters.SelectedItems != null)
                {
                    if (listViewParameters.SelectedItems.Count > 0)
                    {
                        ListViewItem selected = listViewParameters.SelectedItems[0];
                        int selected_index = selected.Index;

                        if (selected_index < listViewParameters.Items.Count - 1)
                        {
                            ListViewItem s_clone = listViewParameters.Items[selected_index].Clone() as ListViewItem;
                            ListViewItem d_clone = listViewParameters.Items[selected_index + 1].Clone() as ListViewItem;

                            if (s_clone != null && d_clone != null)
                            {
                                listViewParameters.Items[selected_index] = d_clone;
                                listViewParameters.Items[selected_index + 1] = s_clone;

                                listViewParameters.Items[selected_index + 1].Selected = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// сместить верх
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void back_Click(object sender, EventArgs e)
        {
            if (app.State == State.Running) return;
            if (listViewParameters.Items.Count > 1)
            {
                if (listViewParameters.SelectedItems != null)
                {
                    if (listViewParameters.SelectedItems.Count > 0)
                    {
                        ListViewItem selected = listViewParameters.SelectedItems[0];
                        int selected_index = selected.Index;

                        if (selected_index > 0)
                        {
                            ListViewItem s_clone = listViewParameters.Items[selected_index].Clone() as ListViewItem;
                            ListViewItem d_clone = listViewParameters.Items[selected_index - 1].Clone() as ListViewItem;

                            if (s_clone != null && d_clone != null)
                            {
                                listViewParameters.Items[selected_index] = d_clone;
                                listViewParameters.Items[selected_index - 1] = s_clone;

                                listViewParameters.Items[selected_index - 1].Selected = true;
                            }
                        }
                    }
                }
            }
        }
    }
}