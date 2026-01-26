namespace LanguageCards.Api.Dto
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public string? ValidationError { get; set; }

        public int Status { get; set; }


    }
}
