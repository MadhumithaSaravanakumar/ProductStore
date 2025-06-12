namespace Products.Service.CommandResults
{
    public class IncrementStockResult
    {
        public bool Success { get; set; }
        public bool NotFound { get; set; }
        public int Stock { get; set; }

        public static IncrementStockResult NotFoundResult() => new IncrementStockResult { NotFound = true };
        public static IncrementStockResult SuccessResult(int stock) => new IncrementStockResult { Success = true, Stock = stock };
    }
}