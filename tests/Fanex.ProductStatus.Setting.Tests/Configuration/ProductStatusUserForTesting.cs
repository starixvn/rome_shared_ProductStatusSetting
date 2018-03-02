namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using Setting.Services;

    public class ProductStatusUserForTesting : IProductStatusUser
    {
        public Func<int> GetActorId()
            => () => 8;

        public Func<string> GetClientIP()
            => () => "127.0.0.1";
    }
}
