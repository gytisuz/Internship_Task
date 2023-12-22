using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using task;
 
namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string filePath = "shortages.json";


        [TestInitialize]
        public void Initialize()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void AddShortage_ShouldAddShortageToEmptyList()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);

            List<Shortage> existingShortages = ShortageManager.GetAll("admin");
            Assert.AreEqual(1, existingShortages.Count);
            ShortageManager.AddShortage("Title2", "User1", Room.MeetingRoom, Category.Electronics, 5);
            existingShortages = ShortageManager.GetAll("admin");
            Assert.AreEqual(2, existingShortages.Count);
        }

        [TestMethod]
        public void AddShortage_ShouldUpdateExistingShortageIfPriorityIsHigher()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);

            // Add a shortage with higher priority
            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 8);

            List<Shortage> existingShortages = ShortageManager.GetAll("admin");
            Assert.AreEqual(1, existingShortages.Count);

            Assert.AreEqual(8, existingShortages[0].Priority);
        }


        [TestMethod]
        public void DeleteRequest_ShouldDeleteShortageForAdmin()
        {
            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title2", "User1", Room.MeetingRoom, Category.Electronics, 5);

            ShortageManager.DeleteRequest("Title1", Room.MeetingRoom, "admin");

            List<Shortage> remainingShortages = ShortageManager.GetAll("admin");
            Assert.AreEqual(1, remainingShortages.Count);
        }

        [TestMethod]
        public void FilterByTitle_ShouldFilterShortagesByTitleForAdmin()
        {
            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title2", "User2", Room.MeetingRoom, Category.Food, 3);

            List<Shortage> filteredShortages = ShortageManager.FilterByTitle("Title", "admin");
            Assert.AreEqual(2, filteredShortages.Count);
        }

        [TestMethod]
        public void FilterByDate_ShouldFilterShortagesByDateForAdmin()
        {
            DateTime startDate = DateTime.Now.AddDays(-2);
            DateTime endDate = DateTime.Now.AddDays(-1);

            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);

            List<Shortage> filteredShortages = ShortageManager.FilterByDate("admin", startDate, endDate);
            Assert.AreEqual(0, filteredShortages.Count);
        }

        [TestMethod]
        public void FilterByRoom_ShouldFilterShortagesByRoomForAdmin()
        {
            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title2", "User1", Room.MeetingRoom, Category.Food, 5);
            ShortageManager.AddShortage("Title3", "User1", Room.Bathroom, Category.Electronics, 5);

            List<Shortage> filteredShortages = ShortageManager.FilterByRoom("admin", Room.MeetingRoom);
            Assert.AreEqual(2, filteredShortages.Count);
        }

        [TestMethod]
        public void FilterByCategory_ShouldFilterShortagesByCategoryForAdmin()
        {
            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title2", "User1", Room.MeetingRoom, Category.Food, 5);
            ShortageManager.AddShortage("Title3", "User1", Room.MeetingRoom, Category.Electronics, 5);

            List<Shortage> filteredShortages = ShortageManager.FilterByCategory("admin", Category.Electronics);
            Assert.AreEqual(2, filteredShortages.Count);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllShortagesForAdmin()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title2", "User1", Room.MeetingRoom, Category.Electronics, 5);
            ShortageManager.AddShortage("Title3", "User1", Room.MeetingRoom, Category.Electronics, 5);


            List<Shortage> allShortages = ShortageManager.GetAll("admin");
            Assert.AreEqual(3, allShortages.Count);
        }

        [TestMethod]
        public void Sort_ShouldSortShortagesByPriority()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            ShortageManager.AddShortage("Title1", "User1", Room.MeetingRoom, Category.Electronics, 8);
            ShortageManager.AddShortage("Title2", "User2", Room.MeetingRoom, Category.Food, 3);
            ShortageManager.AddShortage("Title3", "User3", Room.MeetingRoom, Category.Other, 5);

            List<Shortage> unsortedShortages = new List<Shortage>(ShortageManager.GetAll("admin"));
            List<Shortage> sortedShortages = ShortageManager.Sort(unsortedShortages);

           
            Assert.AreEqual(8, sortedShortages[0].Priority);
            Assert.AreEqual(5, sortedShortages[1].Priority);
            Assert.AreEqual(3, sortedShortages[2].Priority);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}