﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.EventHandlers.DeleteEventHandlers
{
    abstract class DeleteEventHandler
    {
        public abstract void Execute(string eventKey, RModel model, Shape changedShape);
    }
}