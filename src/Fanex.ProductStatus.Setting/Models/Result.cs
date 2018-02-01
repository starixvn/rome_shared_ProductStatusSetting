using System;
using System.Collections;
using Fanex.ProductStatus.Setting.AppResources;

namespace Fanex.ProductStatus.Setting.Models
{
    public class Result
    {
        private Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; set; }

        public static Result CreateSuccessResult(string message = null)
        {
            return new Result(0, message);
        }

        public static Result CreateErrorResult(int code, string message)
        {
            return new Result(code, message);
        }

        public static Result CreateResult(Hashtable outputParams)
        {
            if (outputParams == null ||
                !outputParams.ContainsKey("errcode") ||
                !outputParams.ContainsKey("errmsg"))
            {
                return CreateErrorResult(500, Resources.InternalServerError);
            }

            return CreateErrorResult(Convert.ToInt32(outputParams["errcode"]), outputParams["errmsg"].ToString());
        }

        public bool IsSuccess()
        {
            return Code == 0;
        }
    }
}
