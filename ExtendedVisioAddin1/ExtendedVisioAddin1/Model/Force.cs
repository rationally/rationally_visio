using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.View.Forces;

namespace ExtendedVisioAddin1.Model
{
    public class Force
    {
        public string Concern { get; set; }
        public string Description { get; set; }

        public Force(string concern, string description) //TODO force values?
        {
            Concern = concern;
            Description = description;
        }
    }
}
