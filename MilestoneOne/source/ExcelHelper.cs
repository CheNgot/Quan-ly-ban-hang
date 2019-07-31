using System.Collections.Generic;
using System.Data;
using Aspose.Cells;

namespace MilestoneOne.source
{
    internal class ExcelHelper
    {
        public List<Product> ReadFileExcel(string path)
        {
            var wb = new Workbook(path);
            var worksheet = wb.Worksheets[0];
            var column = 0;
            var row = 1;
            var rows = worksheet.Cells.MaxRow;
            var range = worksheet.Cells.CreateRange(row, column, rows - row + 1, 5);
            var dataTable = range.ExportDataTable();
            var products = new List<Product>();
            foreach (DataRow a in dataTable.Rows)
            {
                var product = new Product
                {
                    Name = a.ItemArray[0].ToString(),
                    Cost = double.Parse(a.ItemArray[1].ToString()),
                    Path = a.ItemArray[2].ToString(),
                    IdType = int.Parse(a.ItemArray[3].ToString()),
                    Amount = int.Parse(a.ItemArray[4].ToString())
                };
                products.Add(product);
            }

            return products;
        }
    }
}