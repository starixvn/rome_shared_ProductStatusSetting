namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Models;
    using Services;

    public class MultipleUpdateViewModel : UpdateViewModelBase
    {
        public MultipleUpdateViewModel()
            : base(null)
        {
        }

        public MultipleUpdateViewModel(IProductStatusUser user)
            : base(user)
        {
        }

        public Customer[] Customers { get; set; }

        public override bool IsValid()
            => base.IsValid() && Customers?.Length > 0;
    }
}
