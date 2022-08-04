namespace Fanex.ProductStatus.Setting.DataAccess
{
    using Data;
    using Fanex.ProductStatus.Setting.Shared;
    using Models;
    using NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface ProductStatusDataAccess
    {
        IEnumerable<ProductStatusModel> GetProductStatus(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<ProductStatusUM> GetProductStatusUM(ICriteria criteria);
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
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                return new Result(db.GetOuputParams());
            }
        }

        private static ProductStatusModel ToProductStatusModel(IDataReader reader)
            => new ProductStatusModel(
                Convert.ToInt32(reader["sporttype"]),
                reader.HasColumn("parentsporttype") ? DataReaderExtensions.ToInt32(reader["parentsporttype"]) : 0,
                Convert.ToInt32(reader["bettype"]),
                reader.HasColumn("parentbettype") ? DataReaderExtensions.ToInt32(reader["parentbettype"]) : 0,
                Convert.ToBoolean(reader["isdisabled"]),
                Convert.ToBoolean(reader["isdisabledupline"]),
                Convert.ToInt32(reader["totaldisabledmasters"]),
                Convert.ToInt32(reader["totalenabledmasters"]),
                Convert.ToInt32(reader["totaldisabledagents"]),
                Convert.ToInt32(reader["totalenabledagents"]),
                Convert.ToInt32(reader["totaldisabledmembers"]),
                Convert.ToInt32(reader["totalenabledmembers"]));

        public IEnumerable<ProductStatusUM> GetProductStatusUM(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetProductStatusUM(reader);
                    }
                }
            }
        }

        private static ProductStatusUM GetProductStatusUM(IDataReader reader)
            => new ProductStatusUM(
                Convert.ToInt32(reader["custid"]),
                Convert.ToInt32(reader["productid"]),
                Convert.ToInt32(reader["sporttype"]),
                Convert.ToInt32(reader["bettype"]),
                Convert.ToByte(reader["settingtype"]),
                Convert.ToDateTime(reader["begintime"]),
                Convert.ToDateTime(reader["endtime"]));

    }
}
