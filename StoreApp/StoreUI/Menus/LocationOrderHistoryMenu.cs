using StoreDB;
using StoreDB.Entities;

namespace StoreUI.Menus
{
    public class LocationOrderHistoryMenu : IMenu
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
        private ManagerMenu managerMenu;

        private DBRepo dBRepo;
        private StoreContext storeContext;
        private StoreMapper storeMapper;

        public LocationOrderHistoryMenu(StoreContext storeContext, StoreMapper storeMapper)
        {
            this.storeContext = storeContext;
            this.storeMapper = storeMapper;
        }

        public void Start()
        {
            Locations locationID = new Locations();
            /// <summary>
            /// different ways to sort orders select by location
            /// </summary>
            /// <value></value>
            System.Console.WriteLine($"How would you like the order history for {locationID.Name} sorted?");
            int id = locationID;
            System.Console.WriteLine("[1] Date most recent-oldest");
            System.Console.WriteLine("[2] Date oldest-most recent");
            System.Console.WriteLine("[3] Price high-low");
            System.Console.WriteLine("[4] Price low-high");
            System.Console.WriteLine("[5] Return to customer menu");
            System.Console.WriteLine("[6] exit");
            string sortedHistory = System.Console.ReadLine();
            do
            {
                switch (sortedHistory)
                {
                    case "1":
                        System.Console.WriteLine("Here is the location order history sorted by most recent first: ");
                        dBRepo.GetAllOrdersByLocationIDDateDescending(id);
                        break;
                    case "2":
                        System.Console.WriteLine("Here is the location order history with oldest orders first: ");
                        dBRepo.GetAllOrdersByLocationIDDateAscending(id);
                        break;
                    case "3":
                        System.Console.WriteLine("Here is the location order history sorted by highest price first: ");
                        dBRepo.GetAllOrdersByLocationIDPriceDescending(id);
                        break;
                    case "4":
                        System.Console.WriteLine("Here is the location order history with cheapest items first: ");
                        dBRepo.GetAllOrdersByLocationIDPriceDescending(id);
                        break;
                    case "5":
                        System.Console.WriteLine("Redirecting you back to the order history menu: ");
                        /// <summary>
                        /// return manager to order history menu
                        /// </summary>
                        /// <returns></returns>
                        managerMenu.Start();
                        break;
                }
            } while (!sortedHistory.Equals(6));
        }
    }
}