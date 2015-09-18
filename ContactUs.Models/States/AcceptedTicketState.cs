using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Models.States
{
    public class AcceptedTicketState: TicketState
    {
        public override TicketStatus Status
        {
            get { return TicketStatus.Accepted; }
        }

        public AcceptedTicketState()
        {

        }

        public AcceptedTicketState(Ticket ticket):base(ticket)
        {

        }

        public override bool IsCloseable
        {
            get{ return true;}
        }

        public override bool IsRejectable
        {
            get { return true; }
        }
    }
}
