using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Books.Queries.DTOs
{
    public class BookDto 
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int AvailableCopies { get; set; }
    }
}
