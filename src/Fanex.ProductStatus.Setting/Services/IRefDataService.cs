namespace Fanex.ProductStatus.Setting.Services
{
    internal interface IRefDataService
    {
        string GetBetTypeName(int betTypeId);

        string GetSportName(int sportTypeId);

        string GetProductName(int productId);
    }
}
