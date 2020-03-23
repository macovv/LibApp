using Application.Errors;
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
    public class Update
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
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
                var book = await _ctx.Books.Where(x => x.BookId == request.BookId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if(book != null)
                {
                    book.Title = request.Title ?? book.Title;
                    book.Description = request.Description ?? book.Description;
                    _ctx.Update(book);
                    if(await _ctx.SaveChangesAsync(cancellationToken) > 0)
                        return Unit.Value;
                    throw new Exception("Problem with updating!");
                }
                throw new RestException(HttpStatusCode.NotFound);
            }
        }
    }
}
