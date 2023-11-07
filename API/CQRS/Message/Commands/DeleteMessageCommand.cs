using API.Helpers;
using MediatR;

namespace API.CQRS.Message.Commands
{
    public record DeleteMessageCommand(int Id, string UserName) : IRequest<ApiResponse<Unit>>;
}
