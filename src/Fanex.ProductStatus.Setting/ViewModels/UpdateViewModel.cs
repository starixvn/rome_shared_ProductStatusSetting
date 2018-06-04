namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
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

        public byte CustRole { get; set; }

        public IEnumerable<ProductStatusCommand> ProductStatus { get; set; }

        public string IP
            => userService.GetClientIP();

        public int ActorId
            => userService.GetActorId();

        public string ActorName
           => userService.GetActorName();

        public virtual bool IsValid()
            => ProductId > 0
            && CustId > 0
            && ProductStatus.ToList().Count > 0
            && ProductStatus.Any(productStatus => !productStatus.Valid())
            && ActorId > 0;
    }
}
