using MessageWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageWebAPI.Repositories
{
    public class CoreRepository : ICoreRepository
    {

        private readonly CoreDbContext _context;

        public CoreRepository(CoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _context.Messages.Where(m => !m.IsDeleted)
                .OrderBy(c => c.SubmittedAt).ToListAsync();
        }

        public async Task<bool> InsertMessageAsync(Message customer)
        {
            bool isSuccessful = false;

            _context.Add(customer);

            try
            {
                await _context.SaveChangesAsync();
                isSuccessful = true;
            }
            catch (Exception) { }

            return isSuccessful;
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            //Extra hop to the database but keeps it nice and simple for this demo
            var message = await _context.Messages.SingleOrDefaultAsync(c => c.Id == id);

            if (message == null) 
            {
                try
                {
                    message.IsDeleted = true;

                    return (await _context.SaveChangesAsync() > 0 ? true : false);
                }
                catch (Exception) { }
            }

            return false;
        }

    }
}
