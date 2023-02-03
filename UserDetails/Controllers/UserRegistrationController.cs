using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserDetails.DAL;
using UserDetails.Models;
using UserDetails.Repository;
using UserDetails.Repository.Interface;

namespace UserDetails.Controllers
{
    [EnableCors("corsapp")]
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<UserRegistrationController> _logger;
        private readonly IUser _userRepository;

        public UserRegistrationController(ILogger<UserRegistrationController> logger, IUser user)
        {
            _logger = logger;
            _userRepository = user;
        }

        [HttpGet]
        public async Task<ActionResult<UserRegistration>> GetUser([FromBody] Guid userid)
        {
            try
            {
                var result = await _userRepository.GetUser(userid);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserDetails")]
        public async Task<ActionResult<UserRegistration>> GetLoggedInUserId()
        {
            var userid = Guid.Parse((HttpContext.User.FindFirstValue("UserId").ToString()));

            try
            {
                var result = await _userRepository.GetUser(userid);

                if (result == null)
                {
                    return NotFound();
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserRegistration>> Register([FromForm] UserRegistration user)
        {
            try
            {
                if (user == null)
                    return BadRequest();

                var createdUser = await _userRepository.Register(user);

                return CreatedAtAction(nameof(GetUser),
                    new { id = createdUser.UserId }, createdUser);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user record");
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UserVM user)
        {
            try
            {
                var userid = Guid.Parse((HttpContext.User.FindFirstValue("UserId").ToString()));

                if (userid != user.UserId)
                    return BadRequest("User ID mismatch");

                var userToUpdate = await _userRepository.GetUser(userid);

                if (userToUpdate == null)
                    return NotFound($"user with Id = {userid} not found");

                UserRegistration result = new UserRegistration
                {
                    UserId = userid,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = userToUpdate.Password,
                    EmailId = user.EmailId,
                    DOB = user.DOB,
                    Gender = user.Gender,
                    MobileNumber = user.MobileNumber,
                    ModifiedOn = DateTime.Now
                };

                await _userRepository.UpdateUser(result);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user record");
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var userid = Guid.Parse((HttpContext.User.FindFirstValue("UserId").ToString()));

                var userToUpdate = await _userRepository.GetUser(userid);

                if (userToUpdate == null)
                    return NotFound($"user with Id = {userid} not found");

                await _userRepository.DeleteUser(userid);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new user record");
            }
        }

    }
}