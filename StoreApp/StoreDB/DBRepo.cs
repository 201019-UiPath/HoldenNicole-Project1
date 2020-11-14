﻿using Microsoft.EntityFrameworkCore;
using StoreDB;
using StoreDB.Entities;
using StoreDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreUI
{
    public class DBRepo : IStoreRepo
    {
        private readonly ixdssaucContext context;
        private readonly IMapper mapper;
        public IMapper Mapper;

        public ixdssaucContext Context { get; set; }

        public DBRepo()
        {
            this.context = new ixdssaucContext();
            this.mapper = new StoreMapper();
        }

        #region cart methods
        public void AddCart(CartsModel cartsModel)
        {
            context.Carts.Add(mapper.ParseCarts(cartsModel));
            context.SaveChanges();
            //want to add this at creation of new customer
        }
        public CartsModel GetCartID(int id)
        {
            return mapper.ParseCarts(
                context.Carts
                .First(c => c.Customer == id)
                );
        } 
        public CartItemModel AddProductToCart(CartItemModel cartItem)
        {
            context.CartItems.Add(mapper.ParseCartItem(cartItem));
            context.SaveChanges();
            return cartItem;
        }
        public LineItemModel AddToOrder(LineItemModel cartItem)
        {
            context.LineItems.Add(mapper.ParseLineItem(cartItem));
            context.SaveChanges();
            return cartItem;
            //need to make order controller that will work without user input to make cart into order
        }
        public CartItemModel UpdateCartItems(CartItemModel cartItem)
        {
            context.CartItems.Update(mapper.ParseCartItem(cartItem));
            context.SaveChangesAsync();
            return cartItem;
        } 

        public CartItemModel DeleteProductInCart(int cartid, int productid, int quantity)
        {
            var cartItem = context.CartItems.First(i => i.Cart == cartid && i.Product == productid);
            cartItem.Quantity -= 1;
            context.SaveChanges();
            return null;
        }
        public void DeleteProductInCart(CartItemModel cartItem)
        {
            context.CartItems.Remove(mapper.ParseCartItem(cartItem));
            context.SaveChanges();
        } //this one is in a previously working version

        public void DeleteCart(CartsModel carts)
        {
            context.Carts.Remove(mapper.ParseCarts(carts));
            context.SaveChanges();
        } 
        public List<CartItemModel> GetAllProductsInCartByCartID(int id)
        {
            return mapper.ParseCartItem(
                context.CartItems
                .Include("Products")
                .Include("Inventory")
                .Where(i => i.Cart == id)
                .ToList()
            );
        }
        public List<LineItemModel> GetAllProductsInOrderByID(int id)
        {
            return mapper.ParseLineItem(
                context.LineItems
                .Include("Products")
                .Where(i => i.Id == id)
                .ToList());
        }
        public OrderModel PlaceOrder(OrderModel order)
        {
            context.Orders.Add(mapper.ParseOrder(order));
            context.SaveChanges();
            return order;
        }
        public void PlaceOrders(OrderModel order)
        {
            context.Orders.Add(mapper.ParseOrder(order));
            context.SaveChanges();
        } // in a working version
        #endregion

        #region inventory methods
        public ProductModel GetProductByID(int id)
        {
            return mapper.ParseProducts(
                context.Products
                .First(i => i.Id == id)
                );
        } 
        public LocationModel GetLocationInventory(int id)
        {
            return mapper.ParseLocation(
                context.Locations
                .Include("Inventory")
                //.Include("Orders")
                .First(l => l.Id == id));
        } //not in a working version
        public List<InventoryModel> ViewAllProductsAtLocation(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Include("Products")
                .ToList());
        }
        public List<InventoryModel> ViewAllProductsAtLocationSortByQuantityAscending(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Include("Inventory")
                .Where(i => i.Location == id)
                .OrderBy(i => i.Quantity)
                .ToList()
            );
        }
        public List<InventoryModel> ViewAllProductsAtLocationSortByQuantityDescending(int id)
        {
            return mapper.ParseInventory(
                context.Inventory
                .Include("Inventory")
                .Where(i => i.Location == id)
                .OrderByDescending(i => i.Quantity)
                .ToList()
            );
        }
      
        public void AddProductToLocation(InventoryModel item, int quantity)
        {
            context.Inventory.First(i => i.Location == item.locationID && i.Product == item.productID).Quantity += quantity;
            context.SaveChanges();
        } 
        public InventoryModel DeleteProductAtLocation(int locationid, int productid, int quantity)
        {
            var inventory = context.Inventory.First(i => i.Location == locationid && i.Product == productid);
            inventory.Quantity -= quantity;
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
            return mapper.ParseCustomer(
                context.Customer
                .First(c => c.Username == name)
            );

        } 

        public CustomerModels GetCustomerByEmail(string email)
        {
            try
            {
                return mapper.ParseCustomer(
                    context.Customer
                    .FirstOrDefault(c => c.Email == email)
                );
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("This email is not registered try again");
            }
            return null;
        } 

        public CustomerModels AddCustomer(CustomerModels customer)
        {
            context.Customer.Add(mapper.ParseCustomer(customer));
            context.SaveChanges();
            return customer;
        }
        public void AddCustomer2(CustomerModels customer)
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
            return mapper.ParseOrder(
                context.Orders
                .Include("Products")
                .Include("LineItems")
                .Where(c => c.Customer == customer.ID)
                .OrderBy(c => c.OrderDate)
                .ToList()
            );
        } 
        public OrderModel GetOrderByID(LocationModel location, CustomerModels customer)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("Products")
                .Include("LineItems")
                .First(o => o.Location == location.ID && o.Customer == customer.ID));
        }
        public List<OrderModel> GetAllOrdersByCustomerIDDateDescending(CustomerModels customer)
        {
            return mapper.ParseOrder(
                 context.Orders
                 .Include("Products")
                .Include("LineItems")
                 .Where(c => c.Customer == customer.ID)
                 .OrderByDescending(c => c.OrderDate)
                 .ToList()
             );
        }

        public List<OrderModel> GetAllOrdersByCustomerIDPriceAscending(CustomerModels customer)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("Products")
                .Include("LineItems")
                .Where(c => c.Customer == customer.ID)
                .OrderBy(c => c.Price)
                .ToList()
            );
        }
        public List<OrderModel> GetAllOrdersByCustomerIDPriceDescending(CustomerModels customer)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("Products")
                .Include("LineItems")
                .Where(c => c.Customer == customer.ID)
                .OrderByDescending(c => c.Price)
                .ToList()
            );
        } 
        #endregion

        #region location methods
        public ManagerModel GetManagerByName(string name)
        {
            return mapper.ParseManager(
                context.Managers
                .Include("Location")
                .First(m => m.Username == name));
        }
        public LocationModel GetLocationByManager(int id)
        {
            return mapper.ParseLocation(
                context.Locations
                .First(l => l.Manager == id));
        }
        public LocationModel GetLocationByID(int id)
        {
            return mapper.ParseLocation(
                context.Locations
                .First(l => l.Id == id)
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
        public List<OrderModel> GetAllOrdersByLocationID(int id)
        {
            return mapper.ParseOrder(
                    context.Orders
                    .Where(c => c.Location == id)
                    .Include("LineItems")
                    .ToList()
                );
        }
        public List<OrderModel> GetAllOrdersByLocationIDDateDescending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("LineItems")
                .Where(l => l.Location == id)
                .OrderByDescending(l => l.OrderDate)
                .ToList()
            );
        }
        public List<OrderModel> GetAllOrdersByLocationIDPriceAscending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("LineItems")
                .Where(l => l.Location == id)
                .OrderBy(l => l.Price)
                .ToList()
            );
        }
        public List<OrderModel> GetAllOrdersByLocationIDPriceDescending(int id)
        {
            return mapper.ParseOrder(
                context.Orders
                .Include("LineItems")
                .Where(l => l.Location == id)
                .OrderByDescending(l => l.Price)
                .ToList()
            );
        } 
        #endregion
    }
}