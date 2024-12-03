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
    public class TeachersControllerTests
    {
        private ApplicationDbContext _context;
        private TeachersController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Teacher.Add(new Teacher { TeacherId = 1, TeacherName = "Teacher One" });
            _context.Teacher.Add(new Teacher { TeacherId = 2, TeacherName = "Teacher Two" });
            _context.SaveChanges();

            controller = new TeachersController(_context);
        }

        [TestMethod]
        public void Index_ReturnsViewWithCorrectModel()
        {
            var result = (ViewResult)controller.Index().Result;
            var model = (List<Teacher>)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName); 
            CollectionAssert.AreEqual(_context.Teacher.ToList(), model);
        }

        [TestMethod]
        public void Details_ValidId_ReturnsCorrectTeacher()
        {
            var result = (ViewResult)controller.Details(1).Result;
            var model = (Teacher)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ViewName); 
            Assert.IsNotNull(model);
            Assert.AreEqual("Teacher One", model.TeacherName);
        }

        [TestMethod]
        public void Details_InvalidId_ReturnsNotFound()
        {
            var result = (NotFoundResult)controller.Details(999).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ValidTeacher_RedirectsToIndex()
        {
            var newTeacher = new Teacher { TeacherName = "Teacher Three" };
            var result = (RedirectToActionResult)controller.Create(newTeacher).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Create_InvalidTeacher_ReturnsView()
        {
            controller.ModelState.AddModelError("TeacherName", "Required");
            var newTeacher = new Teacher();
            var result = (ViewResult)controller.Create(newTeacher).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
