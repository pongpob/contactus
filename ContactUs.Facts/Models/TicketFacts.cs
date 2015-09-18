using ContactUs.Models;
using ContactUs.Models.States;
using ContactUs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactUs.Facts.Models
{
    public class TicketFacts
    {
        public class IsAbleToChangeStatus
        {
            [Fact]
            public void TicketMustAddToServiceBeforeUse()
            {
                // the incorrect way
                var t = new Ticket();
                Assert.ThrowsAny<NullReferenceException>(() =>
                {
                    // Now, Ticket is no state!
                    //
                    // Cannot get status before add to service,
                    // because we add "New" status after adding to service.
                    //
                    // If we add "New" status inside Ticket's constructor,
                    // Entity Framework will always append the "New" state 
                    // to recreated Ticket.  
                    var s = t.Status;
                });

                // the correct way
                using (var app = new App(testing: true))
                {
                    var t2 = new Ticket();

                    app.Tickets.Add(t2);

                    Assert.NotNull(t2.Status);
                }
            }

            // 1. NewTicketStatus_ShouldBeNew
            [Fact]
            public void NewTicketStatus_ShouldBeNew()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);

                    Assert.Equal(TicketStatus.New, t.Status);
                }
            }

            // 2. NewTicket_AbleToChangeToAcceptedAndRejected_ButNotClosed
            [Fact]
            public void NewTicket_AbleToChangeToAcceptedAndRejected_ButNotClosed()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();

                    app.Tickets.Add(t);
                    Assert.True(t.IsAcceptable);
                    Assert.True(t.IsRejectable);
                    Assert.False(t.IsCloseable);
                }
            }


            // 3. AcceptedTicket_AbleToChangeToClosedOrRejected
            [Fact]
            public void AcceptedTicket_AbleToChangeToClosedOrRejected()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);

                    t.Accept();

                    Assert.False(t.IsAcceptable);
                    Assert.True(t.IsRejectable);
                    Assert.True(t.IsCloseable);
                }
            }

            // 4. ClosedTicket_CannotChangeStatusAnymore
            [Fact]
            public void ClosedTicket_CannotChangeStatusAnymore()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);
                    t.Accept();
                    t.Close();

                    Assert.False(t.IsAcceptable);
                    Assert.False(t.IsRejectable);
                    Assert.False(t.IsCloseable);
                }
            }

            // 5. RejectedTicket_CannotChangeStatusAnymore
            public void RejectedTicket_CannotChangeStatusAnymore()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);
                    t.Accept();
                    t.Reject("test reject");

                    Assert.False(t.IsAcceptable);
                    Assert.False(t.IsRejectable);
                    Assert.False(t.IsCloseable);
                }
            }

            [Fact]
            public void ChangeFromNewToAccepted()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);

                    Assert.True(t.Status == TicketStatus.New);
                    Assert.Equal(1, t.TicketStates.Count());

                    t.Accept();

                    Assert.True(t.Status == TicketStatus.Accepted);
                    Assert.Equal(2, t.TicketStates.Count());
                }
            }

            [Fact]
            public void ChangeFromNewToRejected()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);

                    Assert.True(t.Status == TicketStatus.New);
                    Assert.Equal(1, t.TicketStates.Count());

                    t.Reject(reason: "test reject");

                    Assert.True(t.Status == TicketStatus.Rejected);
                    Assert.Equal(2, t.TicketStates.Count());

                    var s2 = t.CurrentState as RejectedTicketState;
                    Assert.Equal("test reject", s2.Reason);
                }
            }

            [Fact]
            public void CannotChangeFromNewToClosed()
            {
                using (var app = new App(testing: true))
                {
                    var t = new Ticket();
                    app.Tickets.Add(t);

                    Assert.Throws<InvalidOperationException>(() =>
                    {
                        t.Close();
                    });
                }
            }
        }
    }
}