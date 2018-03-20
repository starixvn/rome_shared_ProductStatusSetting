namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class QueryViewModel : BaseQueryViewModel
    {
        public string CustName { get; set; }

        public string GetRoleName(byte roleId)
        {
            string roleName;
            roleDictionary.TryGetValue(roleId, out roleName);

            return roleName;
        }
    }
}
