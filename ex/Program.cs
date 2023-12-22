using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

public class Shortage
{
    public string Title { get; set; }
    public string Name { get; set; }
    public Room Room { get; set; }
    public Category Category { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return $"Title: {Title}, Name: {Name}, Room: {Room}, Category: {Category}, Priority: {Priority}, CreatedOn: {CreatedOn}";
    }
}

public enum Room
{
    MeetingRoom,
    Kitchen,
    Bathroom
}

public enum Category
{
    Electronics,
    Food,
    Other
}

public class ShortageManager
{
    private const string FilePath = "shortages.json";
    private List<Shortage> shortages;

    public ShortageManager()
    {
        LoadShortages();
    }

    public void RegisterShortage(Shortage newShortage)
    {
        if (shortages.Any(s => s.Title.Equals(newShortage.Title, StringComparison.OrdinalIgnoreCase) &&
                               s.Room == newShortage.Room))
        {
            var existingShortage = shortages.First(s => s.Title.Equals(newShortage.Title, StringComparison.OrdinalIgnoreCase) &&
                                                        s.Room == newShortage.Room);
            if (newShortage.Priority > existingShortage.Priority)
            {
                existingShortage.Priority = newShortage.Priority;
            }
            else
            {
                Console.WriteLine("Warning: Shortage already exists with the same title and room.");
                return;
            }
        }
        else
        {
            shortages.Add(newShortage);
        }

        SaveShortages();
    }

    public void DeleteShortage(string title, string name)
    {
        var shortageToDelete = shortages.FirstOrDefault(s => s.Title.Equals(title, StringComparison.OrdinalIgnoreCase) &&
                                                              s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (shortageToDelete != null)
        {
            shortages.Remove(shortageToDelete);
            SaveShortages();
        }
        else
        {
            Console.WriteLine("Shortage not found.");
        }
    }

    public List<Shortage> ListShortages(string currentUser, DateTime startDate, DateTime endDate, Category category, Room room)
    {
        var filteredShortages = shortages.Where(s => s.CreatedOn >= startDate &&
                                                     s.CreatedOn <= endDate &&
                                                     (string.IsNullOrEmpty(currentUser) || s.Name.Equals(currentUser, StringComparison.OrdinalIgnoreCase)) &&
                                                     (category == Category.Other || s.Category == category) &&
                                                     (room == Room.MeetingRoom || s.Room == room))
                                        .OrderByDescending(s => s.Priority)
                                        .ToList();
        return filteredShortages;
    }

    private void LoadShortages()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            shortages = JsonConvert.DeserializeObject<List<Shortage>>(json);
        }
        else
        {
            shortages = new List<Shortage>();
        }
    }

    private void SaveShortages()
    {
        var json = JsonConvert.SerializeObject(shortages, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }
}

class Program
{
    static void Main()
    {
        var shortageManager = new ShortageManager();

        // Use shortageManager to interact with the application, handle user input, and display results.
    }
}
