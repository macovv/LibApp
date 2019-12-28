using Application.Books.Queries.DTOs;
using Application.Errors;
using AutoMapper;
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
        public class Query : IRequest<BookDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, BookDto>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDbContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<BookDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var book = await _context.Books.Include(x => x.BookCopies).SingleOrDefaultAsync(x => x.BookId == request.Id);
                if (book == null)
                    throw new RestException(HttpStatusCode.NotFound);
                var bookDto = _mapper.Map<Book, BookDto>(book);
                return bookDto;
            }
        }
    }
}
