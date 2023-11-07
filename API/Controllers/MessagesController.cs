using API.CQRS.Message.Commands;
using API.CQRS.Message.Queries;
using API.Extensions;
using API.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Message;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public MessagesController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<MessageDto>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            var result = await _mediator.Send(new GetMessagesForUserQuery(messageParams, User.GetUserName()));

            Response.AddPaginationHeader(result.Data.CurrentPage, result.Data.PageSize, result.Data.TotalCount, result.Data.TotalPages);

            return Ok(result.Data);
        }
        
        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username)
        {
            return Ok(await _mediator.Send(new GetMessageThreadQuery(User.GetUserName(), username)));
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            return Ok(await _mediator.Send(new CreateMessageCommand(createMessageDto, User.GetUserName())));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            return Ok(await _mediator.Send(new DeleteMessageCommand(id, User.GetUserName())));
        }
    }
}
