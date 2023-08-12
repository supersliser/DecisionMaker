using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Security.Policy;
using System.Windows.Forms;

namespace Actual_Decision_Maker
{
    internal static class Program
    {
        static Table table;

        [STAThread]
        static void Main()
        {
            /// <summary>
            /// The main entry point for the application.
            /// </summary>
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //int startMenuOption = DisplayStartMenu();

            //switch (startMenuOption)
            //{
            //    case 1:
            //        CreateNewTable();
            //        EditTableMenu();
            //        break;
            //}
        }

        private static int DisplayStartMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Decision Maker");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1.Create new table");
            Console.WriteLine("2.Open previous table");
            Console.WriteLine("3.Exort to excel");
            Console.WriteLine("4.Exit");
            var input = Console.ReadLine();
            while (input != "1" || input != "2" || input != "3" || input != "4")
            {
                Console.WriteLine("incorrect value enterred, please try again");
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }

        private static void CreateNewTable()
        {
            Console.WriteLine("Please enter the name of your new table");
            table = new Table();
            table.name = Console.ReadLine();

            Console.WriteLine("Please enter the location of your table");
            table.location = Console.ReadLine();
        }

        private static void EditTableMenu()
        {
            Console.Clear();
            Console.WriteLine("Please select an option:");
        }
    }

    class TableTemp
    {

    }

    struct Point
    {
        public int x;
        public int y;
    }

    class Category
    {
        protected string Name;
        protected int Value;

        public string inName
        {
            set
            {
                Name = value;
            }
            get
            {
                return Name;
            }
        }

        public int inValue
        {
            set
            {
                Value = value;
            }
            get
            {
                return Value;
            }
        }
    }

    class Field
    {
        protected string Value;
        protected Quality Quality;

        public string inValue
        {
            set
            {
                Value = value;
            }
            get
            {
                return Value;
            }
        }
        public int inQuality
        {
            set
            {
                Quality = (Quality)value;
            }
            get
            {
                return (int)Quality;
            }
        }
    }

    class FileBrowser
    {
        protected string FileName;
        protected string FileDirectory;

        public string inFileName
        {
            set
            {
                FileName = value;
            }
            get
            {
                return FileName;
            }
        }
        public string inFileDirectory
        {
            set
            {
                FileDirectory = value;
            }
            get
            {
                return FileDirectory;
            }
        }

        public void WriteValue(string input)
        {
            StreamWriter writer = new StreamWriter(FileDirectory + FileName);
            writer.Write(input);
        }

        public string ReadValue()
        {
            StreamReader reader = new StreamReader(FileDirectory + FileName);
            return reader.ReadToEnd();
        }
    }

    enum Quality
    {
        good = 1,
        neutral = 0,
        bad = -1
    }
}
