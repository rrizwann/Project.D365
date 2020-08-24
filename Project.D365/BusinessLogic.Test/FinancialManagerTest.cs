using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Risika.D365.Core.Managers;
using System;

namespace BusinessLogic.Test
{
    [TestClass]
    public class FinancialManagerTest : BaseManagerTest
    {
        BaseManager _manager;
        EntityReference _companyid; 
        string _cvr;

        [TestInitialize]
        public void FinancialManagerTestSetup()
        {
            this._manager = new FinancialManager(Service);

            this._companyid = new EntityReference("account", new Guid("1AA427D6-AC07-EA11-A811-000D3AB36982")); 
            //this._companyid = new EntityReference("account", new Guid("6690284A-8828-EA11-A810-000D3AB3629A"));
          //  this._cvr = "37677892";
            this._cvr = "75509977";
        }

        [TestMethod]
        public void TestCopyFinancials()
        {
            ((FinancialManager)this._manager).CopyFinancials(this._companyid, this._cvr, BaseUrl, AccessToken, BaseLanguage,Country);
        }
    }
}
