namespace JwtAuthorizationDemo.Models.DTOs
{
    public class ResultDto<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        protected ResultDto(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static ResultDto<T> Success(T value)
        {
            return new ResultDto<T>(true, value, null);
        }

        public static ResultDto<T> Failure(string error)
        {
            return new ResultDto<T>(false, default, error);
        }
    }
}
