using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    internal class CheckBoxComponent : RationallyContainer
    {
        public static readonly double CheckboxSize = 0.4;
        private static readonly Regex Regex = new Regex(@"CheckBoxComponent(\.\d+)?$");

        public CheckBoxComponent(Page page, int index, bool isFinished) : base(page)
        {
            //create a rectangle wrapper shape
            Application application = Globals.RationallyAddIn.Application;
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short)VisOpenSaveArgs.visOpenHidden);
            Master containerMaster = containerDocument.Masters["Plain"];

            Shape = Page.DropContainer(containerMaster, null);

            Shape.CellsU["User.msvSDHeadingStyle"].ResultIU = 0; //Remove visible header
            containerDocument.Close();

            //create a slightly smaller rectangle shape, whose background indicates the state of the checkbox
            Children.Add(new CheckBoxStateComponent(page, index, isFinished));

            AddUserRow("rationallyType");
            AddUserRow("Index");
            RationallyType = "checkBoxComponent";
            Name = "CheckBoxComponent";
            Index = index;
            Width = CheckboxSize;
            Height = CheckboxSize;
            InitStyle();
        }

        public CheckBoxComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;

            foreach (int shapeIdentifier in shape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested))
            {
                Shape checkBoxComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (CheckBoxStateComponent.IsCheckBoxStateComponent(checkBoxComponent.Name))
                {
                    CheckBoxStateComponent cbComponent = new CheckBoxStateComponent(page, checkBoxComponent);
                    Children.Add(cbComponent);
                }
            }

            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            VisioShape shapeComponent = new VisioShape(Page) {Shape = s};

            if (CheckBoxStateComponent.IsCheckBoxStateComponent(shapeComponent.Name))
            {
                Children.Add(new CheckBoxStateComponent(Page, s));
            }//TODO validate whether it's the right one
        }

        private void InitStyle()
        {

        }

        public void Check(bool isChecked) => ((CheckBoxStateComponent) Children.First()).Checked = isChecked;

        public bool Checked => ((CheckBoxStateComponent)Children.First()).Checked;
        public static bool IsCheckBoxComponent(string name) => Regex.IsMatch(name);
    }
}
