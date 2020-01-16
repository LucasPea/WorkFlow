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
    public class HydrationAPITests
    {
        [TestMethod()]
        public void HydrationAPITest()
        {
            HydrationAPI myHydrationAPI = new HydrationAPI();
            Guid companyID =  myHydrationAPI.CreateCompany("VVABCDE2222212");
            myHydrationAPI.uploadExtension(companyID);

            
            Assert.IsNotNull(myHydrationAPI.token);

            
        }
    }
}