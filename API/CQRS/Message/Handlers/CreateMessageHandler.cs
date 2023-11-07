using API.CQRS.Message.Commands;
using API.Helpers;
using API.Repositories;
using AutoMapper;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Handlers
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand, ApiResponse<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CreateMessageHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<MessageDto>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<MessageDto>();

            if (request.UserName == request.CreateMessageDto.RecipientUserName.ToLower())
            {
                response.ErrorMessage = "You cannot send messages to yourself!";
                return response;
            }

            var sender = await _unitOfWork.UserRepository.GetUserByUserNameAsync(request.UserName);
            var recipient = await _unitOfWork.UserRepository.GetUserByUserNameAsync(request.CreateMessageDto.RecipientUserName);

            var message = new Models.Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = request.CreateMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            await _unitOfWork.SaveChanges();

            response.Data = _mapper.Map<MessageDto>(message);

            return response;
        }
    }
}
