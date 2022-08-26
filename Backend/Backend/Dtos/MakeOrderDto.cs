namespace Backend.Dtos
{
    public class MakeOrderDto
    {
        public OrderProductsDto[] orderProducts { get; set; }
        public string comment { get; set; }
        public string payment { get; set; }
    }
}
