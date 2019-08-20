using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace webAppInventhe.Client
{
    public class Startup
    {
        static Startup()
        {
            typeof(System.ComponentModel.INotifyPropertyChanging).GetHashCode();
            typeof(System.ComponentModel.INotifyPropertyChanged).GetHashCode();
        }
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
