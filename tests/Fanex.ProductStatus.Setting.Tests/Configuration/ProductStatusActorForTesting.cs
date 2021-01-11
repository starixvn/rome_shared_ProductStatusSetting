namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System.Collections.Generic;
    using Setting.Services;

    public class ProductStatusActorForTesting : IProductStatusActor
    {
        public int Id { get; } = 8;

        public string Name { get; } = "sa";

        public string ClientIP { get; } = "127.0.0.1";

        // NAP = 1
        // Ext-NAP = 4
        public byte SystemId { get; } = 1;

        public byte SettingStatus { get; } = 0;

        public IEnumerable<KeyValuePair<int, string>> GetProductList()
            => new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(3, "Number Game"),
                new KeyValuePair<int, string>(6, "Casino"),
                new KeyValuePair<int, string>(8, "Keno"),
                new KeyValuePair<int, string>(12, "Gold Deluxe"),
                new KeyValuePair<int, string>(21, "Allbets"),
                new KeyValuePair<int, string>(22, "Macau Games"),
                new KeyValuePair<int, string>(23, "NG2"),
                new KeyValuePair<int, string>(24, "Fishing World"),
            };
    }
}
