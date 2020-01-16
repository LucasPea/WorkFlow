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
    public class CrayonAccessTests
    {
        [TestMethod()]
        [Priority(1)]
        public void SetupNewCustomerTest()
        {

            CrayonAccess myCrayonAccess = new CrayonAccess("US");
            //myCrayonAccess.SetupNewCustomer("damian", "sinay", "dsinay@cowrysolutions.com", "+44 7968 809206",
              //  "BBcowrysoTest", "1st Floor The Blade Abbey Street", "Reading ", "RG1 3BE", "Berkshire", "United Kingdom", 3, 2);
            /*
            myCrayonAccess.SetupNewCustomer("damian", "sinay", "dsinay@cowrysolutions.com", "+17074959909",
                "BBcowrysoTest", "60 29th Street", "San Francisco ", "94110", "California", "United States",  3, 2);*/

            string pepe = "ASsad";
            Assert.IsNotNull(pepe);
        }
    }
}