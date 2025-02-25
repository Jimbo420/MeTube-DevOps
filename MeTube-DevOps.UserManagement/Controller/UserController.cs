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