namespace GripFoodExamBackEnd.Models
{
    public class AddToCartModel
    {
        public string FoodItemId { set; get; } = "";
        public string RestaurantId { get; set; } = "";
        public int Qty { set; get; }
    }
}
