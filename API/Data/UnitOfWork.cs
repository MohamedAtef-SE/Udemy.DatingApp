using API.Interfaces;
using API.Interfaces.Repositories;

namespace API.Data
{
    public class UnitOfWork(IUserRepository userRepository,ILikesRepository likesRepository, IMessageRepository messageRepository, DataContext dbContext) : IUnitOfWork
    {
        public IUserRepository UserRepository => userRepository;

        public ILikesRepository LikesRepository => likesRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public async Task<bool> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return dbContext.ChangeTracker.HasChanges();
        }
    }
}
