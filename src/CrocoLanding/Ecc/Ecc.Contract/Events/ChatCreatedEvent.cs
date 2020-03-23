using System.Collections.Generic;

namespace Ecc.Contract.Events
{
    public class ChatCreatedEvent
    {
        public int ChatId { get; set; }

        public List<string> UserIds { get; set; }
    }
}