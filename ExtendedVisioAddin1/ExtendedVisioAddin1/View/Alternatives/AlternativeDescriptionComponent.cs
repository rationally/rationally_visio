﻿using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeDescriptionComponent : HeaderlessContainer, IAlternativeComponent
    {
        private static readonly Regex DescriptionRegex = new Regex(@"AlternativeDescription(\.\d+)?$");
        public AlternativeDescriptionComponent(Page page, Shape alternativeComponent) : base(page, false)
        {
            RShape = alternativeComponent;
            InitStyle();
        }

        public AlternativeDescriptionComponent(Page page, int alternativeIndex, string description) : base(page)
        {
            Width = 5.15;
            Height = 2.5;
            InitStyle();

            AddUserRow("rationallyType");
            RationallyType = "alternativeDescription";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeDescription";

            Text = description;

            //Locks
            /*LockDelete = true;
            LockRotate = true;
            LockMoveX = true;
            LockMoveY = true; */
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
        }

        public static bool IsAlternativeDescription(string name)
        {
            return DescriptionRegex.IsMatch(name);
        }

        public void InitStyle()
        {
            
            MarginLeft = 0.1;
            MarginRight = 0.1;
            MarginBottom = 0.1;
            MarginTop = 0.05;
        }
    }
}
