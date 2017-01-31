using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rationally.Visio.Forms.WizardComponents
{
    public interface IWizardPanel
    {
        void UpdateModel();

        void InitData();

        bool IsValid();
    }
}
