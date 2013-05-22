using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DeviceManager
{
    public partial class ConverterForm : Form
    {
        private Application app = null;

        public ConverterForm(Application _app)
        {
            app = _app;
            InitializeComponent();
        }

        /// <summary>
        /// Формула более не актуальна
        /// </summary>
        /// <param name="formula"></param>
        private void Converter_OnNotActualFormula(Formula formula)
        {
            foreach (ListViewItem item in listViewFormuls.Items)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Formula)
                    {
                        Formula iformula = item.Tag as Formula;

                        if (iformula.Position == formula.Position)
                        {
                            item.BackColor = Color.Salmon;
                            break;
                        }
                    }
                }
            }          
        }

        /// <summary>
        /// Загружаемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConverterForm_Load(object sender, EventArgs e)
        {
            foreach (var formula in app.Converter.Formuls)
            {
                InsertFormula(formula, false);
            }
        }

        /// <summary>
        /// Добавляем формулу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insert_formula_Click(object sender, EventArgs e)
        {
            switch (comboBoxFormuls.SelectedIndex)
            {
                case 0:

                    AddConstantForm constFrm = new AddConstantForm(app);
                    if (constFrm.ShowDialog(this) == DialogResult.OK)
                    {
                        Formula const_formula = new Formula();

                        Constant constant = new Constant();
                        constant.Value = constFrm.Value;

                        const_formula.Macros = constant;

                        if (constFrm.AutoSetNumber)
                        {
                            const_formula.Position = app.Converter.GetFreeChannel();
                        }
                        else
                        {
                            const_formula.Position = constFrm.Number;
                        }

                        const_formula.Macros.Description = constFrm.Comment;

                        InsertFormula(const_formula, true);
                        app.Converter.InsertFormula(const_formula);
                    }
                    break;

                case 1:

                    AddAssignmentForm assig_frm = new AddAssignmentForm(app);
                    if (assig_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Formula assig = new Formula();
                        Assignment assignment = new Assignment();

                        assignment.Args[0].Index = assig_frm.Position;
                        assignment.Args[0].Source = DataSource.Signals;

                        assig.Macros = assignment;
                        if (assig_frm.AutoSetNumber)
                        {
                            assig.Position = app.Converter.GetFreeChannel();
                        }
                        else
                        {
                            assig.Position = assig_frm.Number;
                        }

                        assig.Macros.Description = assig_frm.Comment;

                        InsertFormula(assig, true);
                        app.Converter.InsertFormula(assig);
                    }
                    break;

                case 2:

                    AddSummaNewForm summa_frm = new AddSummaNewForm(app);
                    //AddSummaForm summa_frm = new AddSummaForm(app);
                    if (summa_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (summa_frm.FirstArg != null && summa_frm.SecondtArg != null)
                        {
                            Summa summa = new Summa();
                            Formula summ_formula = new Formula();                            

                            summa.Args[0] = summa_frm.FirstArg;
                            summa.Args[1] = summa_frm.SecondtArg;

                            summ_formula.Macros = summa;

                            if (summa_frm.AutoSetNumber)
                            {
                                summ_formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                summ_formula.Position = summa_frm.Number;
                            }

                            summ_formula.Macros.Description = summa_frm.Comment;

                            InsertFormula(summ_formula, true);
                            app.Converter.InsertFormula(summ_formula);
                        }
                    }
                    break;

                case 3:

                    //AddDifferenceForm difference_frm = new AddDifferenceForm(app);
                    AddDifferenceNewForm difference_frm = new AddDifferenceNewForm(app);

                    if (difference_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (difference_frm.FirstArg != null && difference_frm.SecondtArg != null)
                        {
                            Formula formula = new Formula();
                            Difference difference = new Difference();

                            difference.Args[0] = difference_frm.FirstArg;
                            difference.Args[1] = difference_frm.SecondtArg;

                            formula.Macros = difference;

                            if (difference_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = difference_frm.Number;
                            }
                            formula.Macros.Description = difference_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 4:

                    //AddMultiplicationForm multiplication_frm = new AddMultiplicationForm(app);
                    AddMultiplicationNewForm multiplication_frm = new AddMultiplicationNewForm(app);

                    if (multiplication_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (multiplication_frm.FirstArg != null && multiplication_frm.SecondtArg != null)
                        {
                            Formula formula = new Formula();
                            Multiplication multiplication = new Multiplication();

                            multiplication.Args[0] = multiplication_frm.FirstArg;
                            multiplication.Args[1] = multiplication_frm.SecondtArg;

                            formula.Macros = multiplication;
                            if (multiplication_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = multiplication_frm.Number;
                            }

                            formula.Macros.Description = multiplication_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 5:

                    //AddDivizionForm divizion_frm = new AddDivizionForm(app);
                    AddDivizionNewForm divizion_frm = new AddDivizionNewForm(app);
                    if (divizion_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (divizion_frm.FirstArg != null && divizion_frm.SecondtArg != null)
                        {
                            Formula formula = new Formula();
                            Division divizion = new Division();

                            divizion.Args[0] = divizion_frm.FirstArg;
                            divizion.Args[1] = divizion_frm.SecondtArg;

                            formula.Macros = divizion;

                            if (divizion_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = divizion_frm.Number;
                            }

                            formula.Macros.Description = divizion_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 6:

                    AddIncrementForm increment_frm = new AddIncrementForm(app);
                    if (increment_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (increment_frm.Position > -1)
                        {
                            Formula formula = new Formula();
                            Increment increment = new Increment();

                            increment.Args[0].Index = increment_frm.Position;
                            increment.Args[0].Source = DataSource.Results;

                            formula.Macros = increment;

                            if (increment_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = increment_frm.Number;
                            }

                            formula.Macros.Description = increment_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 7:

                    AddMaximumForm maximum_frm = new AddMaximumForm(app);
                    if (maximum_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (maximum_frm.Position > -1)
                        {
                            Formula formula = new Formula();
                            Maximum maximum = new Maximum();

                            maximum.Args[0].Index = maximum_frm.Position;
                            maximum.Args[0].Source = DataSource.Results;

                            formula.Macros = maximum;

                            if (maximum_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = maximum_frm.Number;
                            }
                            formula.Macros.Description = maximum_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 8:

                    AddMinimumForm minimum_frm = new AddMinimumForm(app);
                    if (minimum_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (minimum_frm.Position > -1)
                        {
                            Formula formula = new Formula();
                            Minimum minimum = new Minimum();

                            minimum.Args[0].Index = minimum_frm.Position;
                            minimum.Args[0].Source = DataSource.Results;

                            formula.Macros = minimum;
                            if (minimum_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = minimum_frm.Number;
                            }
                            formula.Macros.Description = minimum_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 9:

                    Add10PowXForm powX_frm = new Add10PowXForm(app);
                    if (powX_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (powX_frm.Position > -1)
                        {
                            Formula formula = new Formula();
                            PowerOf10 powX = new PowerOf10();

                            powX.Args[0].Index = powX_frm.Position;
                            powX.Args[0].Source = DataSource.Results;

                            formula.Macros = powX;
                            if (powX_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = powX_frm.Number;
                            }

                            formula.Macros.Description = powX_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 10:

                    AddAccumulationForm accumulation_frm = new AddAccumulationForm(app);
                    if (accumulation_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (accumulation_frm.Position > -1)
                        {
                            Formula formula = new Formula();
                            Accumulation accumulation = new Accumulation();

                            accumulation.Args[0].Index = accumulation_frm.Position;
                            accumulation.Args[0].Source = DataSource.Results;

                            formula.Macros = accumulation;
                            if (accumulation_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = accumulation_frm.Number;
                            }

                            formula.Macros.Description = accumulation_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 11:

                    AddMediaNewForm media_frm = new AddMediaNewForm(app);
                    if (media_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (media_frm.FirstArg != null && media_frm.SecondtArg != null)
                        {
                            Media media = new Media();
                            Formula formula = new Formula();

                            media.Args[0] = media_frm.FirstArg;
                            media.Args[1] = media_frm.SecondtArg;

                            formula.Macros = media;

                            if (media_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = media_frm.Number;
                            }

                            formula.Macros.Description = media_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }
                    break;

                case 12:

                    AddTransformationForm transform_frm = new AddTransformationForm(app);
                    if (transform_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (transform_frm.FirstArg != null && transform_frm.Transformation != null )
                        {
                            Transformation transformation = transform_frm.Transformation;
                            Formula formula = new Formula();

                            transformation.Args[0] = transform_frm.FirstArg;
                            formula.Macros = transformation;

                            if (transform_frm.AutoSetNumber)
                            {
                                formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                formula.Position = transform_frm.Number;
                            }

                            formula.Macros.Description = transform_frm.Comment;

                            InsertFormula(formula, true);
                            app.Converter.InsertFormula(formula);
                        }
                    }

                    break;

                case 13:

                    AddCaptureForm captureFrm = new AddCaptureForm(app);
                    if (captureFrm.ShowDialog(this) == DialogResult.OK)
                    {
                        Formula capture_formula = new Formula();
                        Capture capture = new DeviceManager.Capture();

                        capture_formula.Macros = capture;

                        if (captureFrm.AutoSetNumber)
                        {
                            capture_formula.Position = app.Converter.GetFreeChannel();
                        }
                        else
                        {
                            capture_formula.Position = captureFrm.Number;
                        }

                        capture_formula.Macros.Description = captureFrm.Comment;

                        InsertFormula(capture_formula, true);
                        app.Converter.InsertFormula(capture_formula);
                    }

                    break;

                case 14:

                    GasesForm frm = new GasesForm(app);
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (frm.Gases != null)
                        {
                            Formula gases_formula = new Formula();
                            Gases gases = frm.Gases;

                            gases_formula.Macros = gases;

                            if (frm.AutoSetNumber)
                            {
                                gases_formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                            {
                                gases_formula.Position = frm.Number;
                            }

                            gases_formula.Macros.Description = frm.Comment;

                            InsertFormula(gases_formula, true);
                            app.Converter.InsertFormula(gases_formula);
                        }
                    }
                    break;

                case 15:

                    ScriptForm scriptForm = new ScriptForm();
                    if (scriptForm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (scriptForm.Script != null)
                        {
                            Formula script_formula = new Formula();
                            Script script = scriptForm.Script;

                            script_formula.Macros = script;

                            if (scriptForm.AutoSetNumber)
                            {
                                script_formula.Position = app.Converter.GetFreeChannel();
                            }
                            else
                                script_formula.Position = scriptForm.Number;

                            script_formula.Macros.Description = scriptForm.Comment;
                            
                            InsertFormula(script_formula, true);
                            app.Converter.InsertFormula(script_formula);
                        }
                    }

                    break;

                default:

                    break;
            }

            listViewFormuls.ListViewItemSorter = new ListViewItemComparer(0);
        }

        /// <summary>
        /// Добавить формулу в список
        /// </summary>
        /// <param name="formula">Добавляемая формула</param>
        /// <param name="ensureVisible">J,tcgtxbnm gjrfp 'ktvtynf bkb ytn</param>
        private void InsertFormula(Formula formula, bool ensureVisible)
        {
            ListViewItem item = new ListViewItem(formula.Position.ToString());

            ListViewItem.ListViewSubItem forml = new ListViewItem.ListViewSubItem(item, formula.Macros.Name);
            ListViewItem.ListViewSubItem desc = new ListViewItem.ListViewSubItem(item, formula.Macros.Description);

            ListViewItem.ListViewSubItem descF = new ListViewItem.ListViewSubItem(item, formula.Macros.Arguments);

            item.SubItems.Add(forml);
            item.SubItems.Add(desc);
            item.SubItems.Add(descF);

            if (!formula.IsActual)
            {
                item.BackColor = Color.Salmon;
            }

            item.Tag = formula;
            listViewFormuls.Items.Add(item);

            if (ensureVisible)
            {
                item.EnsureVisible();
                item.Selected = true;
            }
        }

        /// <summary>
        /// Обнивить содержание формулы
        /// </summary>
        /// <param name="selected">Обновляемый элемент</param>
        /// <param name="formula">Формула для обновления</param>
        private void UpdateFormula(ListViewItem selected, Formula formula)
        {
            if (selected != null && formula != null)
            {
                selected.SubItems[0].Text = formula.Position.ToString();
                
                selected.SubItems[1].Text = formula.Macros.Name;
                selected.SubItems[2].Text = formula.Macros.Description;

                selected.SubItems[3].Text = formula.Macros.Arguments;


                selected.BackColor = SystemColors.Window;
                formula.IsActual = true;
            }
        }

        /// <summary>
        /// Удаляем формулу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_formula_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewFormuls.SelectedItems)
            {
                if (item.Tag != null)
                {
                    if (item.Tag is Formula)
                    {
                        app.Converter.RemoveFormula(item.Tag as Formula);
                        listViewFormuls.Items.Remove(item);
                    }
                }
            }

            listViewFormuls.ListViewItemSorter = new ListViewItemComparer(0);
        }

        /// <summary>
        /// Редактируем формулу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edit_formula_Click(object sender, EventArgs e)
        {
            if (listViewFormuls.SelectedItems != null)
            {
                if (listViewFormuls.SelectedItems.Count > 0)
                {
                    if (listViewFormuls.SelectedItems[0].Tag is Formula)
                    {
                        ListViewItem selected = listViewFormuls.SelectedItems[0];
                        Formula edited = listViewFormuls.SelectedItems[0].Tag as Formula;

                        EditFormula(selected, edited);
                    }
                }
            }

            app.Converter.SortFormuls();
            listViewFormuls.ListViewItemSorter = new ListViewItemComparer(0);
        }

        /// <summary>
        /// Редактировать формулу
        /// </summary>
        /// <param name="Selected">Выборанный элемент в сиписке</param>
        /// <param name="Edit">Редактируемая формула</param>
        private void EditFormula(ListViewItem Selected, Formula Edit)
        {
            switch (Edit.Type)
            {
                case FormulaType.Constant:

                    AddConstantForm const_frm = new AddConstantForm(app);

                    const_frm.Value = Edit.Macros.Value;
                    const_frm.Comment = Edit.Macros.Description;

                    const_frm.checkBox1.Visible = false;
                    const_frm.Number = Edit.Position;

                    if (const_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Edit.Position = const_frm.Number;
                        Edit.Macros.Value = const_frm.Value;

                        Edit.Macros.Description = const_frm.Comment;                        

                        UpdateFormula(Selected, Edit);
                    }
                    break;

                case FormulaType.Assignment:

                    AddAssignmentForm assignment_frm = new AddAssignmentForm(app);

                    assignment_frm.Comment = Edit.Macros.Description;
                    
                    assignment_frm.Number = Edit.Position;
                    assignment_frm.Position = Edit.Macros.Args[0].Index;

                    assignment_frm.checkBox1.Visible = false;

                    if (assignment_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Edit.Position = assignment_frm.Number;

                        Edit.Macros.Args[0].Index = assignment_frm.Position;
                        Edit.Macros.Args[0].Source = DataSource.Signals;

                        Edit.Macros.Description = assignment_frm.Comment;
                        UpdateFormula(Selected, Edit);
                    }
                    break;

                case FormulaType.Summa:

                    AddSummaNewForm summa_frm = new AddSummaNewForm(app);

                    summa_frm.FirstArg = Edit.Macros.Args[0];
                    summa_frm.SecondtArg = Edit.Macros.Args[1];

                    summa_frm.Comment = Edit.Macros.Description;
                    summa_frm.checkBox1.Visible = false;

                    summa_frm.Number = Edit.Position;

                    if (summa_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (summa_frm.FirstArg != null && summa_frm.SecondtArg != null)
                        {
                            Edit.Position = summa_frm.Number;

                            Edit.Macros.Args[0] = summa_frm.FirstArg;
                            Edit.Macros.Args[1] = summa_frm.SecondtArg;

                            Edit.Macros.Description = summa_frm.Comment;
                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Difference:

                    AddDifferenceNewForm difference_frm = new AddDifferenceNewForm(app);

                    difference_frm.FirstArg = Edit.Macros.Args[0];
                    difference_frm.SecondtArg = Edit.Macros.Args[1];

                    difference_frm.Comment = Edit.Macros.Description;
                    difference_frm.checkBox1.Visible = false;

                    difference_frm.Number = Edit.Position;

                    if (difference_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (difference_frm.FirstArg != null && difference_frm.SecondtArg != null)
                        {
                            Edit.Position = difference_frm.Number;

                            Edit.Macros.Args[0] = difference_frm.FirstArg;
                            Edit.Macros.Args[1] = difference_frm.SecondtArg;

                            Edit.Macros.Description = difference_frm.Comment;
                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Division:

                    AddDivizionNewForm divizion_frm = new AddDivizionNewForm(app);

                    divizion_frm.FirstArg = Edit.Macros.Args[0];
                    divizion_frm.SecondtArg = Edit.Macros.Args[1];

                    divizion_frm.Comment = Edit.Macros.Description;
                    divizion_frm.checkBox1.Visible = false;

                    divizion_frm.Number = Edit.Position;

                    if (divizion_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (divizion_frm.FirstArg != null && divizion_frm.SecondtArg != null)
                        {
                            Edit.Position = divizion_frm.Number;

                            Edit.Macros.Args[0] = divizion_frm.FirstArg;
                            Edit.Macros.Args[1] = divizion_frm.SecondtArg;

                            Edit.Macros.Description = divizion_frm.Comment;
                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Multiplication:

                    AddMultiplicationNewForm multiplication_frm = new AddMultiplicationNewForm(app);

                    multiplication_frm.FirstArg = Edit.Macros.Args[0];
                    multiplication_frm.SecondtArg = Edit.Macros.Args[1];

                    multiplication_frm.Comment = Edit.Macros.Description;
                    multiplication_frm.checkBox1.Visible = false;

                    multiplication_frm.Number = Edit.Position;

                    if (multiplication_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (multiplication_frm.FirstArg != null && multiplication_frm.SecondtArg != null)
                        {
                            Edit.Position = multiplication_frm.Number;

                            Edit.Macros.Args[0] = multiplication_frm.FirstArg;
                            Edit.Macros.Args[1] = multiplication_frm.SecondtArg;

                            Edit.Macros.Description = multiplication_frm.Comment;
                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Increment:

                    AddIncrementForm increment_frm = new AddIncrementForm(app);

                    increment_frm.Position = Edit.Macros.Args[0].Index;
                    increment_frm.Comment = Edit.Macros.Description;

                    increment_frm.checkBox1.Visible = false;
                    increment_frm.Number = Edit.Position;

                    if (increment_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (increment_frm.Position > -1)
                        {
                            Edit.Position = increment_frm.Number;

                            Edit.Macros.Args[0].Index = increment_frm.Position;
                            Edit.Macros.Description = increment_frm.Comment;                            

                            UpdateFormula(Selected, Edit);                            
                            (Edit.Macros as Increment).Reset();
                        }
                    }

                    break;

                case FormulaType.Maximum:

                    AddMaximumForm maximum_frm = new AddMaximumForm(app);

                    maximum_frm.Position = Edit.Macros.Args[0].Index;
                    maximum_frm.Comment = Edit.Macros.Description;

                    maximum_frm.checkBox1.Visible = false;
                    maximum_frm.Number = Edit.Position;

                    if (maximum_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (maximum_frm.Position > -1)
                        {
                            Edit.Position = maximum_frm.Number;

                            Edit.Macros.Args[0].Index = maximum_frm.Position;
                            Edit.Macros.Description = maximum_frm.Comment;

                            UpdateFormula(Selected, Edit);
                            Edit.Macros.Reset();
                        }
                    }
                    break;

                case FormulaType.Minimum:

                    AddMinimumForm minimum_frm = new AddMinimumForm(app);

                    minimum_frm.Position = Edit.Macros.Args[0].Index;
                    minimum_frm.Comment = Edit.Macros.Description;

                    minimum_frm.checkBox1.Visible = false;
                    minimum_frm.Number = Edit.Position;
                    
                    if (minimum_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (minimum_frm.Position > -1)
                        {
                            Edit.Position = minimum_frm.Number;

                            Edit.Macros.Args[0].Index = minimum_frm.Position;
                            Edit.Macros.Description = minimum_frm.Comment;

                            UpdateFormula(Selected, Edit);
                            Edit.Macros.Reset();
                        }
                    }
                    break;

                case FormulaType.PowerOf10:

                    Add10PowXForm powX_frm = new Add10PowXForm(app);

                    powX_frm.Position = Edit.Macros.Args[0].Index;
                    powX_frm.Comment = Edit.Macros.Description;

                    powX_frm.checkBox1.Visible = false;
                    powX_frm.Number = Edit.Position;

                    if (powX_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (powX_frm.Position > -1)
                        {
                            Edit.Position = powX_frm.Number;

                            Edit.Macros.Args[0].Index = powX_frm.Position;
                            Edit.Macros.Description = powX_frm.Comment;

                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Accumulation:

                    AddAccumulationForm accumulation_frm = new AddAccumulationForm(app);

                    accumulation_frm.Position = Edit.Macros.Args[0].Index;
                    accumulation_frm.Comment = Edit.Macros.Description;

                    accumulation_frm.checkBox1.Visible = false;
                    accumulation_frm.Number = Edit.Position;

                    if (accumulation_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (accumulation_frm.Position > -1)
                        {
                            Edit.Position = accumulation_frm.Number;

                            Edit.Macros.Args[0].Index = accumulation_frm.Position;
                            Edit.Macros.Description = accumulation_frm.Comment;

                            UpdateFormula(Selected, Edit);
                            Edit.Macros.Reset();
                        }
                    }

                    break;

                case FormulaType.Media:

                    AddMediaNewForm media_frm = new AddMediaNewForm(app);

                    media_frm.FirstArg = Edit.Macros.Args[0];
                    media_frm.SecondtArg = Edit.Macros.Args[1];

                    media_frm.Comment = Edit.Macros.Description;
                    media_frm.checkBox1.Visible = false;

                    media_frm.Number = Edit.Position;

                    if (media_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        if (media_frm.FirstArg != null && media_frm.SecondtArg != null)
                        {
                            Edit.Position = media_frm.Number;

                            Edit.Macros.Args[0] = media_frm.FirstArg;
                            Edit.Macros.Args[1] = media_frm.SecondtArg;

                            Edit.Macros.Description = media_frm.Comment;
                            UpdateFormula(Selected, Edit);
                        }
                    }

                    break;

                case FormulaType.Tranformation:

                    AddTransformationForm transform_frm = new AddTransformationForm(app);

                    transform_frm.FirstArg = Edit.Macros.Args[0];
                    transform_frm.Comment = Edit.Macros.Description;

                    transform_frm.checkBoxSetNumberAuto.Visible = false;
                    transform_frm.Number = Edit.Position;

                    if (Edit.Macros is Transformation)
                    {
                        Transformation temp = Edit.Macros as Transformation;

                        Transformation t_clone = new Transformation();
                        foreach (Transformation.TCondition condition in temp.Table)
                        {
                            Transformation.TCondition t_condition = new Transformation.TCondition();

                            t_condition.Multy = condition.Multy;
                            t_condition.Result = condition.Result;

                            t_condition.Shift = condition.Shift;
                            t_condition.Signal = condition.Signal;

                            t_clone.Insert(t_condition);
                        }

                        t_clone.Args[0].Source = temp.Args[0].Source;
                        t_clone.Args[0].Index = temp.Args[0].Index;

                        t_clone.Description = temp.Description;
                        transform_frm.Transformation = t_clone;

                        if (transform_frm.ShowDialog(this) == DialogResult.OK)
                        {
                            Edit.Position = transform_frm.Number;

                            Edit.Macros = transform_frm.Transformation;
                            Edit.Macros.Description = transform_frm.Comment;

                            UpdateFormula(Selected, Edit);
                        }
                    }
                    break;

                case FormulaType.Capture:

                    AddCaptureForm cap_frm = new AddCaptureForm(app);

                    cap_frm.Value = Edit.Macros.Value;
                    cap_frm.Comment = Edit.Macros.Description;

                    cap_frm.checkBox1.Visible = false;
                    cap_frm.Number = Edit.Position;

                    if (cap_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Edit.Position = cap_frm.Number;
                        Edit.Macros.Value = cap_frm.Value;

                        Edit.Macros.Description = cap_frm.Comment;                        

                        UpdateFormula(Selected, Edit);
                    }
                    break;

                case FormulaType.Gases:

                    GasesForm gas_frm = new GasesForm(app);

                    gas_frm.Comment = Edit.Macros.Description;

                    gas_frm.checkBox1.Visible = true;
                    gas_frm.Number = Edit.Position;

                    gas_frm.Gases = Edit.Macros as Gases;

                    if (gas_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Edit.Position = gas_frm.Number;
                        Edit.Macros.Description = gas_frm.Comment;                        

                        UpdateFormula(Selected, Edit);
                    }
                    break;

                case FormulaType.Script:

                    ScriptFormEdit scr_frm = new ScriptFormEdit(Edit.Macros as Script);

                    scr_frm.Comment = Edit.Macros.Description;
                    scr_frm.checkBox1.Visible = true;

                    scr_frm.Number = Edit.Position;

                    if (scr_frm.ShowDialog(this) == DialogResult.OK)
                    {
                        Edit.Position = scr_frm.Number;
                        Edit.Macros.Description = scr_frm.Comment;

                        UpdateFormula(Selected, Edit);

                        Script script = Edit.Macros as Script;
                        if (script != null)
                        {
                            script.Reset();
                        }
                    }
                    break;

                default:

                    break;
            }
        }

        /// <summary>
        /// закрываемся
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConverterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }

    /// <summary>
    /// Реализует сортировку 
    /// </summary>
    class ListViewItemComparer : IComparer
    {
        private int col;                    // столбец по которому сортировать

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        public ListViewItemComparer()
        {
            col = 0;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса
        /// </summary>
        /// <param name="column">Номер столбца по которому выполнять сортировку</param>
        public ListViewItemComparer(int column)
        {
            col = column;
        }

        /// <summary>
        /// Сравнить объекты
        /// </summary>
        /// <param name="x">первый объект</param>
        /// <param name="y">второй объект</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            ListViewItem first = x as ListViewItem;//.SubItems[col].Text
            ListViewItem second = y as ListViewItem;//.SubItems[col].Text

            try
            {
                int f_int = int.Parse(first.SubItems[col].Text);
                int s_int = int.Parse(second.SubItems[col].Text);

                int z = f_int - s_int;
                if (z == 0)
                    return 0;
                else
                    if (z < 0)
                        return -1;
                    else
                        return 1;
            }
            catch
            {
            }

            return 0;
        }
    }
}