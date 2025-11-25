namespace Fanex.ProductStatus.Setting.DataAccess
{
    // TODO: Requires Fanex.Data migration
    // using Data;
    using Fanex.ProductStatus.Setting.Criteria;
    using Fanex.ProductStatus.Setting.Shared;
    using Models;
    // TODO: Requires Fanex.NAP.Framework.Data migration
    // using NAP.Framework.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface ProductStatusDataAccess
    {
        // TODO: Requires ICriteria from Fanex.NAP.Framework
        // IEnumerable<ProductStatusModel> GetProductStatus(ICriteria criteria);
        IEnumerable<ProductStatusModel> GetProductStatus(object criteria);

        // Result ExecuteNonQuery(ICriteria criteria);
        Result ExecuteNonQuery(object criteria);

        // IEnumerable<ProductStatusUM> GetProductStatusUM(ICriteria criteria);
        IEnumerable<ProductStatusUM> GetProductStatusUM(object criteria);
    }

    internal class ProductStatusDataAccessImpl : ProductStatusDataAccess
    {
        public IEnumerable<ProductStatusModel> GetProductStatus(object criteria)
        {
            // TODO: Requires Fanex.Data.ObjectDb and ICriteria migration
            throw new NotImplementedException("Requires Fanex.Data.ObjectDb migration");
            /*
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
            */
        }

        public Result ExecuteNonQuery(object criteria)
        {
            // TODO: Requires Fanex.Data.ObjectDb and ICriteria migration
            throw new NotImplementedException("Requires Fanex.Data.ObjectDb migration");
            /*
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                return new Result(db.GetOuputParams());
            }
            */
        }

        private static ProductStatusModel ToProductStatusModel(IDataReader reader)
            => new ProductStatusModel(
                Convert.ToInt32(reader["sporttype"]),
                reader.HasColumn("parentsporttype") ? Shared.DataReaderExtensions.ToInt32(reader["parentsporttype"]) : 0,
                Convert.ToInt32(reader["bettype"]),
                reader.HasColumn("parentbettype") ? Shared.DataReaderExtensions.ToInt32(reader["parentbettype"]) : 0,
                Convert.ToBoolean(reader["isdisabled"]),
                Convert.ToBoolean(reader["isdisabledupline"]),
                Convert.ToInt32(reader["totaldisabledmasters"]),
                Convert.ToInt32(reader["totalenabledmasters"]),
                Convert.ToInt32(reader["totaldisabledagents"]),
                Convert.ToInt32(reader["totalenabledagents"]),
                Convert.ToInt32(reader["totaldisabledmembers"]),
                Convert.ToInt32(reader["totalenabledmembers"]),
                Convert.ToInt32(reader["totaldisabledsuperdirectmembers"]),
                Convert.ToInt32(reader["totalenabledsuperdirectmembers"]),
                Convert.ToInt32(reader["totaldisabledmasterdirectmembers"]),
                Convert.ToInt32(reader["totalenabledmasterdirectmembers"]));

        public IEnumerable<ProductStatusUM> GetProductStatusUM(object criteria)
        {
            // TODO: Requires Fanex.Data.ObjectDb and ICriteria migration
            throw new NotImplementedException("Requires Fanex.Data.ObjectDb migration");
            /*
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
            */
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