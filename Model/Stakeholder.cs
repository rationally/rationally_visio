using System.Reflection;
using log4net;

namespace Rationally.Visio.Model
{
    public class Stakeholder
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string Name { get; set; }
        public string Role { get; set; }
        public Stakeholder()
        {
            Name = "";
            Role = "";
        }

        public Stakeholder(string name, string role)
        {
            Name = name;
            Role = role;
        }
    }
}
