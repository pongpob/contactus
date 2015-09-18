using ContactUs.Models.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactUs.Models {
  public class Ticket {

    public string Id { get; set; }

    public DateTime LastActivityDate { get; set; }
    public string LastActivityByUser { get; set; }

    public string Title { get; set; }
    public string Body { get; set; }

    public TicketState CurrentState {
      get {
        return TicketStates
          .OrderBy(t => t.Date)
          .LastOrDefault();
      }
    }

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketState> TicketStates { get; set; }

    public Ticket() {
      TicketStates = new HashSet<TicketState>();

      //var t = new NewTicketState(this);
      //TicketStates.Add(t); 
    }

    internal void ChangeStatus(TicketState state) {
      TicketStates.Add(state); 
    }

    public TicketStatus Status {
      get { return CurrentState.Status; }
    }

    public bool IsAcceptable {
      get { return CurrentState.IsAcceptable; }
    }

    public bool IsRejectable {
      get { return CurrentState.IsRejectable; }
    }

    public bool IsCloseable {
      get { return CurrentState.IsCloseable; }
    }

    public void Accept() {
        if (!this.IsAcceptable)
        {
            throw new InvalidOperationException();
        }
      ChangeStatus(new AcceptedTicketState(this));
    }

    public void Close() {
        if (!this.IsCloseable)
        {
            throw new InvalidOperationException();
        }
      ChangeStatus(new ClosedTicketState(this));
    }
    public void Reject(string reason) {
        if (!this.IsRejectable)
        {
            throw new InvalidOperationException();
        }
        var s = new RejectedTicketState(this);
        s.Reason = reason;
      ChangeStatus(s);
    }

  }
}

        //public Ticket()
        //{
        //    this.Status = TicketStatus.New;
        //}
        //public bool AbleToChangeStatus(TicketStatus newStatus)
        //{
        //    if (this.Status == TicketStatus.New)
        //    {
        //        switch (newStatus)
        //        {
        //            case TicketStatus.Accepted:
        //                return true;
        //            case TicketStatus.Rejected:
        //                return true;
        //            case TicketStatus.Closed:
        //                return false;
        //            case TicketStatus.New:
        //                return false;
        //        }
        //    }
        //    else if (this.Status == TicketStatus.Accepted)
        //    {
        //        switch (newStatus)
        //        {
        //            case TicketStatus.Rejected:
        //                return true;
        //            case TicketStatus.Closed:
        //                return true;
        //        }
        //    }
        //    else if (this.Status == TicketStatus.Closed ||
        //               this.Status == TicketStatus.Rejected)
        //    {
        //        return false;
        //    }

        //    return false;
        //}

        //public void Accept()
        //{
        //    Status = TicketStatus.Accepted;
        //}
        //public void Close()
        //{
        //    Status = TicketStatus.Closed;
        //}
        //public void Reject()
        //{
        //    Status = TicketStatus.Rejected;
        //}
        //    }
        //}
