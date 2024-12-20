namespace API.Tests.Services;

public class UserServiceTests
{
    // private readonly Mock<IUserRepository> _mockRepository;
    // private readonly UserService _userService;
    //
    // public UserServiceTests()
    // {
    //     _mockRepository = new Mock<IUserRepository>();
    //     _userService = new UserService(_mockRepository.Object);
    // }
    //
    // [Fact]
    // public void CreateUser_ShouldCallRepositoryOnce()
    // {
    //     var newUser = new User
    //     {
    //         Id = 1,
    //         Email = "example@example.com",
    //         FullName = "John Doe",
    //         Role = UserRole.Student,
    //         CreatedAt = DateTime.Now
    //     };
    //
    //     _mockRepository.Setup(repo => repo.Add(It.IsAny<User>())).Verifiable();
    //
    //     _userService.CreateUser(newUser);
    //
    //     _mockRepository.Verify(repo => repo.Add(newUser), Times.Once());
    // }
    //
    // [Fact]
    // public void CreateUser_ShouldThrowException_WhenEmailIsAlreadyTaken()
    // {
    //     var existingUser = new User
    //     {
    //         Id = 1,
    //         FullName = "Jane",
    //         Email = "john@example.com",
    //         Role = UserRole.Student,
    //         CreatedAt = DateTime.UtcNow
    //     };
    //     var newUser = new User
    //     {
    //         Id = 2,
    //         FullName = "John",
    //         Email = "john@example.com",
    //         Role = UserRole.Student,
    //         CreatedAt = DateTime.UtcNow
    //     };
    //
    //     _mockRepository.Setup(repo => repo.GetByEmail(newUser.Email)).Returns(existingUser);
    //
    //     Assert.Throws<ArgumentException>(() => _userService.CreateUser(newUser));
    // }
}