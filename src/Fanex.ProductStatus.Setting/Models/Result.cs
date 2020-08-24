namespace Fanex.ProductStatus.Setting.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using AppResources;

    public class Result
    {
        private const int InternalErrorCode = 500;
        public const int UMNonDCSErrorCode = 90014;
        private const int DoNothingErrorCode = 90017;

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

        public bool IsSuccess()
            => Code == 0;

        public bool IsUMOfNonDCS()
            => Code == UMNonDCSErrorCode;

        public bool IsDoNothing()
            => Code == DoNothingErrorCode;

        public string ParseDCSProductMessage(int productId)
        {
            if (IsValidXml())
            {
                var errorMessages = new List<string>();

                var xmlRoot = XElement.Parse(Message);
                var errCode = xmlRoot.Attributes("MsgID").FirstOrDefault().Value;
                errorMessages.Add(Resources.ResourceManager.GetString("Msg" + errCode));               

                Message = string.Join("<br/>", errorMessages);
            }

            return Message;
        }

        private void CreateResult(Hashtable outputParams)
        {
            if (outputParams == null
                || !outputParams.ContainsKey("errcode")
                || !outputParams.ContainsKey("errmsg"))
            {
                Code = InternalErrorCode;
                Message = Resources.InternalServerError;
            }
            else
            {
                Code = Convert.ToInt32(outputParams["errcode"]);
                Message = IsSuccess()
                    ? Resources.StatusUpdatedSuccessfully
                    : outputParams["errmsg"].ToString();
            }
        }
        
        private bool IsValidXml()
        {
            var isValidXml = false;

            if (!string.IsNullOrWhiteSpace(Message) && Message.TrimStart().StartsWith("<"))
            {
                try
                {
                    XDocument.Parse(Message);
                    isValidXml = true;
                }
                catch (Exception)
                {
                }
            }

            return isValidXml;
        }
    }
}
