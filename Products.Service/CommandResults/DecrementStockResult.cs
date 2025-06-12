namespace Products.Service.CommandResults
{
    public class DecrementStockResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public int Stock { get; set; }

        public static DecrementStockResult NotFoundResult() => new DecrementStockResult { NotFound = true };
        public static DecrementStockResult SuccessResult(int stock) => new DecrementStockResult { Success = true, Stock = stock };
    }
}