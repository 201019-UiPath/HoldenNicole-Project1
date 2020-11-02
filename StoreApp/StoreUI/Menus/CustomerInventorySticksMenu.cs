using StoreDB;
using StoreDB.Entities;
using System;
using System.Collections.Generic;

namespace StoreUI.Menus
{
    public class CustomerInventorySticks : IMenu
    {
        private string userInput;
        private CustomerMenu customerMenu;
        private CustomerOrderHistoryMenu customerOrderHistoryMenu;
        private CustomerSearch customerSearch;
        private LocationOrderHistoryMenu locationOrderHistoryMenu;
        private SearchBySport searchBySport;
        private SearchByType searchByType;
        private SearchByPerson searchByPerson;
        private SignInMenu signInMenu;
        private SportOrderHistoryMenu sportOrderHistoryMenu;
        private TypeOrderHistoryMenu typeOrderHistoryMenu;
        private ManagerWorldOfBats managerWorldOfBats;
        private ManagerWorldOfGames managerWorldOfGames;
        private ManagerWorldOfJerseys managerWorldOfJerseys;
        private ManagerWorldOfSticks managerWorldOfSticks;
        private CustomerInventoryBatsMenu customerInventoryBatsMenu;
        private CustomerInventoryJerseysMenu customerInventoryJerseysMenu;
        private CustomerInventoryGamesMenu customerInventoryGamesMenu;
        DBRepo dBRepo;
        public void Start()
        {
            ///retrieve location from previous menus
            Locations location = new Locations();
            Console.WriteLine($"How would you like to see the inventory for World Of Sticks:");
            Console.WriteLine("[1] By type /n [2] By sport /n [3] By person /n [4] exit store");
            string sorting = System.Console.ReadLine();
            do
            {
                switch (sorting)
                {
                    /// lists products sorted by type
                    case "1":
                        Console.WriteLine("What type of autographed item are you looking for?");
                        string item = Console.ReadLine();
                        List<Products> allProductsByType = dBRepo.ViewAllProductsByItem(item);
                        break;
                    ///lists products sorted by sport
                    case "2":
                        Console.WriteLine("What sport are you looking for autographs for?");
                        string sport = Console.ReadLine();
                        List<Products> allProductBySport = dBRepo.ViewAllProductsBySport(sport);
                        break;
                    /// lists products by person
                    case "3":
                        Console.WriteLine("What athlete are you looking for?");
                        string athlete = Console.ReadLine();
                        List<Products> allProductsByPerson = dBRepo.ViewAllProductsByAthlete(athlete);
                        break;
                    case "4":
                        Console.WriteLine("Bye hope you come again soon");
                        break;
                }
            } while (!sorting.Equals(4)); 
        }
    }
}