using API.Extensions;
using API.Helpers;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs.Message;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.UserName = User.GetUserName();

            var messages = await _unitOfWork.MessageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return Ok(messages);
        }
        
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            var currentUserName = User.GetUserName();

            return Ok(await _unitOfWork.MessageRepository.GetMessageThread(currentUserName, username));
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUserName();

            if (username == createMessageDto.RecipientUserName.ToLower())
                return BadRequest("You cannot send messages to yourself!");

            var sender = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null)
                return NotFound();

            var message = new Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete())
                return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send a message");
        }
    }
}
