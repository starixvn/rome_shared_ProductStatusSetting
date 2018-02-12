namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using AppResources;
    using Models;
    using Shared;

    public class QueryViewModel
    {
        private readonly IDictionary<byte, string> roleDictionary = new Dictionary<byte, string>
        {
            [RoleIdentity.Super] = Resources.Super,
            [RoleIdentity.Master] = Resources.Master,
            [RoleIdentity.Agent] = Resources.Agent,
            [RoleIdentity.Member] = Resources.Member,
        };

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<ProductStatusModel> ProductStatus { get; set; }

        public string GetRoleName(byte roleId)
        {
            string roleName;
            roleDictionary.TryGetValue(roleId, out roleName);

            return roleName;
        }

        public bool IsValid()
            => ProductId > 0
            && CustId > 0
            && !string.IsNullOrEmpty(CustName)
            && roleDictionary.ContainsKey(CustRole);
    }
}
