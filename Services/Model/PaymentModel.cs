namespace Services.Model
{
    public class PaymentModel
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string CardDueDate { get; set; }
        public IEnumerable<int>? CourseIds { get; set; } = default;
    }
}
