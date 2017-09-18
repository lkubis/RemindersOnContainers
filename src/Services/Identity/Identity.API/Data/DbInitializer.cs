using System;
using System.Threading.Tasks;
using Identity.API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data
{
    public class DbInitializer
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DbInitializer(
            DataContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (await _roleManager.RoleExistsAsync("ADMIN"))
                return;

            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            await _roleManager.CreateAsync(new ApplicationRole() { Id = Guid.NewGuid(), Name = "admin" });
        }

        private async Task SeedUsersAsync()
        {
            var adminAccount = new ApplicationUser()
            {
                Id = Guid.NewGuid(),
                Email = "admin@reminders-on-containers.com",
                UserName = "admin@reminders-on-containers.com"
            };
            await _userManager.CreateAsync(adminAccount, "Pa$$w0rd");
            await _userManager.AddToRoleAsync(adminAccount, "admin");
        }
    }
}