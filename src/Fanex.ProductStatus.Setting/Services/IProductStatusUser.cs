namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;

    public interface IProductStatusUser
    {
        int GetActorId();

        string GetClientIP();

        IEnumerable<KeyValuePair<int, string>> GetProductList();
    }
}
