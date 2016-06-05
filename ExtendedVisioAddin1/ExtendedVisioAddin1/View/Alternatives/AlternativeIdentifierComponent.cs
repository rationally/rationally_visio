﻿using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeIdentifierComponent : TextLabel, IAlternativeComponent
    {
        private static readonly Regex IdentRegex = new Regex(@"AlternativeIdent(\.\d+)?$");
        public AlternativeIdentifierComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            InitStyle();
        }

        public AlternativeIdentifierComponent(Page page, int alternativeIndex, string text) : base(page, text)
        {
            AddUserRow("rationallyType");
            RationallyType = "alternativeIdentifier";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            Name = "AlternativeIdent";
            //Locks
            /*LockDelete = true;
            LockRotate = true;
            LockMoveX = true;
            LockMoveY = true;
            LockHeight = true;
            LockTextEdit = true;
            LockWidth = true;*/
            InitStyle();
        }

        private void InitStyle()
        {
            SetMargin(0.1);
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded; 
            Height = 0.3667;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
            Text = (char)(65 + alternativeIndex) + ":";
        }
        public static bool IsIdentifierDescription(string name)
        {
            return IdentRegex.IsMatch(name);
        }
    }
}
