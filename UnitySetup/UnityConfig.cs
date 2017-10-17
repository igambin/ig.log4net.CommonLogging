using Microsoft.Practices.Unity;
using ig.log4net.Caching;

namespace ig.log4net.UnitySetup
{
    public class UnityConfig : UnityContainer
    {
        public static IUnityContainer Container { get; } = new UnityConfig();
        
        static UnityConfig() { }

        private UnityConfig()
        {
            RegisterTypes();
        }

        private void RegisterTypes()
        {
            this.RegisterType<ICachable, MemoryCaching>();
        }

    }
}
