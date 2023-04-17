namespace GripFoodExamBackEnd.Models
{
    public class FoodItemDetailModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string RestaurantId { get; set; } = "";
        public string RestaurantName { get; set; } = "";
    }
}
