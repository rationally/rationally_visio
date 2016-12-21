using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Rationally.Visio.Model
{
    public class Stakeholder
    {
        public string Name { get; set; }
        public Stakeholder()
        {
            Name = "";
        }

        public Stakeholder(string name)
        {
            Name = name;
        }
    }
}
