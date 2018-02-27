namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using Setting.Services;

    public class ProductStatusUserForTesting : IProductStatusUser
    {
        public int ActorId { get; } = 8;

        public string IP { get; } = "127.0.0.1";
    }
}
