using API.Interfaces.Repositories;



namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get; }
        public ILikesRepository LikesRepository { get; }
        public IMessageRepository MessageRepository { get; }
        Task<bool> CompleteAsync();
        bool HasChanges();
    }
}
