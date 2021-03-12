using SimpleArchitecture.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace SimpleArchitecture.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
