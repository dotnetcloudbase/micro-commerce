namespace MicroCommerce.ApiService.UseCases;

public static class LockKey
{
    public static string Cart(string id) => $"Lock:Cart:{id}";
}