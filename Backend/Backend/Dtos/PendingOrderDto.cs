namespace Backend.Dtos
{
    public class PendingOrderDto
    {
        public long Id { get; set; }
        public float Price { get; set; }
        public long UserId { get; set; }
        public string Comment { get; set; }
    }
}
