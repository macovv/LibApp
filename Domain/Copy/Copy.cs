using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Copy
    {
        public int CopyId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public AppUser CurrentUser { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
