using Products.Repository.Interfaces;

namespace Products.Utility.Generator
{
    public static class UniqueIdentifierGenerator
    {
        private static readonly Random _random = new Random();

        public static async Task<int> GenerateUniqueIdAsync(IProductRepository repository)
        {
            int id;
            bool exists;
            do
            {
                id = _random.Next(100000, 1000000); // 6-digit number
                exists = await repository.ExistsAsync(id);
            } while (exists);

            return id;
        }
    }
}
