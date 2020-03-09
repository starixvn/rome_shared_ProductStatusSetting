namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using Fanex.ProductStatus.Setting.Criteria;
    using Fanex.ProductStatus.Setting.Models;
    using Fanex.ProductStatus.Setting.Shared;

    interface IProductProfileService
    {
        int GetSettingLevel(int productId, int systemId);
    }

    public class ProductProfileService : IProductProfileService
    {
        private static IEnumerable<ProductProfile> NonDCSProductSetting = new List<ProductProfile>
        {
            new ProductProfile(ProductIdentity.Casino, ProductSettingLevel.Product),
            new ProductProfile(ProductIdentity.Keno, ProductSettingLevel.BetType) ,
            new ProductProfile(ProductIdentity.NumberGame, ProductSettingLevel.Product),
            new ProductProfile(ProductIdentity.GoldDeluxe, ProductSettingLevel.Product)
        };

        private static IEnumerable<ProductProfile> DCSProductSetting = new List<ProductProfile>
        {
            new ProductProfile(ProductIdentity.VirtualSports, null, 13),
            new ProductProfile(ProductIdentity.Sportbook2, null, 13),
            new ProductProfile(ProductIdentity.LiveCasino, null, 13),
            new ProductProfile(ProductIdentity.MacauGames, null, 13),
            new ProductProfile(ProductIdentity.NG2, null, 11),
            new ProductProfile(ProductIdentity.FishingWorld, null, 13),
            new ProductProfile(ProductIdentity.Lottery, null, 13),
            new ProductProfile(ProductIdentity.VirtualGames, null, 13),
            new ProductProfile(ProductIdentity.Sabaclub, null, 13),
        };

        private readonly ProductProfileDataAccess productProfileDataAccess;

        public ProductProfileService(ProductProfileDataAccess productProfileDataAccess = null)
        {
            this.productProfileDataAccess = productProfileDataAccess ?? new ProductProfileDataAccessImpl();
        }

        public int GetSettingLevel(int productId, int systemId)
        {
            var isNonDscProduct = NonDCSProductSetting.Any(i => i.ProductId == productId);
            if (isNonDscProduct)
            {
                return NonDCSProductSetting.First(i => i.ProductId == productId).SettingLevel.Value;
            }

            var productProfile = productProfileDataAccess.GetProductProfile(
                new SelectProductProfileCriteria(
                    productId,
                    DCSProductSetting.First(i => i.ProductId == productId).SettingType.Value,
                    systemId));            

            return productProfile.SettingLevel.Value;
        }
    }
}
