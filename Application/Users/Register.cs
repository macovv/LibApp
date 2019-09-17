using Application.Errors;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Register
    {
        public class Command : IRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext context;
            private readonly SignInManager<AppUser> signInManager;
            private readonly UserManager<AppUser> userManager;

            public Handler(AppDbContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
            {
                this.context = context;
                this.signInManager = signInManager;
                this.userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //automappper and delete unused fields
                var userToCreate = new AppUser()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    UserName = request.UserName
                };

                var result = await userManager.CreateAsync(userToCreate, request.Password);
                if(result.Succeeded)
                {
                    return Unit.Value;
                }
                throw new RestException(HttpStatusCode.BadRequest);
            }
        }
    }
}
