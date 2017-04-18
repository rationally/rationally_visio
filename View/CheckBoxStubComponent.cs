using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    internal class CheckBoxStubComponent : VisioShape
    {
        public override int Index
        {
            get; set;
        }

        public CheckBoxStubComponent(Page page, int index) : base(page)
        {
            Index = index;
        }

        public override bool ExistsInTree(Shape s) => false;
    }
}
