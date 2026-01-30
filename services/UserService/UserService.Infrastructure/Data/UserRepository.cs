using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetUserByFirstNameAsync(string firstName)
        {
            return await _context.Users.Where(u => u.FirstName == firstName).AsNoTracking().ToListAsync();
        }
        public async Task<List<User>> GetUserByLastNameAsync(string lastName)
        {
            return await _context.Users.Where(u => u.LastName == lastName).AsNoTracking().ToListAsync();
        }
        public async Task<List<User>> GetUserByRoleAsync(string role)
        {
            return await _context.Users.Where(u => u.Role.ToString() == role).AsNoTracking().ToListAsync();
        }
    }
}
