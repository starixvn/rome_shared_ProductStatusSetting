namespace Fanex.ProductStatus.Setting.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using AppResources;
    using Data;
    using Models;
    using NAP.Framework.Data;

    internal interface ProductStatusDataAccess
    {
        IEnumerable<ProductStatusModel> GetProductStatus(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }

    internal class ProductStatusDataAccessImpl : ProductStatusDataAccess
    {
        public IEnumerable<ProductStatusModel> GetProductStatus(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToProductStatusModel(reader);
                    }
                }
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            if (!criteria.IsValid())
            {
                return new Result(400, Resources.BadRequest);
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                return new Result(db.GetOuputParams());
            }
        }

        private static ProductStatusModel ToProductStatusModel(IDataReader reader)
            => new ProductStatusModel(
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToBoolean(reader["isdisabled"]),
                Convert.ToBoolean(reader["isdisabledupline"]),
                Convert.ToInt32(reader["totaldisabledmasters"]),
                Convert.ToInt32(reader["totalenabledmasters"]),
                Convert.ToInt32(reader["totaldisabledagents"]),
                Convert.ToInt32(reader["totalenabledagents"]),
                Convert.ToInt32(reader["totaldisabledmembers"]),
                Convert.ToInt32(reader["totalenabledmembers"]));
    }
}
