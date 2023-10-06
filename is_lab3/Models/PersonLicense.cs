using System;
using System.Collections.Generic;

#nullable disable

namespace is_lab3.Models
{
    public partial class PersonLicense
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public uint Age { get; set; }
        public sbyte HasDrivingLicense { get; set; }
        public int IdCargoLicense { get; set; }
    }
}
