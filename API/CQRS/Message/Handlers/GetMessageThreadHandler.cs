using API.CQRS.Message.Queries;
using API.Helpers;
using API.Repositories;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Handlers
{
    public class GetMessageThreadHandler : IRequestHandler<GetMessageThreadQuery, ApiResponse<IEnumerable<MessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessageThreadHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<MessageDto>>> Handle(GetMessageThreadQuery request, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<IEnumerable<MessageDto>>
            {
                Data = await _unitOfWork.MessageRepository.GetMessageThread(request.CurrentUserName, request.RecipientUserName)
            };
            return response;
        }
    }
}
