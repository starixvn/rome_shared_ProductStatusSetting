using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class ProductStatusViewModel
    {
        public int ProductId { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<ProductStatusModel> ProductStatuses { get; set; }

        public bool IsInvalid()
            => ProductId <= 0 || CustId <= 0 || string.IsNullOrEmpty(CustName) || CustRole == 0 || CustRole > 4;
    }
}
