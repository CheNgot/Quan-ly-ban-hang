using System;
using System.Collections.Generic;
using System.Linq;
using static MilestoneOne.source.DatabaseHelper;

namespace MilestoneOne.source
{
    internal class DatabaseHelper
    {
        private readonly HealthStoreDatabaseEntities _healthStore = new HealthStoreDatabaseEntities();

        public void InsertProduct(Product product)
        {
            _healthStore.Products.Add(product);
            _healthStore.SaveChanges();
        }

        public void InsertListProducts(List<Product> products)
        {
            foreach (var product in products) InsertProduct(product);
        }

        public List<Product> GetProducts()
        {
            var query = from t in _healthStore.Products
                select t;
            var products = query.ToList();
            foreach (var product in products) product.Image = AppHelper.LoadBitmap(product.Path);

            return products;
        }

        public int GetLastIdProduct()
        {
            var products = GetProducts();
            if (products.Count == 0) return 1;
            return products[products.Count - 1].Id + 1;
        }

        public List<Product> GetProductsByType(Type type)
        {
            var query = from t in _healthStore.Products
                where t.IdType == type.IdType
                select t;
            var products = query.ToList();
            foreach (var product in products) product.Image = AppHelper.LoadBitmap(product.Path);
            return products;
        }

        public Product GetProduct(int idProduct)
        {
            var query = from t in _healthStore.Products
                        where t.Id == idProduct
                        select t;

            return query.First();
        }

        public List<Type> GetTypes()
        {
            var query = from t in _healthStore.Types
                select t;
            return query.ToList();
        }

        public List<string> GetTypesName()
        {
            var query = from t in _healthStore.Types
                select t.TypeName;
            return query.ToList();
        }

        public void InsertType(string typeName)
        {
            var type = new Type {TypeName = typeName};
            _healthStore.Types.Add(type);
            _healthStore.SaveChanges();
        }

        public void UpdateType(int id, string typename)
        {
            var type = _healthStore.Types.First(t => t.IdType == id);
            type.TypeName = typename;
            type.IdType = id;
            _healthStore.Entry(type).CurrentValues.SetValues(type);
            _healthStore.SaveChanges();
        }

        public void DeleteType(int id)
        {
            var query = from t in _healthStore.Types
                where t.IdType == id
                select t;
            foreach (var t in query) _healthStore.Types.Remove(t);

            _healthStore.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var result = _healthStore.Products.First(p => p.Id == product.Id);
            result.Name = product.Name;
            result.Path = product.Path;
            result.Cost = product.Cost;
            result.Amount = product.Amount;
            result.IdType = product.IdType;
            _healthStore.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var query = from p in _healthStore.Products
                where p.Id == id
                select p;
            foreach (var p in query) _healthStore.Products.Remove(p);
            _healthStore.SaveChanges();
        }

        public List<int> GetAllIdType()
        {
            var querry = from p in _healthStore.Types
                select p.IdType;
            return querry.ToList();
        }

        public int GetIdType(string typename)
        {
            var id = 0;
            var list = GetTypes();
            foreach (var type in list)
                if (type.TypeName == typename)
                {
                    id = type.IdType;
                    return id;
                }

            return id;
        }

        public string GetTypesNameById(int id)
        {
            var typesname = "";
            var types = GetTypes();
            {
                for (var i = 0; i < types.Count(); i++)
                    if (types[i].IdType == id)
                    {
                        typesname = types[i].TypeName;
                        return typesname;
                    }
            }

            return typesname;
        }

        public int CreateOrder()
        {
            OrderTable orderTable = new OrderTable();
            orderTable.CreatedDate = DateTime.Now;
            orderTable.Status = "Mới Tạo";
            _healthStore.OrderTables.Add(orderTable);
            _healthStore.SaveChanges();

            return orderTable.Id;
        }

        public List<OrderTable> GetOrderTables()
        {
            var query = from t in _healthStore.OrderTables
                        select t;
            List<OrderTable> orderTables = query.ToList();
            for (int i = 0; i < orderTables.Count; i++)
            {
                orderTables[i].Total = GetTotalMoneyOfOrder(orderTables[i].Id);
                orderTables[i].ProductInOrderTables = GetProductInOrderTablesByIdOrder(orderTables[i].Id);
                orderTables[i].Quantity = GetOrderQuantity(orderTables[i].Id);
            }

            return orderTables;
        }

        public void UpdateOrder(OrderTable order)
        {
            var result = _healthStore.OrderTables.First(p => p.Id == order.Id);
            result.CreatedDate = order.CreatedDate;
            result.Quantity = order.Quantity;
            result.Total = order.Total;
            result.Status = order.Status;
            _healthStore.SaveChanges();
        }

