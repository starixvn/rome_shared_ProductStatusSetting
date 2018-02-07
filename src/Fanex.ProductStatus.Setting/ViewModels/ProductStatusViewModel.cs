using System.Collections.Generic;
using Fanex.ProductStatus.Setting.AppResources;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class ProductStatusViewModel
    {
        private readonly IDictionary<byte, string> roleDictionary = new Dictionary<byte, string>
        {
            [4] = Resources.Super,
            [3] = Resources.Master,
            [2] = Resources.Agent,
            [1] = Resources.Member,
        };

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<ProductStatusModel> ProductStatuses { get; set; }

        public string GetRoleName(byte roleId)
        {
            string roleName;
            roleDictionary.TryGetValue(roleId, out roleName);

            return roleName;
        }

        public bool IsInvalid()
            => ProductId <= 0 || CustId <= 0 || string.IsNullOrEmpty(CustName) || !roleDictionary.ContainsKey(CustRole);
    }
}
