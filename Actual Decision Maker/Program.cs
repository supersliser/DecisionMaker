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
    }

    class TableTemp
    {

    }

    struct Point
    {
        public int x;
        public int y;
    }

    public class Category
    {
        protected string Name;
        protected int Value;
        protected int Type = 0;
        protected decimal FailCriteria = 0;
        protected decimal SuccessCriteria = 0;

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

        public TypeValue inType
        {
            set
            {
                Type = (int)value;
            }
            get
            {
                return (TypeValue)Type;
            }
        }

        public string stringType
        {
            set
            {
                switch (value)
                {
                    case "General":
                        Type = 0;
                        break;
                    case "Boolean":
                        Type = 1;
                        break;
                    case "Number":
                        Type = 2;
                        break;
                    case "Price":
                        Type = 3;
                        break;
                }
            }
            get
            {
                switch (Type)
                {
                    case 0:
                        return "General";
                    case 1:
                        return "Boolean";
                    case 2:
                        return "Number";
                    case 3:
                        return "Price";
                }
                return "General";
            }
        }

        public decimal failValue
        {
            set
            {
                FailCriteria = value;
            }
            get
            {
                return FailCriteria;
            }
        }

        public decimal successValue
        {
            set
            {
                SuccessCriteria = value;
            }
            get
            {
                return SuccessCriteria;
            }
        }
    }

    public class Field
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

    public enum Quality
    {
        good = 1,
        neutral = 0,
        bad = -1
    }

    public enum TypeValue
    {
        general = 0,
        boolean = 1,
        number = 2,
        price = 3,
    }
}
