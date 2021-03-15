using SimpleArchitecture.Domain.Entities;
using SimpleArchitecture.Domain.ValueObjects;
using SimpleArchitecture.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using SimpleArchitecture.Domain.Enums;

namespace SimpleArchitecture.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole("Admin");

            if (roleManager.Roles.All(r => r.Name != adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            var admin = new ApplicationUser { UserName = "admin@localhost", Email = "admin@localhost" };

            if (userManager.Users.All(u => u.UserName != admin.UserName))
            {
                await userManager.CreateAsync(admin, "Admin1!");
                await userManager.AddToRolesAsync(admin, new [] { adminRole.Name });
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Colour = Colour.Blue,
                    Items =
                    {
                        new TodoItem { Title = "Apples", Done = true, Priority = PriorityLevel.High, Note = "Green сrispy"},
                        new TodoItem { Title = "Milk", Done = true, Priority = PriorityLevel.High, Note = "Organic" },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
                });

                context.TodoLists.Add(new Faker<TodoList>()
                    .RuleFor(l => l.Title, "Visit clients")
                    .RuleFor(l => l.Colour, f => f.PickRandom(Colour.SupportedColours))
                    .RuleFor(l => l.Items,
                        faker => new Faker<TodoItem>()
                            .RuleFor(i => i.Title, f => f.Name.FullName())
                            .RuleFor(i => i.Note, f => f.Address.FullAddress())
                            .RuleFor(i => i.Done, f => f.Random.Bool())
                            .RuleFor(i => i.Priority, f => f.PickRandom<PriorityLevel>())
                            .RuleFor(i => i.Reminder,
                                (f, l) => l.Priority == PriorityLevel.High ? f.Date.Soon(days: 5) : null)
                            .Generate(faker.Random.Number(5, 15))
                    )
                    .Generate());

                await context.SaveChangesAsync();
            }
        }
    }
}
