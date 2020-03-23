using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books
{
    public class Return
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
            public string UserName { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly AppDbContext _ctx;

            public Handler(IUserAccessor userAccessor, AppDbContext ctx)
            {
                this._userAccessor = userAccessor;
                this._ctx = ctx;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _ctx.Users.Include(x => x.RentedBooks).SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken: cancellationToken);
                var book = await _ctx.Books.SingleOrDefaultAsync(x => x.BookId == request.BookId, cancellationToken: cancellationToken);

                var rentedBooks = user.RentedBooks.Where(x => x.BookId == request.BookId).ToList();
                if (rentedBooks != null)
                {
                    foreach (var rentedBook in rentedBooks)
                    {
                        user.RentedBooks.Remove(rentedBook);
                        rentedBook.IsAvailable = true;
                        rentedBook.ReturnDate = DateTime.MinValue;
                        rentedBook.RentDate = DateTime.MinValue;
                        book.AvailableCopies++;
                    }
                    await _ctx.SaveChangesAsync();
                }
                return Unit.Value;
            }
        }
    }
}
