using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cowry2019.integrate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowry2019.integrate.Tests
{
    [TestClass()]
    public class CrmAccessTests
    {
        [TestMethod()]
        public void CreateNewCustomerTest()
        {
            CrmAccess myAccess = new CrmAccess();
            Guid newcus = myAccess.CreateNewCustomer();

            Assert.AreNotEqual(newcus, Guid.Empty);

        }
    }
}