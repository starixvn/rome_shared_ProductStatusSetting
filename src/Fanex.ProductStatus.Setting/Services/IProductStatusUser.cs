namespace Fanex.ProductStatus.Setting.Services
{
    using System;
    using System.Collections.Generic;

    public interface IProductStatusUser
    {
        Func<int> GetActorId();

        Func<string> GetClientIP();

        IEnumerable<KeyValuePair<int, string>> GetProductList();
    }
}
