using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Models.States
{
    [Table("RejectedTicketStates")]
    public class RejectedTicketState : TicketState
    {
        public string Reason { get; set; }
        public override TicketStatus Status
        {
            get { return TicketStatus.Rejected; }
        }

        public RejectedTicketState()
        {

        }

        public RejectedTicketState(Ticket ticket):base(ticket)
        {

        }

        public override bool IsCloseable
        {
            get{ return true;}
        }

        public override bool IsAcceptable
        {
            get { return true; }
        }
    }
}
