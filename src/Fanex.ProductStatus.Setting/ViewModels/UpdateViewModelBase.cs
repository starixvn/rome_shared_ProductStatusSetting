namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;

    public class UpdateViewModelBase
    {
        private readonly IProductStatusUserService userService;

        public UpdateViewModelBase()
            : this(null)
        {
        }

        public UpdateViewModelBase(IProductStatusUserService userService)
        {
            this.userService = userService ?? ProductStatusConfigurationHandler.CreateUserService();
        }

        public int ProductId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public int ActorId
        {
            get
            {
                return userService.ActorId;
            }
        }

        public string IP
        {
            get
            {
                return userService.IP;
            }
        }

        public virtual bool IsValid()
            => ProductId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
