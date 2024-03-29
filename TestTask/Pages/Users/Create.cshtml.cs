﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestTask.Data;
using TestTask.Models;

namespace TestTask
{
    public class CreateModel : PageModel
    {
        private readonly TestTask.Data.TestTaskContext _context;

        public CreateModel(TestTask.Data.TestTaskContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public new User User { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var entry = _context.Add(new User());
            entry.CurrentValues.SetValues(User);
            await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
        }
    }
}