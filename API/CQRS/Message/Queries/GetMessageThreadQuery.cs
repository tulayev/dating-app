using API.Helpers;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Queries
{
    public record GetMessageThreadQuery(string CurrentUserName, string RecipientUserName) : IRequest<ApiResponse<IEnumerable<MessageDto>>>;
}
