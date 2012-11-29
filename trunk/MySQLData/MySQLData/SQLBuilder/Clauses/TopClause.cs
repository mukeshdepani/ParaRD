using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLBuilder.Enums;

namespace SQLBuilder.Clauses
{
    public class TopClause
    {
        public int Quantity;
        public TopUnit Unit;

        public TopClause()
        {
        }

        public TopClause(int nr)
        {
            Quantity = nr;
            Unit = TopUnit.Records;
        }
        public TopClause(int nr, TopUnit aUnit)
        {
            Quantity = nr;
            Unit = aUnit;
        }
    }
}
