using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace task
{
    public class ShortageManager
    {
        private static string filePath = "shortages.json";
        private static List<Shortage> existingShortages = ReadShortages(filePath);
        private static List<Shortage> ReadShortages(string filePath)
        {
            try
            {
                var serializer = new JsonSerializer();
                List<Shortage> shortages = new List<Shortage>();
                using (var streamReader = new StreamReader(filePath))
                using (var textReader = new JsonTextReader(streamReader))
                {
                    shortages = serializer.Deserialize<List<Shortage>>(textReader);
                }
                return shortages;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reading error in {filePath}: {ex.Message}");
                return new List<Shortage>();
            }
        }

        public static void AddShortage(string title, string user, Room room, Category category, int priority)
        {

            Shortage shortageToAdd = new Shortage(title, user, room, category, priority, DateTime.Now);

            if (CheckIfExists(shortageToAdd) == null)
            {
                existingShortages.Add(shortageToAdd);
                WriteShortageToJSON(filePath, shortageToAdd);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: Shortage already exists");
                Console.ResetColor();

                Thread.Sleep(3000);

                Shortage old = CheckIfExists(shortageToAdd);
                if (shortageToAdd.Priority > old.Priority)
                {
                    existingShortages.Remove(old);
                    DeleteFromJson(filePath, old);

                    existingShortages.Add(shortageToAdd);
                    WriteShortageToJSON(filePath, shortageToAdd);
                }
            }
        }

        private static void WriteShortageToJSON(string filePath, Shortage shortageToAdd)
        {
            try
            {
                string json = JsonConvert.SerializeObject(existingShortages, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing shortage to {filePath}: {ex.Message}");
                Thread.Sleep(5000);
            }
        }

        private static void DeleteFromJson(string filePath, Shortage shortage)
        {
            try
            {
                List<Shortage> existing = ReadShortages(filePath);
                existing.RemoveAll(s =>
                    s.Title.Equals(shortage.Title, StringComparison.OrdinalIgnoreCase) &&
                    s.Room == shortage.Room);

                string json = JsonConvert.SerializeObject(existingShortages, Formatting.Indented);
                System.IO.File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting in json {filePath}: {ex.Message}");
                Thread.Sleep(5000);
            }
        }

        private static Shortage CheckIfExists(Shortage shortage)
        {
            try
            {
                return existingShortages.Find(s =>
                    s.Title.Equals(shortage.Title, StringComparison.OrdinalIgnoreCase) &&
                    s.Room == shortage.Room);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if shortage exists: {ex.Message}");
                Thread.Sleep(5000);
                return null;
            }
        }

        public static void DeleteRequest(string title, Room room, string user)
        {
            Shortage shortageToDelete = existingShortages.Find(s =>
                    s.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                    s.Room == room);

            if (shortageToDelete != null)
            {
                if (user.ToLower() == "admin" || user.ToLower() == shortageToDelete.Name.ToLower())
                {
                    existingShortages.Remove(shortageToDelete);
                    DeleteFromJson(filePath, shortageToDelete);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING: you dont have permission");
                    Console.ResetColor();
                    Thread.Sleep(3000);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("WARNING: no such shortage");
                Console.ResetColor();
                Thread.Sleep(3000);
            }
        }


        public static List<Shortage> FilterByTitle(string searchTerm, string user)
        {
            string search = searchTerm.ToLower();

            if (user.ToLower() == "admin")
            {
                return existingShortages
                    .Where(s => s.Title.ToLower().Contains(search))
                    .ToList();
            }
            else
            {
                return existingShortages
                    .Where(s => s.Name.ToLower() == user.ToLower())
                    .Where(s => s.Title.ToLower().Contains(search))
                    .ToList();
            }

        }

        public static List<Shortage> FilterByDate(string user, DateTime startDate, DateTime endDate)
        {
            if (user.ToLower() == "admin")
            {
                return existingShortages
                    .Where(s => s.CreatedOn >= startDate && s.CreatedOn <= endDate)
                    .ToList();
            }
            else
            {
                return existingShortages
                    .Where(s => s.Name.ToLower() == user.ToLower())
                    .Where(s => s.CreatedOn >= startDate && s.CreatedOn <= endDate)
                    .ToList();
            }
        }

        public static List<Shortage> FilterByRoom(string user, Room room)
        {
            if (user.ToLower() == "admin")
            {
                return existingShortages
                    .Where(s => s.Room == room)
                    .ToList();
            }
            else
            {
                return existingShortages
                    .Where(s => s.Name.ToLower() == user.ToLower())
                    .Where(s => s.Room == room)
                    .ToList();
            }
        }
        public static List<Shortage> FilterByCategory(string user, Category category)
        {
            if (user.ToLower() == "admin")
            {
                return existingShortages
                    .Where(s => s.Category == category)
                    .ToList();
            }
            else
            {
                return existingShortages
                    .Where(s => s.Name.ToLower() == user.ToLower())
                    .Where(s => s.Category == category)
                    .ToList();
            }
        }

        public static List<Shortage> GetAll(string user)
        {
            if(user.ToLower() == "admin")
            {
                return existingShortages;
            }
            else
            {
                return existingShortages
                    .Where(s => s.Name.ToLower() == user.ToLower())
                    .ToList();
            }
        }

        public static List<Shortage> Sort(List<Shortage> shortages)
        {
            return shortages
                .OrderByDescending(s => s.Priority)
                .ToList();               
        }
    }
}
