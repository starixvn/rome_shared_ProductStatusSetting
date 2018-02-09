namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Models;

    public class MultipleUpdateViewModel : UpdateViewModelBase
    {
        public MultipleUpdateViewModel()
            : base()
        {
        }

        public Customer[] Customers { get; set; }

        public override bool IsValid()
            => base.IsValid() && Customers?.Length > 0;
    }
}
