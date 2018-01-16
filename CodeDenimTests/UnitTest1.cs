using System;
using System.Collections.Generic;
using CodedenimWebApp.Infrastructure;
using CodedenimWebApp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeDenimTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestIGenericRepository()
        {
            var db = new ApplicationDbContext();
            var student = db.Students;
            var repo = new GenericRepository<>(db);

            var result = repo.GetWhere(x => x.FirstName == "Joe");

            Assert.AreEqual(typeof(IEnumerable<Student>), result);
        }
    }

    public class Student
    {
        public string FirstName { get; set; }
    }
}
