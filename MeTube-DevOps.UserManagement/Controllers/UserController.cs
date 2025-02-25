using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MeTube_DevOps.UserManagement.Repositories;
using MeTube_DevOps.UserManagement.Data;
using MeTube_DevOps.UserManagement.Entities;
using AutoMapper;



namespace MeTube_DevOps.UserManagement.Controllers
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
