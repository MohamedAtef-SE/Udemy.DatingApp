using API.DTOs.members;
using API.Entities;
using API.Interfaces.Repositories;
using API.Pagination;
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
            return await _dataContext.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await _dataContext.Users
                .Include(user => user.Photos)
                .FirstOrDefaultAsync(user => user.UserName == username);
        }

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserQueryParams userParams)
        {
            var baseQuery = _dataContext.Users.Where(member => member.UserName != userParams.CurrentUsername);

            //// Correct and more accurate but i can't understand it
            //var minDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            //baseQuery = baseQuery.Where(member => member.DateOfBirth >= minDOB);

            //var maxDOB = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
            //baseQuery = baseQuery.Where(member => member.DateOfBirth <= maxDOB);

            if (userParams.Gender is not null)
            {
                baseQuery = baseQuery.Where(member => member.Gender == userParams.Gender);
            }

            if(userParams.MaxAge is not null)
            {
                baseQuery = baseQuery.Where(member => (DateTime.Now.Year - member.DateOfBirth.Year) <= userParams.MaxAge);
            }

            if (userParams.MinAge is not null)
            {
                baseQuery = baseQuery.Where(member => (DateTime.Now.Year - member.DateOfBirth.Year) >= userParams.MinAge);
            }

            if (userParams.City is not null)
            {
                baseQuery = baseQuery.Where(member => member.City == userParams.City);
            }

            if (userParams.Country is not null)
            {
                baseQuery = baseQuery.Where(member => member.Country == userParams.Country);
            }

            baseQuery = userParams.OrderBy switch
            {
                "created" => baseQuery.OrderByDescending(x => x.Created),
                _ => baseQuery.OrderByDescending(x => x.LastActive)
            };

            var query = baseQuery.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider);

            return await PagedList<MemberDTO>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
                                          
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
