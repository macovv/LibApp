using Application.Errors;
using Application.Users.DTOs;
using AutoMapper;
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
        public class Query : IRequest<List<AppUserDto>> { }

        public class Handler : IRequestHandler<Query, List<AppUserDto>>
        {
            private readonly AppDbContext _ctx;
            private readonly IMapper _mapper;

            public Handler(AppDbContext ctx, IMapper mapper)
            {
                this._ctx = ctx;
                this._mapper = mapper;
            }

            public async Task<List<AppUserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _ctx.Users.ToListAsync(cancellationToken: cancellationToken);
                if (users == null)
                    throw new RestException(HttpStatusCode.NotFound);
                var usersDto = _mapper.Map<List<AppUser>, List<AppUserDto>>(users);
                return usersDto;
            }
        }
    }
}
