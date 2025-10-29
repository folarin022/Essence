namespace EssenceShop.Dto
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }


        public static BaseResponse<T> SuccessResponse(T? data, string message = "Operation successful")
        {
            return new BaseResponse<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static BaseResponse<bool> FailResponse(string message)
        {
            return new BaseResponse<bool>
            {
                IsSuccess = false,
                Data = false,
                Message = message
            };
        }
    }
}
