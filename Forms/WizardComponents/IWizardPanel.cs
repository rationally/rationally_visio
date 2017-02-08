namespace Rationally.Visio.Forms.WizardComponents
{
    public interface IWizardPanel
    {
        void UpdateModel();

        void InitData();

        bool IsValid();
    }
}
