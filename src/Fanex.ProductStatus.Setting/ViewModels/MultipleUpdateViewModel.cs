namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;

    public class MultipleUpdateViewModel
    {
        private readonly IProductStatusUser user;

        public MultipleUpdateViewModel()
            : this(null)
        {
        }

        public MultipleUpdateViewModel(IProductStatusUser user)
        {
            this.user = user ?? ProductStatusConfigurationHandler.User;
        }

        public int ProductId { get; set; }

        public int[] CustIds { get; set; }

        public string[] CustNames { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public string IP
            => user.GetClientIP();

        public int ActorId
            => user.GetActorId();

        public bool IsValid()
        {
            var isValidCustomers = CustIds?.Length > 0
                && CustNames?.Length > 0
                && CustIds.Length == CustNames.Length;

            return ProductId > 0
                && isValidCustomers
                && ProductStatus?.Length > 0;
        }
    }
}
