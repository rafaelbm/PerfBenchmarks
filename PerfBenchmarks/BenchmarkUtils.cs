using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfBenchmarks
{
    public class SimpleClass
    {
        public int ID { get; set; }
        public int AnotherValue { get; set; }
        public string StringValue { get; set; }

        public SimpleClass(int id)
        {
            ID = id;
            AnotherValue = id;
            StringValue = "StringValue" + id;
        }
    }

    public static class BenchmarkUtils
    {
        public static DataTable GetSampleDataTable(int itemsNumber = 0)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("AnotherValue", typeof(int));
            dataTable.Columns.Add("StringValue", typeof(string));

            for (int i = 0; i < itemsNumber; i++)
            {
                var newRow = dataTable.NewRow();
                newRow["ID"] = i;
                newRow["AnotherValue"] = i;
                newRow["StringValue"] = "StringValue" + i;

                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }

        public static Dictionary<int, SimpleClass> GetSampleDictionary(int itemsNumber = 0)
        {
            return Enumerable.Range(0, itemsNumber)
                 .Select(i => new SimpleClass(i))
                 .ToDictionary(key => key.ID, value => value);
        }

        public static ConcurrentDictionary<int, SimpleClass> GetSampleConcurrentDictionary(int itemsNumber = 0)
        {
            return new ConcurrentDictionary<int, SimpleClass>(GetSampleDictionary(itemsNumber));
        }
    }
}
