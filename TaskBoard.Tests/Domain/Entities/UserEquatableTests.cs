using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Domain.Entities;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Tests.Domain.Entities
{
    [TestClass]
    public class UserEquatableTests
    {
        [TestMethod]
        public void Equals_Null_IsFalse()
        {
            var u = new User("Name", "Surname", "name@email.com", UserRole.Member);
            Assert.IsFalse(u.Equals(null));
        }

        [TestMethod]
        public void Users_WithSameId_AreEqual_AndWorkInHashSet()
        {
            var u1 = new User("A", "B", "a@b.com", UserRole.Member);
            var u2 = new User("X", "Y", "x@y.com", UserRole.Admin);

            ForcePersonId(u2, u1.Id);

            Assert.IsTrue(u1.Equals(u2));
            Assert.AreEqual(u1.GetHashCode(), u2.GetHashCode());

            var set = new HashSet<User> { u1};
            Assert.IsTrue(set.Contains(u2));
        }

        private static void ForcePersonId(User user, Guid id)
        {
            //probujemy propertyid z setterem
            var t = user.GetType();
            while(t != null)
            {
                var prop = t.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public |
                    BindingFlags.NonPublic);

                if(prop != null && prop.CanWrite)
                {
                    prop.SetValue(user, id);
                    return;
                }

                t = t.BaseType;
            }

            //probowanie pole _id w PErson
            t = user.GetType();
            while(1 != null)
            {
                var field = t.GetField("_id", BindingFlags.Instance | BindingFlags.NonPublic);
                if(field != null)
                {
                    field.SetValue(user, id);
                    return;
                }

                t = t.BaseType;
            }

            throw new InvalidOperationException("Cannot set Person.Id for test, add setter or expose Id");
        }
    }
}
