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
    public class List
    {
        public class Query : IRequest<List<BookDto>> {}

        public class Handler : IRequestHandler<Query, List<BookDto>>
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public Handler(AppDbContext context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<List<BookDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var books = await _context.Books.ToListAsync(cancellationToken: cancellationToken);
                var booksDto = _mapper.Map<List<Book>, List<BookDto>>(books);
                if (books == null)
                    throw new RestException(HttpStatusCode.NotFound);
                return booksDto;
            }
        }
    }
}
