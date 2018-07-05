namespace Fanex.ProductStatus.Setting.Models
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using AppResources;
    using Common;
    using Services;

    public class Result
    {
        private readonly RefDataServiceProvider refDataServiceProvider;

        public Result(RefDataServiceProvider refDataServiceProvider = null)
        {
            this.refDataServiceProvider = refDataServiceProvider ?? new RefDataServiceProviderImpl();
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public Result(
            Hashtable outputParams, 
            RefDataServiceProvider refDataServiceProvider = null) : this(refDataServiceProvider)
        {
            CreateResult(outputParams);
        }


        public int Code { get; private set; }

        public string Message { get; private set; }

        public bool IsSuccess() => Code == 0;

        private void CreateResult(Hashtable outputParams)
        {
            if (outputParams == null
                || !outputParams.ContainsKey("errcode")
                || !outputParams.ContainsKey("errmsg"))
            {
                Code = 500;
                Message = Resources.InternalServerError;
            }
            else
            {
                Code = Convert.ToInt32(outputParams["errcode"]);
                Message = IsSuccess()
                    ? Resources.StatusUpdatedSuccessfully
                    : ParseDCSProductMessage(outputParams["errmsg"].ToString());
            }
        }

        private string ParseDCSProductMessage(string errorMessageXml)
        {
            if (IsValidXml(errorMessageXml))
            {
                var xmlRoot = XElement.Parse(errorMessageXml);
                var errCode = xmlRoot.Attributes("MsgID").FirstOrDefault().Value;

                var messageElement = xmlRoot.Elements().Select(row => GetBetTypeName(row.Attributes("BetType").FirstOrDefault().Value)).ToList();
                messageElement.Insert(0, Resources.ResourceManager.GetString("Msg" + errCode));

                return string.Join("<br/>", messageElement);
            }

            return errorMessageXml;
        }

        private string GetBetTypeName(string BetType) => refDataServiceProvider.GetBetTypeName(Converter.ToInt(BetType));
        
        private static bool IsValidXml(string xml)
        {
            if (!string.IsNullOrEmpty(xml) && xml.TrimStart().StartsWith("<"))
            {
                try
                {
                    XDocument.Parse(xml);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
