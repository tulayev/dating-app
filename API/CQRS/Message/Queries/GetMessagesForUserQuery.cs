using API.Helpers;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Queries
{
    public record GetMessagesForUserQuery(MessageParams MessageParams, string UserName) : IRequest<ApiResponse<PagedList<MessageDto>>>;
}
