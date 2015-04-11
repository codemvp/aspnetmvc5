using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MatchByteMovieApp.Startup))]
namespace MatchByteMovieApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
