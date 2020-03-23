using Application.Errors;
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
    class Delete
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
                //if admin - delete logic
                var book = await _ctx.Books.SingleOrDefaultAsync(x => x.BookId == request.BookId, cancellationToken: cancellationToken);

                if(book != null)
                {
                    _ctx.Books.Remove(book);
                    if (await _ctx.SaveChangesAsync(cancellationToken) > 0)
                        return Unit.Value;
                    throw new Exception("Problem with deleting user!");
                }
                throw new RestException(HttpStatusCode.NotFound);
            }
        }
    }
}
