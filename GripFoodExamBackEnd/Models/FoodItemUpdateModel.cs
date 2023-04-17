namespace GripFoodExamBackEnd.Models
{
    public class FoodItemUpdateModel
    {
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string RestaurantId { get; set; } = "";
    }
}
