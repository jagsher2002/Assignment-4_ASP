using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeCareAssignment.Controllers;
using WeCareAssignment.Data;
using WeCareAssignment.Models;

namespace WeCareAssignment.Tests
{
    [TestClass]
    public class ChildrenControllerTests
    {
        private ApplicationDbContext _context;
        private ChildrenController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            var parent = new Parent { ParentId = 1, ParentName = "Parent Name" };
            var teacher = new Teacher { TeacherId = 1, TeacherName = "Teacher Name" };
            var dailyActivity = new DailyActivity { DailyActivityId = 1, DailyActivityName = "Activity Name" };

            _context.Parent.Add(parent);
            _context.Teacher.Add(teacher);
            _context.DailyActivity.Add(dailyActivity);

            var child = new Child
            {
                childId = 1,
                childName = "John Doe",
                ParentId = 1,
                TeacherId = 1,
                DailyActivityId = 1
            };

            _context.Child.Add(child);
            _context.SaveChanges();

            controller = new ChildrenController(_context);
        }

        [TestMethod]
        public void Details_ReturnsCorrectChild()
        {
            var result = (ViewResult)controller.Details(1).Result;
            var model = (Child)result.Model;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual("John Doe", model.childName);
        }

        [TestMethod]
        public void Create_ValidChild_RedirectsToIndex()
        {
            var newChild = new Child { childName = "Jane Doe", ParentId = 1, TeacherId = 1, DailyActivityId = 1 };
            var result = (RedirectToActionResult)controller.Create(newChild).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Create_InvalidChild_ReturnsView()
        {
            controller.ModelState.AddModelError("childName", "Required");
            var newChild = new Child { ParentId = 1, TeacherId = 1, DailyActivityId = 1 };
            var result = (ViewResult)controller.Create(newChild).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Details_InvalidId_ReturnsNotFound()
        {
            var result = (NotFoundResult)controller.Details(999).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
