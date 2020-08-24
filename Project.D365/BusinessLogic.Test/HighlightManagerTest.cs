using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Risika.D365.Core.Managers;
using System;

namespace BusinessLogic.Test
{
    [TestClass]
    public class HighlightManagerTest : BaseManagerTest
    {
        BaseManager _manager;
        EntityReference _companyid;
        string _cvr;
        ITracingService tracingService;

        [TestInitialize]
        public void HighlightManagerTestSetup()
        {
            this._manager = new HighlightManager(Service);

            this._companyid = new EntityReference("account", new Guid("1E9F140E-C6D3-E911-A82F-000D3AB7148C"));
            this._cvr = "72288000";//"37677892";
        }

        [TestMethod]
        public void TestCopyHighlights()
        {
            ((HighlightManager)this._manager).CopyHighlights(this._companyid, this._cvr, BaseUrl, AccessToken, BaseLanguage, tracingService,Country);
        }
    }
}
