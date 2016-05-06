﻿namespace ExtendedVisioAddin1.Model
{
    public class Alternative
    {
        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public Alternative(string title, string status, string description)
        {
            Title = title;
            Status = status;
            Description = description;
        }
    }
}
