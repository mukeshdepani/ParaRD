using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Clauses;

namespace SQLBuilder.Common
{
    public class Function
    {
        private string name;
        private int numParameters;
        private List<Parameter> parameters = new List<Parameter>();

        public Function(string name, int numParams)
        {
            this.name = name;
            this.numParameters = numParams;
        }

        public Function(string name, int numParams, List<Parameter> parameters)
        {
            this.name = name;
            this.numParameters = numParams;
            this.parameters = parameters;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int NumParameters
        {
            get { return numParameters; }
            set { numParameters = value; }
        }

        public List<Parameter> Parameters
        {
            get { return parameters; }
        }
    }
}
