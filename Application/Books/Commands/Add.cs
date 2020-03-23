using Application.Errors;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Books
{
    public class Add
    {
        public class Command : IRequest
        {
            public int BookId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                this._context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var book = new Book
                {
                    BookId = request.BookId,
                    Title = request.Title,
                    Description = request.Description,
                    Quantity = request.Quantity,
                    AvailableCopies = request.Quantity,
                    Added = DateTime.Now,
                    BookCopies = new List<Copy>(),
                };

                // to remove later and change the model maybe
                for (int i = 0; i < book.Quantity; i++)
                {
                    book.BookCopies.Add(new Copy() { Book = book, IsAvailable = true });
                }

                _context.Books.Add(book);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                    return Unit.Value;
                throw new RestException(HttpStatusCode.BadRequest);
            }
        }
    }
}
