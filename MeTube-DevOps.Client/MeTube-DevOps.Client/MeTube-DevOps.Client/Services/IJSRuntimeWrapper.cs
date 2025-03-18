namespace MeTube_DevOps.Client.Services
{
    public interface IJSRuntimeWrapper
    {
        ValueTask<T> InvokeAsync<T>(string identifier, params object[] args);
        ValueTask InvokeVoidAsync(string identifier, params object[] args);
    }
}
