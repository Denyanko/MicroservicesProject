using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model
{
    public class StudentCreated
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
    }

    public class StudentUpdated
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
    }

    public class StudentDeleted
    {
        public int StudentId { get; set; }
    }
}
