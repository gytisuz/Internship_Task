using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace task
{
    public static class TaskUtils
    {


        public static void MenuAdd(string user)
        {
            Console.Clear();
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();

            Console.WriteLine("Choose room from: Kitchen, MeetingRoom, Bathroom");
            Console.Write("Enter Room: ");
            Room room;
            while (!Enum.TryParse(Console.ReadLine(), out room))
            {
                Console.Write("INCORRECT! Enter room: ");
            }

            Console.WriteLine("Available Categories: Electronics, Food, Other");
            Console.Write("Enter Category: ");
            Category category;
            while (!Enum.TryParse(Console.ReadLine(), out category))
            {
                Console.Write("INCORRECT! Enter category: ");
            }

            Console.Write("Enter Priority (1-10): ");
            int priority;
            while (!int.TryParse(Console.ReadLine(), out priority) || priority < 1 || priority > 10)
            {
                Console.Write("INCORRECT! Enter priority: ");
            }

            ShortageManager.AddShortage(title, user, room, category, priority);
        }
        public static void MenuRemove(string user)
        {
            Console.Clear();
            Console.Write("Enter Title: ");

            string title = Console.ReadLine();

            Console.WriteLine("Choose room from: Kitchen, MeetingRoom, Bathroom");
            Console.Write("Enter Room: ");
            Room room;
            while (!Enum.TryParse(Console.ReadLine(), out room))
            {
                Console.Write("INCORRECT! Enter room: ");
            }

            ShortageManager.DeleteRequest(title, room, user);
        }
        public static void MenuList(string user)
        {
            bool flag = true;

            while (flag)
            {
                Console.Clear();
                Console.WriteLine("Choose menu function by wirting number:");
                Console.WriteLine("1 see all");
                Console.WriteLine("2 filter by title");
                Console.WriteLine("3 filter by date");
                Console.WriteLine("4 filter by room");
                Console.WriteLine("5 filter by category");
                Console.WriteLine("6 EXIT");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        List<Shortage> all = ShortageManager.GetAll(user);
                        PrintList(all);
                        Console.ReadKey();
                        break;
                        
                    case "2":
                        Console.WriteLine("Enter search phrase: ");
                        string title = Console.ReadLine();
                        List<Shortage> filtered = ShortageManager.FilterByTitle(title, user);
                        PrintList(filtered);
                        Console.ReadKey();
                        break;

                    case "3":

                        DateTime startDate;
                        DateTime endDate;
                        do
                        {
                            Console.Write("Enter start date (YYYY-MM-DD): ");
                        } while (!DateTime.TryParse(Console.ReadLine(), out startDate));

                        do
                        {
                            Console.Write("Enter end date (YYYY-MM-DD): ");
                        } while (!DateTime.TryParse(Console.ReadLine(), out endDate));

                        List<Shortage> filteredBydate = ShortageManager.FilterByDate(user, startDate, endDate);
                        PrintList(filteredBydate);
                        Console.ReadKey();
                        break;

                    case "4":
                        Room room;
                        Console.WriteLine("Choose room from: Kitchen, MeetingRoom, Bathroom");
                        do
                        {
                            Console.Write("Enter Room: ");
                        } while (!Enum.TryParse(Console.ReadLine(), out room));

                        List<Shortage> filteredByRoom = ShortageManager.FilterByRoom(user, room);
                        PrintList(filteredByRoom);
                        Console.ReadKey();
                        break;
                    case "5":
                        Category category;
                        Console.WriteLine("Available Categories: Electronics, Food, Other");
                        do
                        {
                            Console.Write("Enter category: ");
                        } while (!Enum.TryParse(Console.ReadLine(), out category));

                        List<Shortage> filteredByCategory = ShortageManager.FilterByCategory(user, category);
                        PrintList(filteredByCategory);
                        Console.ReadKey();
                        break;

                    case "6":
                        flag = false;
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
            }
        }

        public static void PrintList(List<Shortage> shortages)
        {
            if (shortages.Count == 0)
            {
                Console.WriteLine("your list is empty");

            }
            else
            {
                var header = string.Format("{0,-20} | {1,-20} | {2,-15} | {3,-15} | {4,-10} | {5}",
                 "Title: ", "Name: ", "Room: ", "Category: ", "Priority: ", "CreatedOn: ");

                Console.WriteLine(header);
                Console.WriteLine(new string('-', 100));
                List<Shortage> sorted = ShortageManager.Sort(shortages);
                foreach (var item in sorted)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }


}
