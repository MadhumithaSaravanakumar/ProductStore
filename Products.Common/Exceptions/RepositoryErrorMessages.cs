namespace Products.Utility.Exceptions
{
    public static class RepositoryErrorMessages
    {
        public const string GetAllFailed = "Failed to retrieve products.";
        public const string GetByIdFailed = "Failed to retrieve product.";
        public const string ProductNotFound = "Product not found.";
        public const string AddFailed = "Failed to add product.";
        public const string UpdateFailed = "Failed to update product.";
        public const string UpdateNotFound = "Product not found for update.";
        public const string DeleteFailed = "Failed to delete product.";
        public const string DeleteNotFound = "Product not found for deletion.";
        public const string ExistsFailed = "Failed to check product existence.";
    }
}