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

        public new IList<User> User { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        private DateTime _birthdays;
        private int _id;
        private bool _isMale;
        public async Task OnGetAsync()
        {
            var users = from name in _context.User
                        select name;

            if (!string.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.ToLower();
                if (DateTime.TryParse(SearchString, out _birthdays))
                {
                    string.Format("{0:d.MM.yyyy}", _birthdays);
                    users = users.Where(s => s.Birthday == _birthdays);
                }
                else if (int.TryParse(SearchString, out _id))
                {   
                    users = users.Where(s => s.ID == _id);
                }
                else if (bool.TryParse(SearchString, out _isMale))
                {
                    users = users.Where(s => s.IsMale == _isMale);
                }
                else
                {
                    users = users.Where(s => s.Name.ToLower().Contains(SearchString));
                }
            }
            User = await users.ToListAsync();
        }
    }
}
