using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilestoneOne.source
{
    public class ProductInOrder
    {
        public  int ID { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }

        public static List<ProductInOrderTable> ConvertToProductInOrderTable(List<ProductInOrder> productInOrders, int idOrder)
        {
            List<ProductInOrderTable> productInOrderTables = new List<ProductInOrderTable>();
            for(int i = 0; i < productInOrders.Count; i++)
            {
                ProductInOrderTable productInOrderTable = new ProductInOrderTable();
                productInOrderTable.Id = -1;
                productInOrderTable.Name = productInOrders[i].Name;
                productInOrderTable.OrderId = idOrder;
                productInOrderTable.ProductId = productInOrders[i].ProductId;
                productInOrderTable.Quantity = productInOrders[i].Quantity;
                productInOrderTables.Add(productInOrderTable);
            }

            return productInOrderTables;
        }
    }
}
