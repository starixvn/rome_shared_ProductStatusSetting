namespace Fanex.ProductStatus.Setting.Models
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(int custId, string custName)
        {
            CustId = custId;
            CustName = custName;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }
    }
}
