namespace Services
{
    public class ConnectionStrings
    {
        public string SQLServerDB { get; set; }
    }
    public class JWTSection
    {
        public string SecretKey { get; set; }
        public int ExpiresInDays { get; set; }
    }
}
