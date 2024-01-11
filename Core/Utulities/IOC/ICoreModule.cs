using Microsoft.Extensions.DependencyInjection;

namespace Core.Utulities.IOC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection collection);
    }
}
