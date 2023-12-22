using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task
{
    public class Shortage
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public Room Room { get; set; }
        public Category Category { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedOn { get; set; }

        public Shortage()
        {

        }
        public Shortage(string title, string name, Room room, Category category, int priority, DateTime createdOn)
        {
            Title = title;
            Name = name;
            Room = room;
            Category = category;
            Priority = priority;
            CreatedOn = createdOn;
        }

        public override string ToString()
        {
            return string.Format("{0,-20} | {1,-20} | {2,-15} | {3,-15} | {4,-10} | {5}",
                   Title, Name, Room, Category, Priority, CreatedOn.ToString("yyyy-MM-dd HH:mm:ss"));
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
}
