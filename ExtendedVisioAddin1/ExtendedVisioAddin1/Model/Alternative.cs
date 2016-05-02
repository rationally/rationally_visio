using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.View;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    public class Alternative
    {
        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public Alternative(string title, string status, string description)
        {
            this.Title = title;
            this.Status = status;
            this.Description = description;
        }
    }
}
