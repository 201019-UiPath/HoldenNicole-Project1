using StoreDB.Entities;
using StoreDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StoreUI
{
    public class DBRepo : ICustomerRepo, ILocationRepo, ICartRepo, IOrderRepo
    {
        private readonly ixdssaucContext context;
        private readonly IMapper mapper;

        public DBRepo()
        {
            this.context = new ixdssaucContext();
            this.mapper = new StoreMapper();
        }

        #region cart methods
        public void AddProductToCart(CartItemModel cartItem)
        {
            context.CartItems.Add(mapper.ParseCartItem(cartItem));
            context.SaveChanges();
        }
        public void UpdateCartItems(CartItemModel cartItem)
        {
            context.CartItems.Update(mapper.ParseCartItem(cartItem));
            context.SaveChanges();
        }

        public void DeleteProductInCart(CartItemModel cartItems)
        {
            context.CartItems.Remove(mapper.ParseCartItem(cartItems));
            context.SaveChanges();
        }

        public List<CartItemModel> GetAllProductsInCartByCartID(int id)
        {
            return mapper.ParseCartItem(
                context.CartItems
                .Where(i => i.Id == id)
                .ToList()
            );
        }
        public void PlaceOrder(OrderModel order)
        {
            context.Orders.First(i => i.Id == order.ID);
            context.SaveChanges();
        }

        #endregion

        #region inventory methods
        public ProductModel GetProductByID(int id)
        {
            return mapper.ParseProducts(
                (Products)context.Products
                .Where(i => i.Id == id)
                );
        }
        public List<InventoryModel> ViewAllProductsAtLocationSortByID(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Where(i => i.Location == id)
                .OrderBy(i => i.Product)
                .ToList()
            );
        }
        public List<InventoryModel> ViewAllProductsAtLocationSortByQuantityAscending(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Where(i => i.Location == id)
                .OrderBy(i => i.Quantity)
                .ToList()
            );
        }
        public List<InventoryModel> ViewAllProductsAtLocationSortByQuantityDescending(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Where(i => i.Location == id)
                .OrderByDescending(i => i.Quantity)
                .ToList()
            );
        }
        public Inventory AddProductToLocation(int locationid, int productid, int quantity)
        {
            var inventory = context.Inventory.First(i => i.Location == locationid && i.Product == productid);
            inventory.Quantity = inventory.Quantity + quantity;
            context.SaveChanges();
            return null;
        }

        public Inventory DeleteProductAtLocation(int locationid, int productid, int quantity)
        {
            var inventory = context.Inventory.First(i => i.Location == locationid && i.Product == productid);
            inventory.Quantity = inventory.Quantity - quantity;
            context.SaveChanges();
            return null;
        }
        #endregion

        #region customer methods
        public CustomerModels GetCustomerByID(int id)
        {
            try
            {
                return mapper.ParseCustomer(
                    context.Customer
                    .First(c => c.Id == id)
                );
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("This customer id does not exist try again");
            }
            return null;
        }

        public CustomerModels GetCustomerByName(string name)
        {
            try
            {
                return mapper.ParseCustomer(
                    context.Customer
                    .First(c => c.Username == name)
                );
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("This customer username does not exist try again");
            }
            return null;
        }

        public CustomerModels GetCustomerByEmail(string email)
        {
            try
            {
                return mapper.ParseCustomer(
                    context.Customer
                    .First(c => c.Email == email)
                );
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("This email is not registered try again");
            }
            return null;
        }

        public void AddCustomer(CustomerModels customer)
        {
            context.Customer.Add(mapper.ParseCustomer(customer));
            context.SaveChanges();
        }

        public List<CustomerModels> GetAllCustomersOrderByUsername()
        {
            return mapper.ParseCustomer(
                context.Customer
                .OrderBy(i => i.Username)
                .ToList()
            );
        }
        public List<CustomerModels> GetAllCustomersOrderByOrders()
        {
            return mapper.ParseCustomer(
                context.Customer
                .OrderBy(i => i.Orders)
                .ToList()
            );
        }
        public List<OrderModel> GetAllOrdersByCustomerIDDateAscending(CustomerModels customer)
        {
            try
            {
                return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Customer == customer.ID)
                    .OrderBy(c => c.OrderDate)
                    .ToList()
                );
            }
            catch (System.InvalidOperationException)
            {
                System.Console.WriteLine("This customer does not have any order history yet");
                return null;
            }
        }

        public List<OrderModel> GetAllOrdersByCustomerIDDateDescending(CustomerModels customer)
        {
            try
            {
                return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Customer == customer.ID)
                    .OrderByDescending(c => c.OrderDate)
                    .ToList()
                );
            }
            catch (System.InvalidOperationException)
            {
                System.Console.WriteLine("This customer does not have any order history yet");
                return null;
            }
        }

        public List<OrderModel> GetAllOrdersByCustomerIDPriceAscending(CustomerModels customer)
        {
            try
            {
                return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Customer == customer.ID)
                    .OrderBy(c => c.Price)
                    .ToList()
                );
            }
            catch (System.InvalidOperationException)
            {
                System.Console.WriteLine("This customer does not have any order history yet");
                return null;
            }
        }

        public List<OrderModel> GetAllOrdersByCustomerIDPriceDescending(CustomerModels customer)
        {
            try
            {
                return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Customer == customer.ID)
                    .OrderByDescending(c => c.Price)
                    .ToList()
                );
            }
            catch (System.InvalidOperationException)
            {
                System.Console.WriteLine("This customer does not have any order history yet");
                return null;
            }
        }
        #endregion

        #region location methods
        public ManagerModel GetManagerByName(string name)
        {
            return mapper.ParseManager(
                context.Managers
                .First(m => m.Username == name));
        }
        public LocationModel GetLocationByID(int id)
        {
            return mapper.ParseLocation(
                context.Locations
                .FirstOrDefault(l => l.Id == id)
            );
        }
        public LocationModel GetLocationByName(string name)
        {
            return mapper.ParseLocation(
                context.Locations
                .First(l => l.Name == name)
            );
        }
        public List<LocationModel> GetAllLocations()
        {
            return mapper.ParseLocation(
                context.Locations
                .ToList()
            );
        }
        public List<OrderModel> GetAllOrdersByLocationIDDateAscending(int id)
        {
            return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Location == id)
                    .OrderBy(c => c.OrderDate)
                    .ToList()
                );
        }

        public List<OrderModel> GetAllOrdersByLocationIDDateDescending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Where(l => l.Location == id)
                .OrderByDescending(l => l.OrderDate)
                .ToList()
            );
        }

        public List<OrderModel> GetAllOrdersByLocationIDPriceAscending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Where(l => l.Location == id)
                .OrderBy(l => l.Price)
                .ToList()
            );
        }

        public List<OrderModel> GetAllOrdersByLocationIDPriceDescending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Where(l => l.Location == id)
                .OrderByDescending(l => l.Price)
                .ToList()
            );
        }

        #endregion
    }
}