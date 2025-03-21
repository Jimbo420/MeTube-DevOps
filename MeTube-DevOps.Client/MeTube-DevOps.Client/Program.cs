using MeTube_DevOps.Client.Models;
using MeTube_DevOps.Client.Services;
using MeTube_DevOps.Client.ViewModels;
using MeTube_DevOps.Client.ViewModels.HistoryViewModels;
using MeTube_DevOps.Client.ViewModels.LoginViewModels;
using MeTube_DevOps.Client.ViewModels.LogoutViewModels;
using MeTube_DevOps.Client.ViewModels.ManageUsersViewModels;
using MeTube_DevOps.Client.ViewModels.SignupViewModels;
using MeTube_DevOps.Client.ViewModels.VideoViewModels;
using MeTube_DevOps.Client.Views;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
namespace MeTube_DevOps.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Add after builder initialization
            // if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("METUBE_CLIENT_PORT"))) {
            //     throw new Exception("Please specify the port number for METUBE.Client with the environment variable METUBE_CLIENT_PORT.");
            // }
            

            var clientPort = Environment.GetEnvironmentVariable("METUBE_CLIENT_PORT") ?? "8080";  


            builder.Services.AddScoped(sp => { 
            var gatewayScheme = Environment.GetEnvironmentVariable("METUBE_PUBLIC_GATEWAY_SCHEME") ?? "http";
            // var gatewayHost = Environment.GetEnvironmentVariable("METUBE_PUBLIC_GATEWAY_HOST") ?? "localhost";
            // if(gatewayHost == "localhost")
            // {
            //     gatewayHost = "57.153.7.223";
            // }

            var gatewayHost = "57.153.7.223";
            
            var gatewayPort = Environment.GetEnvironmentVariable("METUBE_PUBLIC_GATEWAY_PORT") ?? "5010";
            
            var apiBaseUrl = $"{gatewayScheme}://{gatewayHost}:{gatewayPort}";
            
                return new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
            });

            builder.Services.AddSingleton<LoginView>();
            builder.Services.AddSingleton<ManageUsersView>();
            builder.Services.AddSingleton<VideoView>();
            builder.Services.AddSingleton<Home>();
            builder.Services.AddSingleton<ManageVideos>();
            builder.Services.AddSingleton<EditVideo>();
            builder.Services.AddSingleton<UploadVideo>();
            builder.Services.AddSingleton<HistoryView>();
            builder.Services.AddSingleton<ManageHistory>();


            builder.Services.AddTransient<SignupViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddScoped<LogoutViewModel>();
            builder.Services.AddTransient<ManageUsersViewModel>();
            builder.Services.AddTransient<VideoViewModel>();
            builder.Services.AddTransient<VideoListViewModel>();
            builder.Services.AddTransient<ManageVideosViewModel>();
            builder.Services.AddTransient<EditVideoViewModel>();
            builder.Services.AddTransient<UploadVideoViewModel>();
            builder.Services.AddTransient<UserHistoryViewModel>();
            builder.Services.AddTransient<AdminHistoryViewModel>();

            builder.Services.AddSingleton<IJSRuntimeWrapper, JSRuntimeWrapper>();
            builder.Services.AddSingleton<IClientService,ClientService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<ILikeService, LikeService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<IAdminHistoryService, AdminHistoryService>();

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddAutoMapper(typeof(User));
            builder.Services.AddAutoMapper(typeof(Video));
            builder.Services.AddAutoMapper(typeof(Like));
            builder.Services.AddAutoMapper(typeof(History));
            builder.Services.AddTransient<HttpClient>();


            await builder.Build().RunAsync();
        }
    }
}
