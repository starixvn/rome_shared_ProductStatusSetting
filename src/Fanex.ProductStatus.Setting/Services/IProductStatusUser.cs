namespace Fanex.ProductStatus.Setting.Services
{
    using System;

    public interface IProductStatusUser
    {
        Func<int> GetActorId();

        Func<string> GetClientIP();
    }
}
