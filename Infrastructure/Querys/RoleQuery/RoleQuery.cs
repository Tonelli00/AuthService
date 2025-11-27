using Application.Interfaces.Query;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Querys.RoleQuery
{
    public class RoleQuery : IRoleQuery
    {
        private readonly AppDbContext _context;

        public RoleQuery(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles.ToListAsync(cancellationToken);
        }

        public async Task<Role> GetByIdAsync(int roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(c => c.RoleId == roleId, cancellationToken);
        }
    }
}
