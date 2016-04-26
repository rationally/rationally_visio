using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    internal class Alternative
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

        public void AddTo(IVShape alternatives, int alternativeIdentifier) 
        {
            
            Application application = Globals.ThisAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);

            Master alternativeMaster = containerDocument.Masters["Plain"];//wrapper for one whole alternative

            //--- define sub parts of the alternative. Only a Selection can be used to fill a container, so empty the current selection of the window and fill it with sub parts
            application.ActiveWindow.DeselectAll();

            //1) identifier ("A:")
            string identifier = (char) (65 + alternativeIdentifier) + "";
            Master identifierRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape identifierRectangle = application.ActivePage.Drop(identifierRectangleMaster, 0, 0); //TODO check if can drop on a shape directly
            RationallyComponent identifierComponent = new RationallyComponent(identifierRectangle);
            identifierComponent.ToggleBoldFont(true);
            identifierComponent.Text = identifier + ":";
            identifierComponent.Width = 0.5;
            identifierComponent.Height = 0.4;
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            identifierComponent.RationallyType = "alternativeIdentifier";
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            identifierComponent.AlternativeIndex = alternativeIdentifier;

            application.ActiveWindow.Select(identifierRectangle, (short)VisSelectArgs.visSelect);

            //locks
            identifierComponent.LockWidth = true;//TODO other locks




            //2) title
            Master titleRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape titleRectangle = application.ActivePage.Drop(titleRectangleMaster, 0, 0);
            RationallyComponent titleComponent = new RationallyComponent(titleRectangle);
            titleComponent.Width = 4;
            titleComponent.Height = 0.5;
            titleComponent.Text = this.Title;
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            titleComponent.RationallyType = "alternativeTitle";
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            titleComponent.AlternativeIndex = alternativeIdentifier;
            application.ActiveWindow.Select(titleRectangle, (short)VisSelectArgs.visSelect);

            //3) state box
            Master stateRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape stateRectangle = application.ActivePage.Drop(stateRectangleMaster, 0, 0);
            RationallyComponent stateComponent = new RationallyComponent(stateRectangle);
            stateComponent.Width = 2;
            stateComponent.Height = 0.5;
            stateComponent.Text = this.Status;
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            stateComponent.RationallyType = "alternativeState";
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            stateComponent.AlternativeIndex = alternativeIdentifier;
            application.ActiveWindow.Select(stateRectangle, (short)VisSelectArgs.visSelect);

            //4) description area
            Master descRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape descRectangle = application.ActivePage.Drop(descRectangleMaster, 0, 0);
            RationallyComponent descComponent = new RationallyComponent(descRectangle);
            descComponent.Width = 5;
            descComponent.Height = 5;
            descComponent.Text = this.Description;
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            descComponent.RationallyType = "alternativeDescription";
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            descComponent.AlternativeIndex = alternativeIdentifier;
            application.ActiveWindow.Select(descRectangle, (short)VisSelectArgs.visSelect);

            RationallyComponent altComponent = new RationallyComponent(alternatives);
            IVShape droppedAlternative = application.ActivePage.DropContainer(alternativeMaster, null);//altComponent.CenterX, altComponent.CenterY
            droppedAlternative.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpand;
            RationallyComponent alternative = new RationallyComponent(droppedAlternative);
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            alternative.RationallyType = "alternative";
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            alternative.AlternativeIndex = alternativeIdentifier;

            alternative.Height = 0;
            alternative.Width = 0;
            droppedAlternative.ContainerProperties.AddMember(identifierRectangle, VisMemberAddOptions.visMemberAddUseResizeSetting);
            droppedAlternative.ContainerProperties.AddMember(titleRectangle, VisMemberAddOptions.visMemberAddUseResizeSetting);
            droppedAlternative.ContainerProperties.AddMember(stateRectangle, VisMemberAddOptions.visMemberAddUseResizeSetting);
            droppedAlternative.ContainerProperties.AddMember(descRectangle, VisMemberAddOptions.visMemberAddUseResizeSetting);
            droppedAlternative.AddToContainers();//TODO position alternative above alternatives

            containerDocument.Close();
        }
    }
}
