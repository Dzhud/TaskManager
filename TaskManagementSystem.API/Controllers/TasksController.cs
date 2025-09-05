using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.API.DTOs;
using TaskManagementSystem.Core.Interfaces;

namespace TaskManagementSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    /// <summary>
    /// Gets all tasks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        var tasks = await _taskRepository.GetAllAsync();
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            DueDateTime = t.DueDateTime,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });
        return Ok(taskDtos);
    }

    /// <summary>
    /// Gets a task by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetById(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(id);
            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDateTime = task.DueDateTime,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
            return Ok(taskDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Creates a new task
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Create(CreateTaskDto createTaskDto)
    {
        var task = new Core.Entities.WorkTask
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Status = Core.Entities.WorkTaskStatus.Todo,
            DueDateTime = createTaskDto.DueDateTime
        };

        var createdTask = await _taskRepository.CreateAsync(task);
        var taskDto = new TaskDto
        {
            Id = createdTask.Id,
            Title = createdTask.Title,
            Description = createdTask.Description,
            Status = createdTask.Status,
            DueDateTime = createdTask.DueDateTime,
            CreatedAt = createdTask.CreatedAt,
            UpdatedAt = createdTask.UpdatedAt
        };

        return CreatedAtAction(nameof(GetById), new { id = taskDto.Id }, taskDto);
    }

    /// <summary>
    /// Updates an existing task
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskDto>> Update(Guid id, UpdateTaskDto updateTaskDto)
    {
        try
        {
            var existingTask = await _taskRepository.GetByIdAsync(id);
            existingTask.Title = updateTaskDto.Title;
            existingTask.Description = updateTaskDto.Description;
            existingTask.Status = updateTaskDto.Status;
            existingTask.DueDateTime = updateTaskDto.DueDateTime;

            var updatedTask = await _taskRepository.UpdateAsync(existingTask);
            var taskDto = new TaskDto
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                Status = updatedTask.Status,
                DueDateTime = updatedTask.DueDateTime,
                CreatedAt = updatedTask.CreatedAt,
                UpdatedAt = updatedTask.UpdatedAt
            };

            return Ok(taskDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deletes a task
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
