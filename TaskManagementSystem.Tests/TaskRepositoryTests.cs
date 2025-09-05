using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.Entities;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Repositories;

namespace TaskManagementSystem.Tests;

public class TaskRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly ApplicationDbContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new ApplicationDbContext(_options);
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_ShouldSetIdAndCreatedAt()
    {
        // Arrange
        var task = new WorkTask
        {
            Title = "Test Task",
            Description = "Test Description",
            Status = WorkTaskStatus.Todo,
            DueDateTime = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await _repository.CreateAsync(task);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.NotEqual(default, result.CreatedAt);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetById_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new WorkTask
        {
            Title = "Test Task",
            Description = "Test Description",
            Status = WorkTaskStatus.Todo,
            DueDateTime = DateTime.UtcNow.AddDays(1)
        };
        var created = await _repository.CreateAsync(task);

        // Act
        var result = await _repository.GetByIdAsync(created.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.Title, result.Title);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetById_ShouldThrowException_WhenTaskDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repository.GetByIdAsync(nonExistentId));
    }

    [Fact]
    public async System.Threading.Tasks.Task UpdateTask_ShouldUpdateProperties()
    {
        // Arrange
        var task = new WorkTask
        {
            Title = "Original Title",
            Description = "Original Description",
            Status = WorkTaskStatus.Todo,
            DueDateTime = DateTime.UtcNow.AddDays(1)
        };
        var created = await _repository.CreateAsync(task);

        // Act
        created.Title = "Updated Title";
        created.Status = WorkTaskStatus.InProgress;
        var result = await _repository.UpdateAsync(created);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal(WorkTaskStatus.InProgress, result.Status);
        Assert.NotNull(result.UpdatedAt);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteTask_ShouldRemoveTask()
    {
        // Arrange
        var task = new WorkTask
        {
            Title = "Test Task",
            Description = "Test Description",
            Status = WorkTaskStatus.Todo,
            DueDateTime = DateTime.UtcNow.AddDays(1)
        };
        var created = await _repository.CreateAsync(task);

        // Act
        await _repository.DeleteAsync(created.Id);

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _repository.GetByIdAsync(created.Id));
    }
}
