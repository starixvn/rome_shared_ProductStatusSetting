using Fanex.ProductStatus.Setting.Configuration;
using Fanex.ProductStatus.Setting.Services;

namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class QueryViewModel : BaseQueryViewModel
    {
        private readonly IProductStatusUserService userService;

        public QueryViewModel()
            : this(null)
        {
        }

        public QueryViewModel(IProductStatusUserService userService)
        {
            this.userService = userService ?? ProductStatusConfigurationHandler.UserService;
        }

        public string CustName { get; set; }

        public byte SystemId
            => userService.GetSystemId();
    }
}
