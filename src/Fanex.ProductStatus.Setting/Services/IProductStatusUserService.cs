namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;

    public interface IProductStatusUserService
    {
        int GetActorId();

        string GetActorName();

        string GetClientIP();

        byte GetSystemId();

        IEnumerable<KeyValuePair<int, string>> GetProductList();
    }
}
