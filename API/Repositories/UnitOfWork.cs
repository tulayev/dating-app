using API.Repositories.Repository.Like;
using API.Repositories.Repository.Message;
using API.Repositories.Repository.User;
using AutoMapper;
using Data;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public ILikeRepository LikesRepository => new LikeRepository(_context, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            _context.ChangeTracker.DetectChanges();
            var changes = _context.ChangeTracker.HasChanges();

            return changes;
        }
    }
}
