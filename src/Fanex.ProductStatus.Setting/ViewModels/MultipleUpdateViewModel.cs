namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;
    using Shared;

    public class MultipleUpdateViewModel
    {
        private readonly IProductStatusUser user;

        private int[] customerIds;
        private string[] customerNames;

        public MultipleUpdateViewModel()
            : this(null)
        {
        }

        public MultipleUpdateViewModel(IProductStatusUser user)
        {
            this.user = user ?? ProductStatusConfigurationHandler.User;
        }

        public int ProductId { get; set; }

        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public string IP
            => user.GetClientIP();

        public int ActorId
            => user.GetActorId();

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
                && ProductStatus?.Length > 0;
        }
    }
}
