using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MeTube_DevOps.Client;
using MeTube_DevOps.Client.Models;
using MeTube_DevOps.Client.Services;
using MeTube_DevOps.Client.Pages;
using MeTube_DevOps.Client.ViewModels;
using Microsoft.JSInterop;
using MeTube.Client.Pages;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5020/") });
builder.Services.AddSingleton<LoginView>();
builder.Services.AddSingleton<ManageUsersView>();
builder.Services.AddSingleton<Video>();
builder.Services.AddSingleton<Home>();
builder.Services.AddSingleton<ManageVideos>();
builder.Services.AddSingleton<EditVideo>();
builder.Services.AddSingleton<UploadVideo>();
builder.Services.AddSingleton<History>();
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

builder.Services.AddScoped<IJSRuntimeWrapper, JSRuntimeWrapper>();
builder.Services.AddScoped<IClientService,ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<ICommentServicee, CommentService>();
builder.Services.AddScoped<IAdminHistoryService, AdminHistoryService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddAutoMapper(typeof(User));
builder.Services.AddAutoMapper(typeof(Video));
builder.Services.AddAutoMapper(typeof(Like));
builder.Services.AddAutoMapper(typeof(History));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddTransient<HttpClient>();


// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapHealthChecks("/health");
// });
// app.MapHealthChecks("/health");
await builder.Build().RunAsync();
