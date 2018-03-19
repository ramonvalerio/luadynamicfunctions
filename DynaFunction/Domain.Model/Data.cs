using System;
using System.Collections.Generic;
using System.Data;

namespace DynaFunction.Domain.Model
{
    public class Data
    {
        public List<DateTime> X { get; set; }
        public List<double?> Y { get; set; }

        public Data()
        {
            X = new List<DateTime>();
            Y = new List<double?>();
        }

        public void AddData(DateTime date, double? value)
        {
            X.Add(date);
            Y.Add(value);
        }

        /// <summary>
        /// É necessário uma coluna com nome DATE
        /// É necessário uma coluna com nome VALUE
        /// </summary>
        /// <param name="dataTable"></param>
        public void InsertData(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var date = Convert.ToDateTime(dt.Rows[i]["DATE"]);
                var value = Convert.ToDouble(dt.Rows[i]["VALUE"]);

                X.Add(date);
                Y.Add(value);
            }
        }

        public void RollByDate()
        {
            //Values = Values.OrderBy(x => x.Date).ToList();
        }

        public void RollByValue()
        {
            //Values = Values.OrderBy(x => x.Value).ToList();
        }
    }
}