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

namespace Application.Books
{
    public class AddCopy
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _ctx;

            public Handler(AppDbContext ctx)
            {
                this._ctx = ctx;
            }


            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = await _ctx.Books.SingleOrDefaultAsync(x => x.BookId == request.BookId);

                if(book != null)
                {
                    book.BookCopies = new List<Copy>();
                    book.BookCopies.Add(new Copy() { IsAvailable = true });
                    book.Quantity++;
                    book.AvailableCopies++;
                    if (await _ctx.SaveChangesAsync() > 0)
                    {
                        return Unit.Value;
                    }
                    throw new Exception("Problem with adding new copy!");
                }
                throw new RestException(HttpStatusCode.NotFound);
            }
        }

    }
}
