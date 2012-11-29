using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Clauses;

namespace SQLBuilder.Common
{
    public class Functions
    {
        public static List<Function> functions = new List<Function>();
        public static Dictionary<string, Function> functionsMap = new Dictionary<string, Function>();

        //public const string FUNC_Select = " ";
        public const string FUNC_CASE = "CASE";
        public const string FUNC_DATE = "DATE_FORMAT";
        public const string FUNC_IFNULL = "IFNULL";
        public const string FUNC_MIN = "MIN";
        public const string FUNC_MAX = "MAX";
        public const string FUNC_AVG = "AVG";
        public const string FUNC_SUM = "SUM";
        public const string FUNC_COUNT = "COUNT";
        //public const string FUNC_POW = "POW";
        public const string FUNC_ROUND = "ROUND";
        public const string FUNC_MOD = "MOD";
        public const string FUNC_GROUP_FIRST = "GROUP_FIRST";
        public const string FUNC_GROUP_LAST = "GROUP_LAST";

        private static Functions instance = Functions.Instance;

        private Functions()
        {
            List<Parameter> paramList = new List<Parameter>();
            Parameter param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            Function function = new Function(FUNC_MIN, 1, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_MIN, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_MAX, 1, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_MAX, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_AVG, 1, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_AVG, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_SUM, 1, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_SUM, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_COUNT, 1, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_COUNT, function);

            //paramList = new List<Parameter>();
            //param = new Parameter("Column Name", Parameter.STRING);
            //paramList.Add(param);
            //param = new Parameter("Value", Parameter.INT);
            //paramList.Add(param);
            //function = new Function(FUNC_POW, 2, paramList);
            //functions.Add(function);
            //functionsMap.Add(FUNC_POW, function);

            paramList = new List<Parameter>();
            param = new Parameter("When", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Then", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Else", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_CASE, 5, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_CASE, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Value", Parameter.INT);
            paramList.Add(param);
            function = new Function(FUNC_IFNULL, 2, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_IFNULL, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Value", Parameter.INT);
            paramList.Add(param);
            function = new Function(FUNC_ROUND, 2, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_ROUND, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Value", Parameter.INT);
            paramList.Add(param);
            function = new Function(FUNC_MOD, 2, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_MOD, function);

            paramList = new List<Parameter>();
            param = new Parameter("Column Name", Parameter.DATE);
            paramList.Add(param);
            param = new Parameter("Value", Parameter.STRING);
            paramList.Add(param);
            function = new Function(FUNC_DATE, 2, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_DATE, function);

            paramList = new List<Parameter>();
            param = new Parameter("Group Column", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Order Column", Parameter.INT);
            paramList.Add(param);
            param = new Parameter("Ref Column", Parameter.INT);
            paramList.Add(param);
            function = new Function(FUNC_GROUP_FIRST, 3, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_GROUP_FIRST, function);

            paramList = new List<Parameter>();
            param = new Parameter("Group Column", Parameter.STRING);
            paramList.Add(param);
            param = new Parameter("Order Column", Parameter.INT);
            paramList.Add(param);
            param = new Parameter("Ref Column", Parameter.INT);
            paramList.Add(param);
            function = new Function(FUNC_GROUP_LAST, 3, paramList);
            functions.Add(function);
            functionsMap.Add(FUNC_GROUP_LAST, function);

        }

        public static Functions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Functions();
                }
                return instance;
            }
        }

        public static Function getFunction(string name)
        {
            Function value = null;
            if (name != null && name !="" && functionsMap.ContainsKey(name))
            {
                value = functionsMap[name];
            }

            return value;
        }

        public static List<string> getFunctionNames()
        {
            return functionsMap.Keys.ToList();
        }

    }
}
