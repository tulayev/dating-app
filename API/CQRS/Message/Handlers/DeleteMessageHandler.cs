using API.CQRS.Message.Commands;
using API.Helpers;
using API.Repositories;
using MediatR;

namespace API.CQRS.Message.Handlers
{
    public class DeleteMessageHandler : IRequestHandler<DeleteMessageCommand, ApiResponse<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteMessageHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Unit>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Unit>();
            var message = await _unitOfWork.MessageRepository.GetMessage(request.Id);

            if (message.Sender.UserName != request.UserName && message.Recipient.UserName != request.UserName)
            {
                response.ErrorMessage = "Unauthorized";
                return response;
            }

            if (message.Sender.UserName == request.UserName)
                message.SenderDeleted = true;

            if (message.Recipient.UserName == request.UserName)
                message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _unitOfWork.MessageRepository.DeleteMessage(message);

            await _unitOfWork.SaveChanges();

            return response;
        }
    }
}
