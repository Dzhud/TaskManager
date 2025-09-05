using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Core.Entities.WorkTask> GetByIdAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
            throw new KeyNotFoundException($"Task with ID {id} not found");
        return task;
    }

    public async Task<IEnumerable<Core.Entities.WorkTask>> GetAllAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<Core.Entities.WorkTask> CreateAsync(Core.Entities.WorkTask task)
    {
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<Core.Entities.WorkTask> UpdateAsync(Core.Entities.WorkTask task)
    {
        var existingTask = await GetByIdAsync(task.Id);
        
        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.Status = task.Status;
        existingTask.DueDateTime = task.DueDateTime;
        existingTask.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingTask;
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await GetByIdAsync(id);
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
