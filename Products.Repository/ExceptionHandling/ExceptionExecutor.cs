using Microsoft.EntityFrameworkCore;
using Products.Utility.Exceptions;

namespace Products.Repository.Exceptions
{
    public static class ExceptionExecutor
    {
        public static async Task<T> ExecuteWithExceptionHandling<T>(
            Func<Task<T>> func,
            string errorMessage,
            int statusCode = 500)
        {
            try
            {
                return await func();
            }
            catch (RepositoryException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new RepositoryException(RepositoryErrorMessages.UpdateNotFound, 404);
            }
            catch (Exception)
            {
                throw new RepositoryException(errorMessage, statusCode);
            }
        }
    }
}