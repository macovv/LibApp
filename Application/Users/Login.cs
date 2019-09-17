using Application.Errors;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SignInManager<AppUser> _signInManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtService _jwtService;

            public Handler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IJwtService jwtService)
            {
                this._signInManager = signInManager;
                this._userManager = userManager;
                this._jwtService = jwtService;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                var loggedUser = await _userManager.FindByEmailAsync(request.Email);
                var result = await _signInManager.PasswordSignInAsync(loggedUser.UserName, request.Password, false, false);
                if(result.Succeeded)
                {
                    var token = _jwtService.CreateToken(loggedUser);
                    return new User { Token = token, Email = loggedUser.Email, UserName = loggedUser.UserName };
                }
                throw new RestException(HttpStatusCode.BadRequest);
            }
        }
    }
}
