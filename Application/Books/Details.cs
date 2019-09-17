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
    public class Details
    {
        public class Query : IRequest<Book>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Book>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                this._context = context;
            }

            public async Task<Book> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.Include(x => x.BookCopies).SingleOrDefaultAsync(x => x.BookId == request.Id);
                if (book == null)
                    throw new RestException(HttpStatusCode.NotFound);
                return book;
            }
        }
    }
}
