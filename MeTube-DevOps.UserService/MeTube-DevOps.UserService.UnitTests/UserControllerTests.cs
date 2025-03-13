using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using MeTube_DevOps.UserService.Controllers;
using MeTube_DevOps.UserService.Entities;
using MeTube_DevOps.UserService.DTO;
using MeTube_DevOps.UserService.Repositories;

namespace MeTube_DevOps.UserService.UnitTests;

 public class UserControllerTests
 {
     private readonly Mock<IUnitOfWork> _mockUnitOfWork;
     private readonly Mock<IMapper> _mockMapper;
     private readonly UserController _controller;

     public UserControllerTests()
     {
         _mockUnitOfWork = new Mock<IUnitOfWork>();
         _mockMapper = new Mock<IMapper>();
         _controller = new UserController(_mockUnitOfWork.Object, _mockMapper.Object);

         // Setup ClaimsPrincipal för autentiserade anrop
         var claims = new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier, "1"), // Mockar inloggad användare med ID 1
             new Claim(ClaimTypes.Role, "Admin") // Om metoder kräver admin-behörighet
         };

         var identity = new ClaimsIdentity(claims, "TestAuthType");
         var claimsPrincipal = new ClaimsPrincipal(identity);

         _controller.ControllerContext = new ControllerContext
         {
             HttpContext = new DefaultHttpContext
             {
                 User = claimsPrincipal
             }
         };
     }

     [Fact]
     public async Task GetUser_ShouldReturnUser_WhenUserExists()
     {
         // Arrange
         var userId = 1;
         var user = new User 
         { 
             Id = userId, 
             Username = "TestUser", 
             Email = "test@example.com", 
             Password = "Svartlosenord",
             Role = "User" 
         
         };
         var userDto = new UserDto { Id = userId, Username = "TestUser", Email = "test@example.com" };

         _mockUnitOfWork.Setup(uow => uow.Users.GetUserByIdAsync(userId)).ReturnsAsync(user);
         _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

         // Act
         var result = await _controller.GetUser(userId);

         // Assert
         var okResult = Assert.IsType<OkObjectResult>(result);
         var returnedUser = Assert.IsType<UserDto>(okResult.Value);
         Assert.Equal(userDto.Username, returnedUser.Username);
     }

     [Fact]
     public async Task GetUser_ShouldReturnNotFound_WhenUserDoesNotExist()
     {
         // Arrange
         var userId = 1;
         _mockUnitOfWork.Setup(uow => uow.Users.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

         // Act
         var result = await _controller.GetUser(userId);

         // Assert
         Assert.IsType<NotFoundObjectResult>(result);
     }

[Fact]
    public async Task GetAllusers_ReturnsOkWithUsers_WhenUsersExist()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "User1", Email = "user1@example.com", Password = "password1", Role = "User" },
            new User { Id = 2, Username = "User2", Email = "user2@example.com", Password = "password2", Role = "User" }
        };
        var userDtos = new List<UserDto>
        {
            new UserDto { Id = 1, Username = "User1", Email = "user1@example.com" },
            new UserDto { Id = 2, Username = "User2", Email = "user2@example.com" }
        };

        _mockUnitOfWork.Setup(uow => uow.Users.GetAllAsync())
            .ReturnsAsync(users);
        
        _mockMapper.Setup(m => m.Map<IEnumerable<UserDto>>(users))
            .Returns(userDtos);

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count());
    }

     [Fact]
     public async Task SignUp_ShouldCreateUserSuccessfully()
     {
         var request = new CreateUserDto { Username = "NewUser", Email = "new@example.com", Password = "password" };
         var user = new User { Id = 1, Username = "NewUser", Email = "new@example.com", Password = "password", Role = "User" };

         _mockUnitOfWork.Setup(uow => uow.Users.GetUserByUsernameAsync(request.Username)).ReturnsAsync((User)null);
         _mockUnitOfWork.Setup(uow => uow.Users.GetUserByEmailAsync(request.Email)).ReturnsAsync((User)null);
         _mockMapper.Setup(m => m.Map<User>(request)).Returns(user);
         _mockUnitOfWork.Setup(uow => uow.Users.AddUserAsync(user)).Returns(Task.CompletedTask);
         _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

         var result = await _controller.SignUp(request);

         var okObjectResult = Assert.IsType<OkObjectResult>(result);
         var response = okObjectResult.Value;

         Assert.NotNull(response);
         Assert.True(response.GetType().GetProperty("Message") != null, "Response saknar Message");

         var message = response.GetType().GetProperty("Message")?.GetValue(response, null);
         Assert.NotNull(message);
         Assert.Equal("User signed up successfully", message);
     }

     [Fact]
     public async Task SignUp_ShouldReturnBadRequest_WhenUsernameExists()
     {
         var request = new CreateUserDto { Username = "ExistingUser", Email = "existing@example.com", Password = "password" };
         var existingUser = new User { Id = 1, Username = "ExistingUser", Email = "existing@example.com", Password = "password", Role = "User" };

         _mockUnitOfWork.Setup(uow => uow.Users.GetUserByUsernameAsync(request.Username)).ReturnsAsync(existingUser);

         var result = await _controller.SignUp(request);

         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
         var response = badRequestResult.Value;

         Assert.NotNull(response);
         Assert.True(response.GetType().GetProperty("Message") != null, "Response saknar Message");

         var message = response.GetType().GetProperty("Message")?.GetValue(response, null);
         Assert.NotNull(message);
         Assert.Equal("Username already exists", message);
     }
 }
