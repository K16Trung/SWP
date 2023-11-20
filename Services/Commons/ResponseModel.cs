namespace Services.Commons
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public string? Errors { get; set; }
        public bool HasError => Errors?.Length > 0;
    }
}
