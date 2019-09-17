using Application.Errors;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Details
    {
        public class Query : IRequest<AppUser>
        {
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, AppUser>
        {
            private readonly AppDbContext _ctx;

            public Handler(AppDbContext ctx)
            {
                this._ctx = ctx;
            }

            public async Task<AppUser> Handle(Query request, CancellationToken cancellationToken)
            {
                //var user = await _ctx.Users.Include(x => x.RentedBooks).ThenInclude(w => w.).SingleOrDefaultAsync(u => u.UserName == request.UserName);
                var user = await _ctx.Users.Include(x => x.RentedBooks).ThenInclude(w => w.Book).SingleOrDefaultAsync(u => u.UserName == request.UserName);
                if(user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { user = "Not Found" });
                return user;
            }
        }
    }
}
