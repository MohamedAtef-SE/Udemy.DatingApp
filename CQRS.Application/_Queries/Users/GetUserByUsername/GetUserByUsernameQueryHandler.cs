using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Interfaces;
using MediatR;

namespace CQRS.Application._Queries.Users.GetUserByUsername
{
    internal class GetUserByUsernameQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetUserByUsernameQuery, Result<MemberDTO>>
    {
        public async Task<Result<MemberDTO>> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var memberDTO = await _unitOfWork.UserRepository.GetMemberByUsernameAsync(request.Username);
            if (memberDTO is not null) return Result<MemberDTO>.Success(memberDTO);

            return Result<MemberDTO>.Failure("member not found");
        }
    }
}
