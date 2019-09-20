using Application.Errors;
using Domain;
using FluentValidation;
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

namespace Application.Books
{
    public class AddCopy
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
            public int Amount { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            }
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
                var book = await _ctx.Books.Include(x => x.BookCopies).SingleOrDefaultAsync(x => x.BookId == request.BookId);

                if(book != null)
                {
                    for(int i = 0; i < request.Amount; i++)
                    {
                        book.BookCopies.Add(new Copy() { IsAvailable = true });
                    }
                    book.Quantity = book.BookCopies.Count();
                    book.AvailableCopies = book.Quantity - book.BookCopies.Where(x => x.IsAvailable == false).Count();
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
