using SimpleArchitecture.Application.Common.Mappings;
using SimpleArchitecture.Domain.Entities;

namespace SimpleArchitecture.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
