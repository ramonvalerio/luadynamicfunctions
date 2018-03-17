using System;
using System.Collections.Generic;
using System.Data;

namespace DynaFunction.Domain.Model
{
    public class Data
    {
        //public DateTime Date { get; private set; }
        //public double? Value { get; private set; }
        public bool IsDataCommited { get; private set; }
        public List<double?> Values { get; set; }

        public Data()
        {
            //Values = new Tuple<DateTime, double?>(DateTime.Now, null);
        }

        public void AddData(DateTime date, double? value)
        {
            if (IsDataCommited)
                return;

            //Add(new Data(date, value));
        }

        /// <summary>
        /// É necessário uma coluna com nome DATE
        /// É necessário uma coluna com nome VALUE
        /// </summary>
        /// <param name="dataTable"></param>
        public void InsertData(DataTable dt)
        {
            if (IsDataCommited)
                return;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var date = Convert.ToDateTime(dt.Rows[i]["DATE"]);
                var value = Convert.ToDouble(dt.Rows[i]["VALUE"]);
                //Values.Add(new Data(date, value));
            }
        }

        public void Commit()
        {
            IsDataCommited = true;
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