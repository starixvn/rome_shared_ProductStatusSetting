namespace Fanex.ProductStatus.Setting.Models
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using AppResources;

    public class Result
    {
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public Result(Hashtable outputParams)
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

        private static string ParseDCSProductMessage(string errorMessage)
        {
            if (IsValidXml(errorMessage))
            {
                var xmlRoot = XElement.Parse(errorMessage);
                var attributes = xmlRoot.Attributes("Msg");

                return string.Join("<br/>", attributes.Select(a => a.Value));
            }

            return errorMessage;
        }

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
