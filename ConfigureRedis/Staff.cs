using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigureRedis
{
    public class Staff
    {
        public Guid StaffId { get; set; }

        public string FirstName { get; set; }

        public bool Gender { get; set; }

        public DateTime Birthday { get; set; }

        public int Age { get; set; }
    }
}
