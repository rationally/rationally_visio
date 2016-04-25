using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.Model
{
    class RModel
    {
        private string author;
        private string decisionName;
        private string date;
        private string version;

        public string Author
        {
            get
            {
                return author;
            }

            set
            {
                author = value;
            }
        }

        public string DecisionName
        {
            get
            {
                return decisionName;
            }

            set
            {
                decisionName = value;
            }
        }

        public string Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        public string Version
        {
            get
            {
                return version;
            }

            set
            {
                version = value;
            }
        }

        public RModel()
        {

        }
    }
}
