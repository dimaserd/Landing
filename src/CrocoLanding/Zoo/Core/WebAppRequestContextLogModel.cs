using System;

namespace Zoo.Core
{
    public class WebAppRequestContextLogModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string RequestId { get; set; }

        public string ParentRequestId { get; set; }

        public string Uri { get; set; }

        public DateTime StartedOn { get; set; }

        public DateTime FinishedOn { get; set; }
    }
}