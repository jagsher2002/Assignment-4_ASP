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
    public class ParentsControllerTests
    {
        private ApplicationDbContext _context;
        private ParentsController controller;

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            _context.Parent.Add(new Parent { ParentId = 1, ParentName = "Parent One" });
            _context.Parent.Add(new Parent { ParentId = 2, ParentName = "Parent Two" });
            _context.SaveChanges();

            controller = new ParentsController(_context);
        }

        [TestMethod]
        public void Index_ReturnsViewWithCorrectModel()
        {
            var result = (ViewResult)controller.Index().Result;
            var model = (List<Parent>)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ViewName); 
            CollectionAssert.AreEqual(_context.Parent.ToList(), model);
        }

        [TestMethod]
        public void Details_ValidId_ReturnsCorrectParent()
        {
            var result = (ViewResult)controller.Details(1).Result;
            var model = (Parent)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ViewName); 
            Assert.IsNotNull(model);
            Assert.AreEqual("Parent One", model.ParentName);
        }

        [TestMethod]
        public void Details_InvalidId_ReturnsNotFound()
        {
            var result = (NotFoundResult)controller.Details(999).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Create_ValidParent_RedirectsToIndex()
        {
            var newParent = new Parent { ParentName = "Parent Three" };
            var result = (RedirectToActionResult)controller.Create(newParent).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void Create_InvalidParent_ReturnsView()
        {
            controller.ModelState.AddModelError("ParentName", "Required");
            var newParent = new Parent();
            var result = (ViewResult)controller.Create(newParent).Result;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
