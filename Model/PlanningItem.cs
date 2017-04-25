using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class PlanningItem
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestId = -1;
        public string ItemText { get; set; }
        public bool Finished { get; set; }
        public int Id { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of the item

        [JsonConstructor]
        private PlanningItem()
        {
        }
        public PlanningItem(string text, bool finished)
        {
            ItemText = text;
            Finished = finished;
            Id = ++highestId;
        }

        public PlanningItem(string text, bool finished, int id)
        {
            ItemText = text;
            Finished = finished;
            Id = id;
            if (id > highestId)
            {
                highestId = id;
            }
        }
    }
}
