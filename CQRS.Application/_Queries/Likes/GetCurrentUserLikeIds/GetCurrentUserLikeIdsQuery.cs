using CQRS.Application.Helpers;
using MediatR;

namespace CQRS.Application._Queries.Likes.GetCurrentUserLikeIds
{
    public record GetCurrentUserLikeIdsQuery:IRequest<Result<IEnumerable<int>>>;
    
}
