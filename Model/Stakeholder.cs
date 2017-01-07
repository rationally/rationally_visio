using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using log4net;

namespace Rationally.Visio.Model
{
    public class Stakeholder
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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
