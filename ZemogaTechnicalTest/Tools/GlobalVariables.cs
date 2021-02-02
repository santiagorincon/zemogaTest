using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Tools
{
    public class GlobalVariables
    {
        public int InitialStatus { get; set; }
        public int PendingStatus { get; set; }
        public int PublishedStatus { get; set; }
        public int RejectedStatus { get; set; }
        public int DeletedStatus { get; set; }
        public int WriterRole { get; set; }
        public int EditorRole { get; set; }
    }
}
