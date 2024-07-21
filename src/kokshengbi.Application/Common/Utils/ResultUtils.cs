using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Models;

namespace kokshengbi.Application.Common.Utils
{
    public class ResultUtils
    {
        public static BaseResponse<T> success<T>(T data)
        {
            return new BaseResponse<T>(0, data, "ok");
        }
        public static BaseResponse<T> error<T>(ErrorCode errorCode, string message)
        {
            return new BaseResponse<T>(errorCode.Code, default(T), message, errorCode.Description);
        }
        public static BaseResponse<T> error<T>(int code, T data, string message, string description)
        {
            return new BaseResponse<T>(code, default(T), message, description);
        }
    }
}
