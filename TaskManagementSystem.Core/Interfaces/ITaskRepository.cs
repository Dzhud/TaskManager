using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Core.Entities;

namespace TaskManagementSystem.Core.Interfaces;

public interface ITaskRepository
{
    Task<WorkTask> GetByIdAsync(Guid id);
    Task<IEnumerable<WorkTask>> GetAllAsync();
    Task<WorkTask> CreateAsync(WorkTask task);
    Task<WorkTask> UpdateAsync(WorkTask task);
    Task DeleteAsync(Guid id);
}
