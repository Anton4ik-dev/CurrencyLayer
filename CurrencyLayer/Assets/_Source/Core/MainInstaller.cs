using CurrencyLayer;
using Zenject;

namespace Core
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            #region CurrencyLayer
            Container
                .Bind<CurrencyLayerAPI>()
                .AsSingle()
                .NonLazy();
            #endregion
        }
    }
}