using Fanex.ProductStatus.Setting.Services;

namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class UpdateViewModel : UpdateViewModelBase
    {
        public UpdateViewModel()
            : base(null)
        {
        }

        public UpdateViewModel(IProductStatusUserService userService)
            : base(userService)
        {
        }

        public int CustId { get; set; }

        public override bool IsValid()
            => base.IsValid() && CustId > 0;
    }
}
