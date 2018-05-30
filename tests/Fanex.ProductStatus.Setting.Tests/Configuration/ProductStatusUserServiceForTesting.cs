namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System.Collections.Generic;
    using Setting.Services;

    public class ProductStatusUserServiceForTesting : IProductStatusUserService
    {
        public int GetActorId()
            => 8;

        public string GetActorName()
            => "sa";

        public string GetClientIP()
            => "127.0.0.1";

        // NAP = 1
        // Ext-NAP = 4
        public byte GetSystemId()
            => 1;

        public IEnumerable<KeyValuePair<int, string>> GetProductList()
            => new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(3, "Number Game"),
                new KeyValuePair<int, string>(6, "Casino"),
                new KeyValuePair<int, string>(8, "Keno"),
                new KeyValuePair<int, string>(12, "Gold Deluxe"),
                new KeyValuePair<int, string>(15, "ColossusBets"),
                new KeyValuePair<int, string>(21, "Allbets"),
                new KeyValuePair<int, string>(22, "Macau Games"),
                new KeyValuePair<int, string>(23, "NG2"),
                new KeyValuePair<int, string>(24, "Fishing World"),
                new KeyValuePair<int, string>(30, "Bullfighting")
            };
    }
}
