namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Models;
    using Services;
    using Shared;

    public class MultipleUpdateViewModel
    {
        private readonly IProductStatusUserService userService;

        private int[] customerIds;
        private string[] customerNames;

        public MultipleUpdateViewModel()
            : this(null)
        {
        }

        public MultipleUpdateViewModel(IProductStatusUserService userService)
        {
            this.userService = userService ?? ProductStatusConfigurationHandler.UserService;
        }

        public byte CustRole { get; set; }

        public int ProductId { get; set; }

        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public IEnumerable<ProductStatusCommand> ProductStatus { get; set; }

        public string IP
            => userService.GetClientIP();


        public int ActorId
            => userService.GetActorId();

        public string ActorName
            => userService.GetActorName();

        public int[] CustomerIds
        {
            get
            {
                customerIds = customerIds ?? CustIds.SplitInt();

                return customerIds;
            }
        }

        public string[] CustomerNames
        {
            get
            {
                customerNames = customerNames ?? CustNames.SplitString();

                return customerNames;
            }
        }

        public bool IsValid()
        {
            var isValidCustomers = CustomerIds.Length > 0
                && CustomerNames.Length > 0
                && CustomerIds.Length == CustomerNames.Length;

            return ProductId > 0
                && isValidCustomers
                 && ProductStatus.ToList().Count > 0
                && ProductStatus.Any(productStatus => !productStatus.Valid());
        }
    }
}
