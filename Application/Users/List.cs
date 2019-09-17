using Application.Errors;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class List
    {
        public class Query : IRequest<List<AppUser>> { }

        public class Handler : IRequestHandler<Query, List<AppUser>>
        {
            private readonly AppDbContext _ctx;

            public Handler(AppDbContext ctx)
            {
                this._ctx = ctx;
            }

            public async Task<List<AppUser>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _ctx.Users.ToListAsync();
                if (users == null)
                    throw new RestException(HttpStatusCode.NotFound);
                return users;
            }
        }
    }
}
