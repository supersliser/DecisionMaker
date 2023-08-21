using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Actual_Decision_Maker
{
    public partial class Form1 : Form
    {
        List<Category> categories = new List<Category>();
        List<List<Field>> fields = new List<List<Field>>();

        public Form1()
        {
            InitializeComponent();
        }

        private void AddCategoryCMD_Click(object sender, EventArgs e)
        {
            if (CategoryNameTXT.Text != "")
            {
                var temp = new Category();
                temp.inName = CategoryNameTXT.Text;
                temp.inValue = (int)CategoryWorthTXT.Value;
                temp.stringType = CategoryTypeTXT.Text;
                temp.successValue = CategorySuccessTXT.Value;
                temp.failValue = CategoryFailTXT.Value;

                if (!categories.Exists(x => x.inName == temp.inName))
                {
                    TableViewer.Columns.Add(new DataGridViewColumn()
                    {
                        Name = temp.inName,
                        HeaderText = temp.inName,
                        Resizable = DataGridViewTriState.True,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                        CellTemplate = new DataGridViewTextBoxCell()
                    });

                    categories.Add(temp);
                    fields.Add(new List<Field>());
                    if (fields.Count > 1)
                    {
                        for (int i = 0; i < fields[0].Count; i++)
                        {
                            fields.Last().Add(new Field());
                        }
                    }
                }
                TableViewer.ClearSelection();

                CategoryNameTXT.Text = "";
                CategoryWorthTXT.Value = 0;
                CategoryTypeTXT.SelectedIndex = (int)TypeValue.general;
                HandleCategoryTypeChange(TypeValue.general);


                FieldValueTXT.Focus();
            }
        }

        private void AddFieldCMD_Click(object sender, EventArgs e)
        {
            TableViewer.Rows.Add();
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].Add(new Field());
            }
            TotalScoreTable.Rows.Add();
        }

        private void WorkOutTotalScores()
        {
            if (fields[0].Count > 0)
            {
                List<int> score = new List<int>();
                for (int i = 0; i < TableViewer.Rows.Count; i++)
                {
                    score.Add(0);
                    for (int j = 0; j < categories.Count; j++)
                    {
                        score[i] += fields[j][i].inQuality * categories[j].inValue;
                    }
                }
                for (int i = 0; i < score.Count; i++)
                {
                    TotalScoreTable.Rows[i].Cells[0].Value = score[i];
                }
            }
        }

        private void FieldValueTXT_TextChanged(object sender, EventArgs e)
        {
            if (FieldValueTXT.Text == "") { return; }
            if (TableViewer.Rows.Count > 0)
            {
                fields[TableViewer.SelectedCells[0].ColumnIndex][TableViewer.SelectedCells[0].RowIndex].inValue = FieldValueTXT.Text;
                TableViewer.SelectedCells[0].Value = FieldValueTXT.Text;

                CalculateValues();
            }
        }

        private void FieldWorthTXT_ValueChanged(object sender, EventArgs e)
        {
            if (TableViewer.Columns.Count > 0)
            {
                fields[TableViewer.SelectedCells[0].ColumnIndex][TableViewer.SelectedCells[0].RowIndex].inQuality = (int)FieldWorthTXT.Value;
                switch (FieldWorthTXT.Value)
                {
                    case -1:
                        TableViewer.SelectedCells[0].Style.BackColor = Color.Red;
                        break;
                    case 0:
                        TableViewer.SelectedCells[0].Style.BackColor = Color.Yellow;
                        break;
                    case 1:
                        TableViewer.SelectedCells[0].Style.BackColor = Color.Green;
                        break;

                }
                WorkOutTotalScores();
            }
        }

        private void CalculateValues()
        {
            Category category = categories[TableViewer.SelectedCells[0].ColumnIndex];
            if (category.inType != TypeValue.general)
            {
                foreach (var item in fields[TableViewer.SelectedCells[0].ColumnIndex])
                {
                    try
                    {
                        decimal ItemValue = decimal.Parse(item.inValue);

                        if (category.inType == TypeValue.price)
                        {
                            if (ItemValue >= category.failValue)
                            {
                                item.inQuality = -1;
                            }
                            else if (ItemValue <= category.successValue)
                            {
                                item.inQuality = 1;
                            }
                            else
                            {
                                item.inQuality = 0;
                            }
                            FieldWorthTXT.Value = item.inQuality;
                        }
                        else
                        {
                            if (ItemValue <= category.failValue)
                            {
                                item.inQuality = -1;
                            }
                            else if (ItemValue >= category.successValue)
                            {
                                item.inQuality = 1;
                            }
                            else
                            {
                                item.inQuality = 0;
                            }
                            FieldWorthTXT.Value = item.inQuality;
                        }
                    }
                    catch (FormatException) { }
                    catch (ArgumentNullException) { }
                }
            }
        }

        private void deletePreviousCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fields.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
            categories.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
            TableViewer.Columns.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
            if (TableViewer.ColumnCount == 0)
            {
                TotalScoreTable.Rows.RemoveAt(1);
            }
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].RemoveAt(TableViewer.SelectedCells[0].RowIndex);
            }
            TableViewer.Rows.RemoveAt(TableViewer.SelectedCells[0].RowIndex);
            TotalScoreTable.Rows.RemoveAt(TableViewer.SelectedCells[0].RowIndex);
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            categories.Clear();
            fields.Clear();
            TableViewer.Columns.Clear();
            TableViewer.Rows.Clear();
            TableViewer.Refresh();
            TotalScoreTable.Rows.Clear();
            TotalScoreTable.Refresh();
            CategoryNameTXT.Clear();
            CategoryWorthTXT.Value = 0;
            FieldValueTXT.Clear();
            FieldWorthTXT.Value = 0;
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string categoriesJSON = JsonConvert.SerializeObject(new Table()
            {
                categories = categories,
                fields = fields
            });

            StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);
            writer.Write(categoriesJSON);
            writer.Close();
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            StreamReader reader = new StreamReader(openFileDialog1.FileName);
            Table temp = JsonConvert.DeserializeObject<Table>(reader.ReadToEnd());
            categories = temp.categories;
            fields = temp.fields;

            foreach (Category category in categories)
            {
                TableViewer.Columns.Add(new DataGridViewColumn()
                {
                    Name = category.inName,
                    HeaderText = category.inName,
                    Resizable = DataGridViewTriState.True,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    CellTemplate = new DataGridViewTextBoxCell()
                });
            }


            for (int i = 0; i < fields[0].Count; i++)
            {
                TableViewer.Rows.Add();
                TotalScoreTable.Rows.Add();
                for (int j = 0; j < categories.Count; j++)
                {
                    TableViewer.Rows[i].Cells[j].Value = fields[j][i].inValue;
                }
            }

            WorkOutTotalScores();
        }

        private void CategoryNameTXT_TextChanged(object sender, EventArgs e)
        {
            if (TableViewer.SelectedCells.Count > 0)
            {
                categories[TableViewer.SelectedCells[0].ColumnIndex].inName = CategoryNameTXT.Text;
                TableViewer.SelectedCells[0].OwningColumn.Name = CategoryNameTXT.Text;
                TableViewer.SelectedCells[0].OwningColumn.HeaderText = CategoryNameTXT.Text;
            }
        }

        private void CategoryWorthTXT_ValueChanged(object sender, EventArgs e)
        {
            if (TableViewer.SelectedCells.Count > 0)
            {
                categories[TableViewer.SelectedCells[0].ColumnIndex].inValue = (int)CategoryWorthTXT.Value;
                WorkOutTotalScores();
            }
        }

        private void CategoryTypeTXT_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (categories.Where(x => x.inName == CategoryNameTXT.Text).Any())
            TypeValue SelectedType = (TypeValue)CategoryTypeTXT.SelectedIndex;
            if (TableViewer.SelectedCells.Count > 0)
            {
                categories[TableViewer.SelectedCells[0].ColumnIndex].inType = SelectedType;
                CalculateValues();
            }
            HandleCategoryTypeChange(SelectedType);

        }

        private void HandleCategoryTypeChange(TypeValue type)
        {
            switch (type)
            {
                case TypeValue.general:
                    CategoryFailTXT.Enabled = false;
                    CategoryFailTXT.Maximum = 0;
                    CategoryFailTXT.Minimum = 0;
                    CategoryFailTXT.DecimalPlaces = 0;

                    CategorySuccessTXT.Enabled = false;
                    CategorySuccessTXT.Maximum = 0;
                    CategorySuccessTXT.Minimum = 0;
                    CategorySuccessTXT.DecimalPlaces = 0;

                    FieldWorthTXT.Enabled = true;
                    break;
                case TypeValue.boolean:
                    CategoryFailTXT.Enabled = false;
                    CategoryFailTXT.Maximum = 0;
                    CategoryFailTXT.Minimum = 0;
                    CategoryFailTXT.DecimalPlaces = 0;

                    CategorySuccessTXT.Enabled = false;
                    CategorySuccessTXT.Maximum = 1;
                    CategorySuccessTXT.Minimum = 1;
                    CategorySuccessTXT.DecimalPlaces = 0;

                    FieldWorthTXT.Enabled = false;
                    break;
                case TypeValue.number:
                    CategoryFailTXT.Enabled = true;
                    CategoryFailTXT.Maximum = 100000;
                    CategoryFailTXT.Minimum = -100000;
                    CategoryFailTXT.DecimalPlaces = 3;

                    CategorySuccessTXT.Enabled = true;
                    CategorySuccessTXT.Maximum = 100000;
                    CategorySuccessTXT.Minimum = -100000;
                    CategorySuccessTXT.DecimalPlaces = 3;


                    FieldWorthTXT.Enabled = false;
                    break;
                case TypeValue.price:
                    CategoryFailTXT.Enabled = true;
                    CategoryFailTXT.Maximum = 100000;
                    CategoryFailTXT.Minimum = -100000;
                    CategoryFailTXT.DecimalPlaces = 2;

                    CategorySuccessTXT.Enabled = true;
                    CategorySuccessTXT.Maximum = 100000;
                    CategorySuccessTXT.Minimum = -100000;
                    CategorySuccessTXT.DecimalPlaces = 2;

                    FieldWorthTXT.Enabled = false;
                    break;
            }
        }

        private void CategoryFailTXT_ValueChanged(object sender, EventArgs e)
        {
            if (TableViewer.SelectedCells.Count > 0)
            {
                categories[TableViewer.SelectedCells[0].ColumnIndex].failValue = CategoryFailTXT.Value;
                CalculateValues();
            }
        }

        private void CategorySuccessTXT_ValueChanged(object sender, EventArgs e)
        {
            if (TableViewer.SelectedCells.Count > 0)
            {
                categories[TableViewer.SelectedCells[0].ColumnIndex].successValue = CategorySuccessTXT.Value;
                CalculateValues();
            }
        }

        private void TableViewer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TableViewer.SelectedCells.Count == 0)
            {
                FieldValueTXT.Text = "";
                FieldWorthTXT.Value = 0;
            }
            else
            {
                FieldValueTXT.Text = fields[TableViewer.SelectedCells[0].ColumnIndex][TableViewer.SelectedCells[0].RowIndex].inValue;
                FieldWorthTXT.Value = fields[TableViewer.SelectedCells[0].ColumnIndex][TableViewer.SelectedCells[0].RowIndex].inQuality;
            }

            CategoryNameTXT.Text = categories[TableViewer.SelectedCells[0].ColumnIndex].inName;
            CategoryWorthTXT.Value = categories[TableViewer.SelectedCells[0].ColumnIndex].inValue;
            CategoryTypeTXT.SelectedIndex = (int)categories[TableViewer.SelectedCells[0].ColumnIndex].inType;

            CategoryFailTXT.Value = categories[TableViewer.SelectedCells[0].ColumnIndex].failValue;
            CategorySuccessTXT.Value = categories[TableViewer.SelectedCells[0].ColumnIndex].successValue;

            FieldValueTXT.Focus();
        }
    }

    public class Table
    {
        public List<Category> categories;
        public List<List<Field>> fields;
    }
}