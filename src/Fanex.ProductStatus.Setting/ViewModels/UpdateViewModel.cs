namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;

    public class UpdateViewModel
    {
        private readonly IProductStatusUserService userService;

        public UpdateViewModel()
            : this(null)
        {
        }

        public UpdateViewModel(IProductStatusUserService userService)
        {
            this.userService = userService ?? ProductStatusConfigurationHandler.UserService;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public string IP
            => userService.GetClientIP();

        public int ActorId
            => userService.GetActorId();

        public string ActorName
           => userService.GetActorName();

        public virtual bool IsValid()
            => ProductId > 0
            && CustId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
