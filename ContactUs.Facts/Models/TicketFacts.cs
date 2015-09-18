using ContactUs.Models;
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

        // 1. NewTicketStatus_ShouldBeNew
        [Fact]
        public void NewTicketStatus_ShouldBeNew()
        {
            var t = new Ticket();

            Assert.Equal(TicketStatus.New, t.Status);
        }

        // 2. NewTicket_AbleToChangeToAcceptedAndRejected_ButNotClosed
        [Fact]
        public void NewTicket_AbleToChangeToAcceptedAndRejected_ButNotClosed()
        {
            var t = new Ticket();

            Assert.True(t.IsAcceptable);
            Assert.True(t.IsRejectable);
            Assert.False(t.IsCloseable);
        }


        // 3. AcceptedTicket_AbleToChangeToClosedOrRejected
        [Fact]
        public void AcceptedTicket_AbleToChangeToClosedOrRejected()
        {
            var t = new Ticket();
            t.Accept();

            Assert.False(t.IsAcceptable);
            Assert.True(t.IsRejectable);
            Assert.True(t.IsCloseable);
        }

        // 4. ClosedTicket_CannotChangeStatusAnymore
        [Fact]
        public void ClosedTicket_CannotChangeStatusAnymore()
        {
            var t = new Ticket();
            t.Accept();
            t.Close();

            Assert.False(t.IsAcceptable);
            Assert.False(t.IsRejectable);
            Assert.False(t.IsCloseable);
        }

        // 5. RejectedTicket_CannotChangeStatusAnymore
        public void RejectedTicket_CannotChangeStatusAnymore()
        {
            var t = new Ticket();
            t.Accept();
            t.Reject();

            Assert.False(t.IsAcceptable);
            Assert.False(t.IsRejectable);
            Assert.False(t.IsCloseable);
        }
    }
}