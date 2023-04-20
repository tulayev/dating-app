using API.Extensions;
using API.Repository.IRepository;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Services.PhotoUploadService;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        private readonly IPhotoService _photoService;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _unitOfWork.GetQueryable<AppUser>()
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<MemberDto>>(users));
        }
        
        [HttpGet("{username}", Name = "GetUser")]
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
            var user = await _unitOfWork.GetQueryable<AppUser>()
                .FirstOrDefaultAsync(u => u.UserName == User.GetUserName());

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

        [HttpPost("upload-photo")]
        public async Task<ActionResult<PhotoDto>> UploadPhoto(IFormFile file)
        {
            var user = await _unitOfWork.GetQueryable<AppUser>()
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == User.GetUserName());
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) 
                return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
                photo.IsMain = true;

            user.Photos.Add(photo);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));
            }
            catch
            {
                return BadRequest("Failed to upload the photo!");
            }
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _unitOfWork.GetQueryable<AppUser>()
               .Include(u => u.Photos)
               .FirstOrDefaultAsync(u => u.UserName == User.GetUserName());

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo.IsMain)
                return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);

            if (currentMain != null)
                currentMain.IsMain = false;

            photo.IsMain = true;

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return NoContent();
            }
            catch
            {
                return BadRequest("Failed to set the main photo!");
            }
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _unitOfWork.GetQueryable<AppUser>()
              .Include(u => u.Photos)
              .FirstOrDefaultAsync(u => u.UserName == User.GetUserName());

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null)
                return NotFound();

            if (photo.IsMain)
                return BadRequest("You can't delete your main photo!");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest("Failed to delete the photo!");
            }

        }
    }
}
