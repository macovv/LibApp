using Application.Errors;
using Application.Users.DTOs;
using AutoMapper;
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
        public class Query : IRequest<AppUserDto>
        {
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Query, AppUserDto>
        {
            private readonly AppDbContext _ctx;
            private readonly IMapper _mapper;

            public Handler(AppDbContext ctx, IMapper mapper)
            {
                this._ctx = ctx;
                this._mapper = mapper;
            }

            public async Task<AppUserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.Include(x => x.RentedBooks).SingleOrDefaultAsync(u => u.UserName == request.UserName);
                if(user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { user = "Not Found" });
                var userDto = _mapper.Map<AppUser, AppUserDto>(user);
                return userDto;
            }
        }
    }
}
