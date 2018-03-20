using System.Collections.Generic;
using System.Data;

namespace DynaFunction.Core.Domain.Model
{
    public class Data
    {
        public List<dynamic> X { get; private set; }
        public List<dynamic> Y { get; private set; }

        public Data()
        {
            X = new List<dynamic>();
            Y = new List<dynamic>();
        }

        public void AddData(dynamic date, dynamic value)
        {
            X.Add(date);
            Y.Add(value);
        }

        /// <summary>
        /// É necessário uma coluna com nome X
        /// É necessário uma coluna com nome Y
        /// </summary>
        /// <param name="dataTable"></param>
        public void InsertData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                X.Add(dt.Rows[i]["X"]);
                Y.Add(dt.Rows[i]["Y"]);
            }
        }
    }
}