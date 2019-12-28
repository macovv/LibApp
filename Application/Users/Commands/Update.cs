using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    class Update
    {
        public class Command : IRequest
        {

        }

        public class Handler : IRequestHandler<Command>
        {
            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                /*
                 Add adress to AppUser and Phone and let user change it here
                 */
                throw new NotImplementedException();
            }
        }
    }
}
