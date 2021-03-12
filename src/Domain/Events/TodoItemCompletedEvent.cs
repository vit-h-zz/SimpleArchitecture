using SimpleArchitecture.Domain.Common;
using SimpleArchitecture.Domain.Entities;

namespace SimpleArchitecture.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