        public List<ProductInOrderTable> GetProductInOrderTablesByIdOrder(int idOrder)
        {
            var query = from t in _healthStore.ProductInOrderTables
                        where t.OrderId == idOrder
                        select t;

            return query.ToList();
        }

        public void SaveProductInOrderTable(ProductInOrderTable productInOrder)
        {
            _healthStore.ProductInOrderTables.Add(productInOrder);
            _healthStore.SaveChanges();
        }

        public void SaveListProductsInOrderTable(List<ProductInOrderTable> productInOrderTables)
        {
            for (int i = 0; i < productInOrderTables.Count; i++)
            {
                this.SaveProductInOrderTable(productInOrderTables[i]);
            }
        }

        public double GetTotalMoneyOfOrder(int idOrder)
        {
            var query = from t in _healthStore.ProductInOrderTables
                        where t.OrderId == idOrder
                        select t;
            var listIdOrders = query.ToList();
            double result = 0;
            for (int i = 0; i < listIdOrders.Count(); i++)
            {
                result += GetProduct(listIdOrders[i].ProductId).Cost * listIdOrders[i].Quantity.Value;

            }
            
            return Math.Round(result, 5); 
        }
        public void DelOrder(int id)
        {
            var query = from o in _healthStore.OrderTables
                        where o.Id == id
                        select o;
            foreach (var o in query) _healthStore.OrderTables.Remove(o);
            _healthStore.SaveChanges();
        }

        public OrderTable GetOrderTable(int idOrderTable)
        {
            var query = from o in _healthStore.OrderTables
                        where o.Id == idOrderTable
                        select o;

            OrderTable orderTable = query.First();
            orderTable.Total = GetTotalMoneyOfOrder(idOrderTable);
            orderTable.Quantity = GetOrderQuantity(idOrderTable);
            return orderTable;
        }

        public int GetOrderQuantity(int idOrder)
        {
            var query = from t in _healthStore.ProductInOrderTables
                        where t.OrderId == idOrder
                        select t;
            var listIdOrders = query.ToList();
            int  quantiy = 0;
            for (int i = 0; i < listIdOrders.Count(); i++)
            {
                quantiy += listIdOrders[i].Quantity.Value;

            }

            return quantiy;
        }

        public List<OrderTable> GetDataByDay (DateTime datefrom ,DateTime dateto)
        {
            var quer = from o in _healthStore.OrderTables
                       select o;
            var listtemp = quer.ToList();
            List<OrderTable> orderTables = new List<OrderTable>();
            for ( int i=0;i<listtemp.Count();i++)
            {
                DateTime df = DateTime.Parse(datefrom.ToShortDateString());
                DateTime dt = DateTime.Parse(dateto.ToShortDateString());
                DateTime dtemp = DateTime.Parse(listtemp[i].CreatedDate.Value.ToShortDateString());
                if(dtemp.Date>=df.Date &&dtemp.Date<=dt.Date)
                {
                    orderTables.Add(listtemp[i]);
                }
            }

            return orderTables;
        }

        public class pie
        {
            public string name { get; set; }
            public double total { get; set; }

            public bool Equals(pie x, pie y)
            {
                if (x.name == y.name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(pie obj)
            {
                return obj.name.GetHashCode();
            }
        }

        public List<pie> GetDataByProductInOrder(DateTime datefrom, DateTime dateto)
        {
            var query = from pit in _healthStore.ProductInOrderTables
                        join p in _healthStore.Products on pit.ProductId equals p.Id
                        join o in _healthStore.OrderTables on pit.OrderId equals o.Id
                        where o.CreatedDate >= datefrom && o.CreatedDate <= dateto
                        select new
                        {
                            name = p.Name,
                            Total = pit.Quantity * p.Cost
                        } into g
                        group g by g.name
                        into a
                        orderby a.Key ascending
                        select a;

            var list = query.ToList(); ;
            
            List<pie> listProductTemp = new List<pie>();
            foreach(var value in list)
            {
                var valueOfList = value.Key;
                foreach(var p in value)
                {
                    pie pie_tmp = new pie() { name = p.name, total = (double)p.Total };
                    if (!isExist(listProductTemp, pie_tmp.name))
                    {
                        listProductTemp.Add(pie_tmp);
                    }
                }
            }

            return listProductTemp;
        }

        private bool isExist(List<pie> pies, String name)
        {
            for (int i = 0; i < pies.Count; i++)
            {
                if (name == pies[i].name)
                {
                    return true;
                }
            }

            return false;
        }
      

    }
}