using System.Linq;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Data.Services;
using Famoser.ETHZMensa.Test.Setup;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;

namespace Famoser.ETHZMensa.Test.BusinessTests
{
    [TestClass]
    public class TestMensaRepository
    {
        [TestMethod]
        public async Task TestRefresh()
        {
            UnitTestHelper.Instance.SetupIoc();
            var repo = SimpleIoc.Default.GetInstance<IMensaRepository>();
            var ss = SimpleIoc.Default.GetInstance<IStorageService>();
            var dataService = SimpleIoc.Default.GetInstance<IDataService>();

            //act
            var locs = await repo.GetLocations();
            var refreshResult = await repo.Refresh();

            //assert
            Assert.IsTrue(locs != null && locs.Count > 0, "locs != null && locs.Count > 0");
            Assert.IsTrue(refreshResult, "refreshResult");

            //get json & deserialize
            var json = await ss.GetCachedData();
            var saveModel = JsonConvert.DeserializeObject<SaveModel>(json);
            var excludes = new string[] { "Bistro"};

            foreach (var locationModel in saveModel.Locations)
            {
                foreach (var mensaModel in locationModel.Mensas)
                {
                    if (excludes.All(e => e != mensaModel.Name))
                    Assert.IsTrue(mensaModel.Menus.Count > 0, "Menus of Mensa " + JsonConvert.SerializeObject(mensaModel) + " empty");

                    var html = await dataService.GetHtml(mensaModel.TodayApiUrl);
                    Assert.IsNotNull(html, "TodayApiUrl of Mensa " + JsonConvert.SerializeObject(mensaModel) + " invalid");

                    html = await dataService.GetHtml(mensaModel.InfoUrl);
                    Assert.IsNotNull(html, "InfoUrl of Mensa " + JsonConvert.SerializeObject(mensaModel) + " invalid");
                }
            }
        }
    }
}
