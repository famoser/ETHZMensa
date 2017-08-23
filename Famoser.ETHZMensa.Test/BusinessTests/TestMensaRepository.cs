using System;
using System.Linq;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Data.Services;
using Famoser.ETHZMensa.Test.Setup;
using Famoser.FrameworkEssentials.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Newtonsoft.Json;

namespace Famoser.ETHZMensa.Test.BusinessTests
{
    [TestClass]
    public class TestMensaRepository
    {
        [TestInitialize]
        public void InitializeTests()
        {
            UnitTestHelper.Instance.SetupIoc();
        }

        [TestMethod]
        public async Task TestLinks()
        {
            var repo = SimpleIoc.Default.GetInstance<IMensaRepository>();

            //act
            var locs = await repo.GetLocations();

            var service = new HttpService();

            foreach (var loc in locs)
            {
                foreach (var mensaModel in loc.Mensas)
                {
                    var url = new[]
                    {
                        mensaModel.TodayApiUrl,
                        mensaModel.TodayMenuUrl,
                        mensaModel.InfoUrl
                    };
                    foreach (var uri in url)
                    {
                        try
                        {
                            var resp = await service.DownloadAsync(uri);
                            Assert.IsTrue(resp.IsRequestSuccessfull);
                            Assert.IsTrue(!string.IsNullOrEmpty(await resp.GetResponseAsStringAsync()));
                        }
                        catch (Exception e)
                        {
                            Assert.Fail("failed to access " + uri + " from mensa " + mensaModel.Name);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public async Task TestRefresh()
        {
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
            var json = await ss.GetCachedTextFileAsync("cache.json");
            var saveModel = JsonConvert.DeserializeObject<SaveModel>(json);

            var anyHas = false;
            foreach (var locationModel in saveModel.Locations)
            {
                foreach (var mensaModel in locationModel.Mensas)
                {
                    anyHas = anyHas || mensaModel.Menus.Count > 0;

                    var html = await dataService.GetHtml(mensaModel.TodayMenuUrl);
                    Assert.IsNotNull(html, "TodayMenuUrl of Mensa " + JsonConvert.SerializeObject(mensaModel) + " invalid");

                    html = await dataService.GetHtml(mensaModel.InfoUrl);
                    Assert.IsNotNull(html, "InfoUrl of Mensa " + JsonConvert.SerializeObject(mensaModel) + " invalid");
                }
            }
            Assert.IsTrue(anyHas, "no menus have been downloaded");
        }
    }
}
