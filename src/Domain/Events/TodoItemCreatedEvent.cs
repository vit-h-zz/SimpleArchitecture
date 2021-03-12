using SimpleArchitecture.Domain.Common;
using SimpleArchitecture.Domain.Entities;

namespace SimpleArchitecture.Domain.Events
{
    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
