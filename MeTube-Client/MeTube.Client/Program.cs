using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MeTube.Client;
using MeTube.Client.Models;
using MeTube.Client.Services;
using MeTube.Client.Pages;
using MeTube.Client.ViewModels;
using MeTube.Client.ViewModels.HistoryViewModels;
using MeTube.Client.ViewModels.LoginViewModels;
using MeTube.Client.ViewModels.LogoutViewModels;
using MeTube.Client.ViewModels.ManageUsersViewModels;
using MeTube.Client.ViewModels.SignupViewModels;
using MeTube.Client.ViewModels.VideoViewModels;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
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

builder.Services.AddSingleton<IJSRuntimeWrapper>();
builder.Services.AddSingleton<JSRuntimeWrapper>();
builder.Services.AddSingleton<IClientService>();

builder.Services.AddSingleton<IUserService>();
builder.Services.AddScoped<IVideoService>();
builder.Services.AddScoped<ILikeService>();
builder.Services.AddScoped<IHistoryService>();
builder.Services.AddScoped<ICommentServicee>();
builder.Services.AddScoped<IAdminHistoryService>();

builder.Services.AddSingleton<ClientService>();
builder.Services.AddSingleton<VideoService>();
builder.Services.AddSingleton<LikeService>();
builder.Services.AddSingleton<HistoryService>();
builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<AdminHistoryService>();




builder.Services.AddScoped<IAuthenticationService>();
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddAutoMapper(typeof(Video));
builder.Services.AddAutoMapper(typeof(Like));
builder.Services.AddAutoMapper(typeof(History));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddTransient<HttpClient>();

await builder.Build().RunAsync();
