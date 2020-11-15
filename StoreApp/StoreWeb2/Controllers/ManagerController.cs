﻿using Microsoft.AspNetCore.Mvc;
using StoreDB;
using StoreDB.Entities;
using StoreDB.Models;
using System.Collections.Generic;
using db = StoreDB.Models;

namespace StoreWeb2.Controllers
{
    public class ManagerController : Controller
    {
        private IStoreRepo storeRepo;
        db.ManagerModel manager = new db.ManagerModel();
        public void ManagersController(IStoreRepo repo)
        {
            storeRepo = repo;
        }
     /*   public IActionResult SignInAsManager(string manager)
        {
            db.ManagerModel managers = storeRepo.GetManagerByName(manager);
            return View(managerHome);
        }
        public IActionResult PickManagerLocation(int id)
        {
            db.LocationModel location = storeRepo.GetLocationByManager(id);
            return View(locationInventory);
        }
        public IActionResult GetOrdersByLocation(int id)
        {
            List<OrderModel> orders = storeRepo.GetAllOrdersByLocationIDDateAscending(id);
            return View(locationHistory);
        }
         public IActionResult AddProductToLocation(int locationid, int productid, int quantity)
         {
            Inventory inventory = storeRepo.AddProductToLocation(locationid, productid, quantity);
             return View(locationInventory);
             ///want to send this to matching location may need to have 1 for each
         }
         public IActionResult RemoveProductFromLocation(int locationid, int productid, int quantity)
         {
            Inventory inventory = storeRepo.DeleteProductAtLocation(locationid, productid, quantity);
            return View(locationInventory);
             ///want to send this to matching location may need to have 1 for each
         }  */
    }
}