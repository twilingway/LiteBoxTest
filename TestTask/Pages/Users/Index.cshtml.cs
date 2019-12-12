using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;

namespace TestTask
{
    public class IndexModel : PageModel
    {
        private readonly TestTask.Data.TestTaskContext _context;

        public IndexModel(TestTask.Data.TestTaskContext context)
        {
            _context = context;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public new PaginatedList<User> Users { get; set; }
        
        private DateTime _birthdays;
        private int _id;
        private bool _isMale;
        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<User> users = from user in _context.User
                                     select user;


            if (!string.IsNullOrEmpty(searchString))
            {
                if (DateTime.TryParse(searchString, out _birthdays))
                {
                    string.Format("{0:d.MM.yyyy}", _birthdays);
                    users = users.Where(s => s.Birthday == _birthdays);
                }
                else if (int.TryParse(searchString, out _id))
                {
                    users = users.Where(s => s.ID == _id);
                }
                else if (bool.TryParse(searchString, out _isMale))
                {
                    users = users.Where(s => s.IsMale == _isMale);
                }
                else
                {
                    users = users.Where(s => s.Name.Contains(searchString));
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    users = users.OrderBy(s => s.Birthday);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.Birthday);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            Users = await PaginatedList<User>.CreateAsync(
                users.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
