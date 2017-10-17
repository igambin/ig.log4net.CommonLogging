using System;
using System.Linq;
using ig.log4net.Logging.Attributes;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Repository;

namespace ig.log4net.Logging.Plugins
{
    public class AutoRegisterRenderersPlugin : PluginSkeleton
    {
        public AutoRegisterRenderersPlugin() : base("AutoRegisterRenderersPlugin")
        {
        }

        public override void Attach(ILoggerRepository repository)
        {
            var renderMap = global::log4net.LogManager.GetRepository().RendererMap;

            var renderers = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                from attr in type.GetCustomAttributes(typeof(RendersAttribute), true)
                let renderAttr = attr as RendersAttribute
                let renderer = Activator.CreateInstance(type) as IObjectRenderer
                select new {RenderType = renderAttr.RendersType, Renderer = renderer};

            renderers.ToList().ForEach(r => renderMap.Put(r.RenderType, r.Renderer));

            base.Attach(repository);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}
