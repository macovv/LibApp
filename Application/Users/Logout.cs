using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Logout
    {
        public class Command : IRequest { }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SignInManager<AppUser> _signManager;

            public Handler(SignInManager<AppUser> signInManager)
            {
                this._signManager = signInManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                await _signManager.SignOutAsync();
                return Unit.Value;
            }
        }
    }
}
