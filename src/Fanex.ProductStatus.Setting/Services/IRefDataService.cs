namespace Fanex.ProductStatus.Setting.Services
{
    public interface IRefDataService
    {
        string GetBetTypeName(int betTypeId);

        string GetSportName(int sportTypeId);

        string GetProductName(int productId);
    }
}
