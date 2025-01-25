using API.DTOs.members;
using API.Entities;
using API.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class UserRepository(DataContext _dataContext,IMapper _mapper) : IUserRepository
    {
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _dataContext.Users
                .Include(user => user.Photos)
                .ToListAsync();
        }
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _dataContext.Users
                .Include(user => user.Photos)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users
                .Include(user => user.Photos)
                .FirstOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _dataContext.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                           .ToListAsync();
        }
        public async Task<MemberDTO?> GetMemberByIdAsync(int id)
        {
            return await _dataContext.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<MemberDTO?> GetMemberByUsernameAsync(string username)
        {
            return await _dataContext.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                                           .FirstOrDefaultAsync(user => user.UserName == username);
        }

        public void Update(AppUser user)
        {
            _dataContext.Users.Update(user);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
