using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Project.D365.Core.Managers;
using System;

namespace BusinessLogic.Test
{
    [TestClass]
    public class ScoreManagerTest : BaseManagerTest
    {
        BaseManager _manager;
        EntityReference _companyid;
        string _cvr;

        [TestInitialize]
        public void ScoreManagerTestSetup()
        {
            this._manager = new ScoreManager(Service);

            this._companyid = new EntityReference("account", new Guid("1E9F140E-C6D3-E911-A82F-000D3AB7148C"));
            this._cvr = "37677892";
        }

        [TestMethod]
        public void TestCopyScores()
        {
            ((ScoreManager)this._manager).CopyScores(this._companyid, this._cvr, BaseUrl, AccessToken, BaseLanguage,Country);
        }
    }
}
