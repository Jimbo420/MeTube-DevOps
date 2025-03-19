using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MeTube_DevOps.UserService.Repositories;
using MeTube_DevOps.UserService.Data;
using MeTube_DevOps.UserService.Entities;
//using AutoMapper;
using MeTube_DevOps.UserService.DTO;

namespace MeTube_DevOps.UserService.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper? _mapper;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      //_mapper = mapper;
    }

    // GET: all users
    // GET: api/user/manageUsers
    [HttpGet("manageUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _unitOfWork.Users.GetAllAsync();
      if (!users.Any())
        return NotFound(new { Message = "Users not found" });

      var userDtos = < IEnumerable < UserDto >> (users);

      return Ok(userDtos);
    }

    // GET: user by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _unitOfWork.Users.GetUserByIdAsync(id);
      if (user == null)
      {
        return NotFound(new { Message = "User is not found" });
      }
      var userDto = < UserDto > (user);
      return Ok(userDto);
    }

    // POST: signup
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto request)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      if (await _unitOfWork.Users.GetUserByUsernameAsync(request.Username) != null)
      {
        return BadRequest(new { Message = "Username already exists" });
      }

      if (await _unitOfWork.Users.GetUserByEmailAsync(request.Email) != null)
      {
        return BadRequest(new { Message = "Email already exists" });
      }

      var user = < User > (request);

      await _unitOfWork.Users.AddUserAsync(user);
      await _unitOfWork.SaveChangesAsync();

      return Ok(new { Message = "User signed up successfully" });
    }

    // DELETE: remove user by username
    // DELETE: api/user/byUsername/{username}
    [HttpDelete("byUsername/{username}")]
    public async Task<IActionResult> DeleteUserByUsername(string username)
    {
      var user = await _unitOfWork.Users.GetUserByUsernameAsync(username);
      if (user == null)
      {
        return NotFound(new { Message = "User not found" });
      }

      await _unitOfWork.Users.RemoveAsync(user);
      await _unitOfWork.SaveChangesAsync();

      return Ok(new { Message = $"User with username '{username}' has been deleted successfully" });
    }
  }
}