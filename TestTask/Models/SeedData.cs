using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Data;

namespace TestTask.Models
{
    public class SeedData
    {
        Random random = new Random();
        DateTime start = new DateTime(2010, 1, 1);
        int range;
        public async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TestTaskContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TestTaskContext>>()))
            {
               // context.Database.EnsureCreated();
                // Look for any movies.
                if (context.User.Any())
                {
                    return;   // DB has been seeded
                }
                await Task.Run(() =>
                {
                    context.User.AddRange(GetSampleUsers(50));
                    context.SaveChanges();
                });
            }
        }

        private IEnumerable<User> GetSampleUsers(int count)
        {
            for (int i = 0; i < count; i++)
                yield return new User 
                {
                    Name = "TestName" + i,
                    Birthday = RandomDay(),
                    IsMale = random.Next(100) <= 50 ? true : false,
                    Request = random.Next(30)
                };
        }

        private DateTime RandomDay()
        {
            range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
