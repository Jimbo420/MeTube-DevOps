using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MeTube_DevOps.UserService.Repositories;
using MeTube_DevOps.UserService.Data;
using MeTube_DevOps.UserService.Entities;
using AutoMapper;
using MeTube_DevOps.UserService.DTO;

namespace MeTube_DevOps.UserService.Controllers
{

  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    // GET: all users
    // Adding a comment
    // another one
    [HttpGet("manageUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        if (!users.Any())
            return NotFound(new { Message = "Users not found" });

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

        return Ok(userDtos);
    }

    // GET: user by id
    // Adding another comment
    // Another one
    // Another one 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _unitOfWork.Users.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { Message = "User is not found" });
        }
        var userDto = _mapper.Map<UserDto>(user);
        return Ok(userDto);
    }

    // POST: signup
    // Another one
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

      var user = _mapper.Map<User>(request);

      await _unitOfWork.Users.AddUserAsync(user);
      await _unitOfWork.SaveChangesAsync();

      return Ok(new { Message = "User signed up successfully" });
    }
  }
}
