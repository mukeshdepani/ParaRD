using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLBuilder.Common
{
    public class ColumnFormat
    {
        public static Dictionary<string, string> formats= new Dictionary<string, string>();
        public static Dictionary<string, int> excelFormats = new Dictionary<string, int>();
    
        private static ColumnFormat instance;

        private ColumnFormat()
        {
            formats.Add("Number", "{0:0,0}");
            formats.Add("Percentage", "##.##%");
            formats.Add("Phone", "(###) ###-####");
            formats.Add("Currency", "{0:C}");
            formats.Add("Decimal(1)", "{0:0,0.0}");
            formats.Add("Decimal(2)", "{0:0,0.00}");
            formats.Add("Decimal(3)", "{0:0,0.000}");
            formats.Add("Decimal(4)", "{0:0,0.0000}");


            excelFormats.Add("Number", 11);
            excelFormats.Add("Percentage", 10);
            excelFormats.Add("Phone", 1);
            excelFormats.Add("Currency", 4);
            excelFormats.Add("Decimal(1)", 4);
            excelFormats.Add("Decimal(2)", 4);
            excelFormats.Add("Decimal(3)", 4);
            excelFormats.Add("Decimal(4)", 4);
        }

        public static ColumnFormat Instance
        {
            get 
            {
                if (instance == null)
                {
                    instance = new ColumnFormat();
                }
                return instance;
            }
        }

        public string getColumnFormat(string name)
        {
            string value = null;
            if (name != null && formats.ContainsKey(name))
            {
                value = formats[name].ToString();
            }

            return value;
        }

        public int getExcelColumnFormat(string name)
        {
            int value = 0;
            if (name != null && excelFormats.ContainsKey(name))
            {
                value = excelFormats[name] ;
            }

            return value;
        }
    }
}
