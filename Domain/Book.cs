using System;
using System.Collections.Generic;

namespace Domain
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public int Quantity { get; set; }
        public ICollection<Copy> BookCopies { get; set; }
        public int AvailableCopies { get; set; }
    }
}
