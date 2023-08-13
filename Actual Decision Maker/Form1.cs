using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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

                CategoryNameTXT.Text = "";
                CategoryWorthTXT.Value = 0;

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
            if (TableViewer.Rows.Count > 0)
            {
                fields[TableViewer.SelectedCells[0].ColumnIndex][TableViewer.SelectedCells[0].RowIndex].inValue = FieldValueTXT.Text;
                TableViewer.SelectedCells[0].Value = FieldValueTXT.Text;
            }
        }

        private void TableViewer_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void TableViewer_Click_1(object sender, EventArgs e)
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

            FieldValueTXT.Focus();
        }

        private void FieldWorthTXT_ValueChanged(object sender, EventArgs e)
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

        private void deletePreviousCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableViewer.Columns.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
            fields.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
            categories.RemoveAt(TableViewer.SelectedCells[0].ColumnIndex);
        }

        private void deleteItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TableViewer.Rows.RemoveAt(TableViewer.SelectedCells[0].RowIndex);
            for (int i = 0; i < fields.Count; i++)
            {
                fields[i].RemoveAt(TableViewer.SelectedCells[0].RowIndex);
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            categories.Clear();
            fields.Clear();
            TableViewer.Columns.Clear();
            TableViewer.Rows.Clear();
            TotalScoreTable.Rows.Clear();
            TotalScoreTable.Columns.Add(new DataGridViewColumn()
            {
                Name = "TotalScore",
                HeaderText = "Total Score",
                Frozen = true
            });
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
            categories = JsonConvert.DeserializeObject<Table>(reader.ReadToEnd()).categories;
            fields = JsonConvert.DeserializeObject<Table>(reader.ReadToEnd()).fields;

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
    }

    public class Table
    {
        public List<Category> categories;
        public List<List<Field>> fields;
    }
}