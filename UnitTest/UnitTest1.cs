using Microsoft.VisualStudio.TestTools.UnitTesting;
using sotrudniki;
using sotrudniki.Model;
using sotrudniki.ViewModel;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RoleTest()
        {
            RoleViewModel v = new RoleViewModel();
            Role r = new Role { 
                Id = 5,
                NameRole = "Сварщик"
            };
            v.ListRole.Add(r);
            v.SaveChanges(v.ListRole);
        }
        [TestMethod]
        public void PersonTest()
        {
            PersonViewModel v = new PersonViewModel();
            Person r = new Person {
                Id = 5,
                RoleId = 1,
                FirstName = "Сергей",
                LastName = "Сергеев",
                Birthday = DateTime.Now,
                Role = new Role()
            };
            v.ListPerson.Add(r);
        }
    }
}
