namespace GripFoodExamBackEnd.Entities
{
    public class CartDetail
    {
        public string Id { get; set; } = "";
        public string CartId { get; set; } = "";
        public Cart Cart { get; set; } = null!;
        public string FoodItemId { get; set; } = "";
        public FoodItem FoodItem { get; set; } = null!;
        public int Qty { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}