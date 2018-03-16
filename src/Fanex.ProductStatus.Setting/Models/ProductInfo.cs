namespace Fanex.ProductStatus.Setting.Models
{
    public class ProductInfo
    {
        public ProductInfo(int sportType, int betType)
        {
            SportType = sportType;
            BetType = betType;
        }

        public int SportType { get; }

        public int BetType { get; }

        public int ProductId { get; set; }

        public string Name { get; set; }
    }
}
