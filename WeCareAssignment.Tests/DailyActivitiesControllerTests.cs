using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using WeCareAssignment.Controllers;
using WeCareAssignment.Data;
using WeCareAssignment.Models;

namespace WeCareAssignment.Tests
{
    [TestClass]
    public class DailyActivitiesControllerTests
    {
        private ApplicationDbContext _context;
        private DailyActivitiesController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _context.DailyActivity.Add(new DailyActivity { DailyActivityId = 1, DailyActivityName = "Activity One" });
            _context.DailyActivity.Add(new DailyActivity { DailyActivityId = 2, DailyActivityName = "Activity Two" });
            _context.SaveChanges();

            controller = new DailyActivitiesController(_context);
        }

        [TestMethod]
        public void Index_ReturnsViewWithCorrectModel()
        {
            var result = (ViewResult)controller.Index().Result;
            var model = (List<DailyActivity>)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName); 
            CollectionAssert.AreEqual(_context.DailyActivity.ToList(), model);
        }

        [TestMethod]
        public void Details_ValidId_ReturnsCorrectActivity()
        {
            var result = (ViewResult)controller.Details(1).Result;
            var model = (DailyActivity)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ViewName); 
            Assert.IsNotNull(model);
            Assert.AreEqual("Activity One", model.DailyActivityName);
        }

        [TestMethod]
        public void Details_InvalidId_ReturnsNotFound()
        {
            var result = (NotFoundResult)controller.Details(999).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ValidActivity_RedirectsToIndex()
        {
            var newActivity = new DailyActivity { DailyActivityName = "Activity Three" };
            var result = (RedirectToActionResult)controller.Create(newActivity).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Create_InvalidActivity_ReturnsView()
        {
            controller.ModelState.AddModelError("DailyActivityName", "Required");
            var newActivity = new DailyActivity();
            var result = (ViewResult)controller.Create(newActivity).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
