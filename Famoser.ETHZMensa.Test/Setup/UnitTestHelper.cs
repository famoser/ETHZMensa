using Famoser.ETHZMensa.Business.Repositories;
using Famoser.ETHZMensa.Business.Repositories.Interfaces;
using Famoser.ETHZMensa.Data.Services;
using Famoser.ETHZMensa.Presentation.WinUniversal.Services;
using Famoser.ETHZMensa.Test.Setup.Services;
using Famoser.ETHZMensa.View.Services;
using Famoser.FrameworkEssentials.Services.Interfaces;
using Famoser.FrameworkEssentials.Singleton;
using Famoser.FrameworkEssentials.UniversalWindows.Platform;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using IProgressService = Famoser.ETHZMensa.Business.Services.IProgressService;

namespace Famoser.ETHZMensa.Test.Setup
{
    public class UnitTestHelper : SingletonBase<UnitTestHelper>
    {
        public void SetupIoc()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<IStorageService>(() => new StorageService());
            SimpleIoc.Default.Register<IInteractionService, InteractionService>();
            SimpleIoc.Default.Register<INavigationService, MockNavigationService>();
            
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<IProgressService, ProgressService>();
            SimpleIoc.Default.Register<IMensaRepository, MensaRespository>();
        }
    }
}
