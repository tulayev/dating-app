using API.CQRS.Message.Queries;
using API.Helpers;
using API.Repositories;
using MediatR;
using Models.DTOs.Message;

namespace API.CQRS.Message.Handlers
{
    public class GetMessagesForUserHandler : IRequestHandler<GetMessagesForUserQuery, ApiResponse<PagedList<MessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMessagesForUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<PagedList<MessageDto>>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            request.MessageParams.UserName = request.UserName;
            
            var response = new ApiResponse<PagedList<MessageDto>>()
            {
                Data = await _unitOfWork.MessageRepository.GetMessageForUser(request.MessageParams)
            };

            return response;
        }
    }
}
