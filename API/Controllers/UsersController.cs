using API.Repository.IRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _unitOfWork.GetQueryable<AppUser>()
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _unitOfWork.GetQueryable<AppUser>()
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return Ok(_mapper.Map<MemberDto>(user));
        }

        [HttpPut] 
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _unitOfWork.GetQueryable<AppUser>()
                .FirstOrDefaultAsync(u => u.UserName == username);

            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.Update(user);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("Failed to update user!");
            }
        }
    }
}
