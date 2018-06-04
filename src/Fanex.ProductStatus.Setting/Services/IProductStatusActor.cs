namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;

    public interface IProductStatusActor
    {
        int Id { get; }

        string Name { get; }

        string ClientIP { get; }

        byte SystemId { get; }

        IEnumerable<KeyValuePair<int, string>> GetProductList();
    }
}
