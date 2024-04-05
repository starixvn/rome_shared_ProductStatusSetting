namespace Fanex.ProductStatus.Setting.DataAccess
{
    using System;
    using Criteria;
    using Data;
    using Models;

    public interface ProductGroupProfileDataAccess
    {
        ProductGroupProfile GetProductGroupProfile(SelectProductProfileByGroupCriteria criteria);
    }

    public class ProductGroupProfileDataAccessImpl : ProductGroupProfileDataAccess
    {
        public ProductGroupProfile GetProductGroupProfile(SelectProductProfileByGroupCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return new ProductGroupProfile(
                            criteria.GroupId,
                            Convert.ToByte(reader["grouptype"]),
                            Convert.ToByte(reader["settinglevel"]));
                    }
                }
            }

            return null;
        }
    }
}
