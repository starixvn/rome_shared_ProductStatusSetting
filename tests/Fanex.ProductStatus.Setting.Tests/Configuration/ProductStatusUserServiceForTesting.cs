namespace Fanex.ProductStatus.Setting.Tests.Configuration
{
    using System;
    using Setting.Services;

    public class ProductStatusUserServiceForTesting : IProductStatusUserService
    {
        public int ActorId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string IP
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
