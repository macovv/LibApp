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
    public class List
    {
        public class Query : IRequest<List<Book>> {}

        public class Handler : IRequestHandler<Query, List<Book>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                this._context = context;
            }

            public async Task<List<Book>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Books.ToListAsync();
                if (books == null)
                    throw new RestException(HttpStatusCode.NotFound);
                return books;
            }
        }
    }
}
