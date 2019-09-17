using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books
{
    public class Rent
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _ctx;
            private readonly IUserAccessor _userAccessor;

            public Handler(AppDbContext ctx, IUserAccessor userAccessor)
            {
                this._ctx = ctx;
                this._userAccessor = userAccessor;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.Include(x => x.RentedBooks).SingleOrDefaultAsync(x => x.UserName == _userAccessor.getUserName());
                var book = await _ctx.Books.Include(x => x.BookCopies).SingleOrDefaultAsync(x => x.BookId == request.BookId);

                if (book.AvailableCopies > 0)
                {
                    Console.WriteLine("========================================="+book.BookCopies.First().Book.BookId);
                    var copy = book.BookCopies.Where(x => x.IsAvailable == true).FirstOrDefault();
                    copy.IsAvailable = false;
                    book.AvailableCopies--;
                    user.RentedBooks.Add(copy);
                    await _ctx.SaveChangesAsync();
                }

                return Unit.Value;
            }
        }
    }
}
