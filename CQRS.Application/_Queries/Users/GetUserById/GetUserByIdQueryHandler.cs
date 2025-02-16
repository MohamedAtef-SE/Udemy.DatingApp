using CQRS.Application.DTOs.members;
using CQRS.Application.Helpers;
using CQRS.Infrastructure.Interfaces;
using MediatR;

namespace CQRS.Application._Queries.Users.GetUserById
{
    public class GetUserByIdQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetUserByIdQuery, Result<MemberDTO>>
    {
        public async Task<Result<MemberDTO>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var memberDTO = await _unitOfWork.UserRepository.GetMemberByIdAsync(request.Id);
            if(memberDTO is not null) return Result<MemberDTO>.Success(memberDTO);

            return Result<MemberDTO>.Failure("member not found");
        }
    }
}
