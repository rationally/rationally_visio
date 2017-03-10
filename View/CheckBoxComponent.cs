using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    class CheckBoxComponent : RationallyContainer
    {
        public static double CHECKBOX_SIZE = 0.4;
        private static readonly Regex regex = new Regex(@"CheckBoxComponent(\.\d+)?$");

        public CheckBoxComponent(Page page) : base(page)
        {
            //create a rectangle wrapper shape
            Application application = Globals.RationallyAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short)VisOpenSaveArgs.visOpenHidden);
            Master containerMaster = containerDocument.Masters["Plain"];

            RShape = Page.DropContainer(containerMaster, null);

            RShape.CellsU["User.msvSDHeadingStyle"].ResultIU = 0; //Remove visible header
            containerDocument.Close();

            //create a slightly smaller rectangle shape, whose background indicates the state of the checkbox
            Children.Add(new CheckBoxStateComponent(page));

            AddUserRow("rationallyType");
            AddUserRow("Index");
            RationallyType = "checkBoxComponent";
            Name = "CheckBoxComponent";
            Index = -1;//TODO implement via model
            Width = CHECKBOX_SIZE;
            Height = CHECKBOX_SIZE;

            InitStyle();
        }

        public CheckBoxComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) {RShape = s};

            if (CheckBoxStateComponent.IsCheckBoxStateComponent(shapeComponent.Name))
            {
                Children.Add((CheckBoxStateComponent)shapeComponent);
            }//TODO validate whether it's the right one
        }

        private void InitStyle()
        {

        }

        public void Check(bool isChecked)
        {
            ((CheckBoxStateComponent) Children.First()).Checked = isChecked;
        }

        public static bool IsCheckBoxComponent(string name)
        {
            return regex.IsMatch(name);
        }
    }
}
