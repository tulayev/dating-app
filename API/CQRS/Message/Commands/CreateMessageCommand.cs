using API.Helpers;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Commands
{
    public record CreateMessageCommand(CreateMessageDto CreateMessageDto, string UserName) : IRequest<ApiResponse<MessageDto>>;
}
