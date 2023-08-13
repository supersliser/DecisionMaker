using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actual_Decision_Maker
{
    internal class TableIgnore
    {
        public string name;
        public string location;
        protected List<Category> columnHeaders;
        protected List<List<Field>> values;

        public int RowCount
        {
            get
            {
                return values[0].Count + 1;
            }
        }
        public int ColumnCount
        {
            get
            {
                return values.Count;
            }
        }

        public Field[] getRow(int index)
        {
            List<Field> output = new List<Field>();
            for (int i = 0; i < values.Count; i++)
            {
                output.Add(values[i][index]);
            }
            return output.ToArray();
        }
        public Field[] getColumn(int index)
        {
            return values[index].ToArray();
        }

        public void displayTable()
        {
            const string VSeparator = "|";
            const string HSeparator = "-";

            int[] cellSizes = getLongestValues();

            for (int column = 0; column < columnHeaders.Count; column++)
            {
                Console.Write(VSeparator);
                Console.Write(column);
                for (int space = 0; space < cellSizes[column] - column.ToString().Length; space++)
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine(VSeparator);

            WriteRowSeparator(cellSizes, HSeparator, VSeparator);
        }

        public void WriteRowSeparator(int[] cellSizes, string HSeparator, string VSeparator)
        {
            for (int column = 0; column < ColumnCount; column++)
            {
                    Console.Write(VSeparator);
                for (int space = 0; space < cellSizes[column]; space++)
                {
                    Console.Write(HSeparator);
                }
            }
            Console.WriteLine(VSeparator);
        }

        public int getLongestValue(int columnIndex)
        {
            int longestValue = columnHeaders[columnIndex].inName.Length;

            foreach (Field item in getColumn(columnIndex))
            {
                if (item.inValue.Length > longestValue)
                {
                    longestValue = item.inValue.Length;
                }
            }
            return longestValue;
        }

        public int[] getLongestValues()
        {
            List<int> values = new List<int>();
            values.Add(RowCount.ToString().Length);
            for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
            {
                values.Add(getLongestValue(columnIndex));
            }
            return values.ToArray();
        }

        public void WriteCell(Point point, int cellSize, string HSeparator)
        {
            Field val = getRow(point.x)[point.y];
            Console.Write(val.inValue);
            for (int i = val.inValue.Length; i < cellSize; i++)
            {
                Console.Write(" ");
            }
            Console.Write(HSeparator);
        }

        public void WriteRowStarter(int y, int cellsize, string HSeparator)
        {
            Console.Write(y);
            for (int i = y.ToString().Length; i < cellsize; i++)
            {
                Console.Write(" ");
            }
            Console.Write(HSeparator);
        }

        public void WriteRow(int y, int[] cellSizes, string HSeparator)
        {

        }
    }
}
