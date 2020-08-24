using Microsoft.VisualStudio.TestTools.UnitTesting;
using Risika.D365.Core.Managers;
using Risika.D365.Core.Models.Company;
using Risika.D365.Core.Models.Rating;

namespace BusinessLogic.Test
{
    [TestClass]
    public class CompanyManagerTest : BaseManagerTest
    {
        BaseManager _manager;
        string _name;
        string _cvr;

        [TestInitialize]
        public void TestManagerSetup()
        {
            this._manager = new CompanyManager(Service);

            this._name = "mic";
            this._cvr = /*"25142161"*//*"89858410"*/"11573444"; 
        }

        [TestMethod]
        public void TestGetCompany()
        {
            ((CompanyManager)this._manager).GetCompany(this._cvr);
        }

        [TestMethod]
        public void TestGetCompanyData()
        {
            var companyResponses = ((CompanyManager)this._manager)
                .GetCompanyData(this._cvr, BaseUrl, AccessToken, BaseLanguage,Country);
        }

        [TestMethod]
        public void TestGetCreditData()
        {
            var creditResponses = ((CompanyManager)this._manager)
                .GetCreditData(this._cvr, BaseUrl, AccessToken, BaseLanguage,Country);
        }

        [TestMethod]
        public void TestGetCompanies()
        {
            string companies = ((CompanyManager)this._manager)
                .GetCompanies(this._name, BaseUrl, AccessToken, Country);
        }
    }
}
