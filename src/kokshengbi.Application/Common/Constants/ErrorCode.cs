namespace kokshengbi.Application.Common.Constants
{
    public class ErrorCode
    {
        public int Code { get; }
        public string Message { get; }
        public string Description { get; }

        private ErrorCode(int code, string message, string description)
        {
            Code = code;
            Message = message;
            Description = description;
        }

        public static readonly ErrorCode SUCCESS = new ErrorCode(0, "ok", "");
        public static readonly ErrorCode PARAMS_ERROR = new ErrorCode(40000, "请求参数错误", "");
        public static readonly ErrorCode NULL_ERROR = new ErrorCode(40001, "请求数据为空", "");
        public static readonly ErrorCode NOT_LOGIN = new ErrorCode(40100, "未登录", "");
        public static readonly ErrorCode NO_AUTH_ERROR = new ErrorCode(40101, "无权限", "");
        public static readonly ErrorCode EXISTED_ERROR = new ErrorCode(40201, "请求数据已存在", "");
        public static readonly ErrorCode TOO_MANY_REQUEST = new ErrorCode(42900, "请求过于频繁", "");
        public static readonly ErrorCode FORBIDDEN_ERROR = new ErrorCode(40300, "禁止访问", "");
        public static readonly ErrorCode NOT_FOUND_ERROR = new ErrorCode(40400, "请求数据不存在", "");
        public static readonly ErrorCode SYSTEM_ERROR = new ErrorCode(50000, "系统内部异常", "");
        public static readonly ErrorCode OPERATION_ERROR = new ErrorCode(50001, "操作失败", "");
        // Add more error codes as needed

        //public static readonly ErrorCode SUCCESS = new ErrorCode(0, "ok", "");
        //public static readonly ErrorCode PARAMS_ERROR = new ErrorCode(40000, "Request parameter error", "");
        //public static readonly ErrorCode NULL_ERROR = new ErrorCode(40001, "Request data is null", "");
        //public static readonly ErrorCode NOT_LOGIN = new ErrorCode(40100, "Not logged in", "");
        //public static readonly ErrorCode NO_AUTH_ERROR = new ErrorCode(40101, "No permission", "");
        //public static readonly ErrorCode EXISTED_ERROR = new ErrorCode(40201, "Request data already exists", "");
        //public static readonly ErrorCode FORBIDDEN_ERROR = new ErrorCode(40300, "Forbidden access", "");
        //public static readonly ErrorCode NOT_FOUND_ERROR = new ErrorCode(40400, "Request data not found", "");
        //public static readonly ErrorCode SYSTEM_ERROR = new ErrorCode(50000, "Internal system error", "");
        //public static readonly ErrorCode OPERATION_ERROR = new ErrorCode(50001, "Operation failed", "");
    }
}
