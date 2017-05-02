using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json;
using Rationally.Visio.Enums;
using Rationally.Visio.Forms;
using Rationally.Visio.RationallyConstants;
using RestSharp;

namespace Rationally.Visio
{
    public partial class RationallyRibbon
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void RationallyRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            wizardButton.Click += wizardButton_Click;
            alternativeStatesOptionsButton.Visible = false;
            saveToServerButton.Visible = false;
        }

        private static void wizardButton_Click(object sender, RibbonControlEventArgs e)
        {
            if (Globals.RationallyAddIn.Application.ActiveDocument.Template.Contains(Information.TemplateName))
            {
                ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Title);

            }
            else
            {
                MessageBox.Show(Messages.Warning_WizardOnlyOnRationallyTemplates_Description,
                            Messages.Warning_WizardOnlyOnRationallyTemplates_Label,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign,
                            false);
            }
        }

        private void settingsButton_Click(object sender, RibbonControlEventArgs e) => ProjectSetupWizard.Instance.ShowDialog(false, WizardFieldTypes.Title);

        private void alternativeStatesOptionsButton_Click(object sender, RibbonControlEventArgs e) => AlternativeStatesConfigurator.Instance.ShowDialog();

        private void SaveToServerButton_click(object sender, RibbonControlEventArgs e)
        {
            RestClient client = new RestClient("http://82.73.233.237:4567/");//localhost
            RestRequest request = new RestRequest("/decision/", Method.POST);
            string jsonToSend = JsonConvert.SerializeObject(Globals.RationallyAddIn.Model);
            request.AddParameter("application/json; charset=utf-8", jsonToSend, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}
