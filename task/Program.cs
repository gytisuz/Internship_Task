using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("write your name or ADMIN (if you are the administrator)");
            string user = Console.ReadLine();

            bool flag = true;
            while (flag)
            {
                Console.Clear();
                Console.WriteLine("Choose menu function by wirting number:");
                Console.WriteLine("1 regisgter new shortage");
                Console.WriteLine("2 delete the request");
                Console.WriteLine("3 list all the requests");
                Console.WriteLine("4 EXIT");


                string choice = Console.ReadLine();

                switch (choice)
                {
                    //function to add new shortage
                    case "1":
                        TaskUtils.MenuAdd(user);
                        break;

                    //function to delete existing shortage
                    case "2":
                        TaskUtils.MenuRemove(user);
                        break;

                    //new menu to get shortages list
                    case "3":
                        TaskUtils.MenuList(user);
                        break;

                    //exiting program
                    case "4":
                        Console.Clear();
                        flag = false;
                        Console.WriteLine("Goodbye");
                        break;

                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
            }


        }
    }
}
