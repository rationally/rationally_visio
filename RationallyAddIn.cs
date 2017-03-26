using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using Microsoft.Office.Interop.Visio;
using Microsoft.Office.Tools.Ribbon;
using Newtonsoft.Json.Linq;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.EventHandlers.DeleteEventHandlers;
using Rationally.Visio.EventHandlers.MarkerEventHandlers;
using Rationally.Visio.EventHandlers.QueryDeleteEventHandlers;
using Rationally.Visio.EventHandlers.TextChangedEventHandlers;
using Rationally.Visio.Forms;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Rationally.Visio.View.Planning;
using Rationally.Visio.View.Stakeholders;
using Application = Microsoft.Office.Interop.Visio.Application;

// ReSharper disable ClassNeverInstantiated.Global

//Main class for the visio add in. Everything is managed from here.
//Developed by Ruben Scheedler and Ronald Kruizinga for the University of Groningen

namespace Rationally.Visio
{
    public partial class RationallyAddIn
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public RationallyModel Model { get; set; }
        public RationallyView View { get; private set; }

        //Variables responsible for undo-scope handling
        public int StartedUndoState;
        public string LastDelete = "";

        //Variable to use for undo/redo handling
        private bool rebuildTree;

        private bool showRationallyUpdatePopup;
        public bool NewVersionAvailable;

        //Version numbers
        internal readonly Version AddInLocalVersion = new Version("0.1.5");
        private Version addInOnlineVersion;

        private void RationallyAddIn_Startup(object sender, EventArgs e)
        {
            //init for logger
            XmlConfigurator.Configure();
            Log.Info("Rationally started!");
            Model = new RationallyModel();
            View = new RationallyView(Application.ActivePage);
            rebuildTree = false;

            Application.MarkerEvent += Application_MarkerEvent;
            Application.TemplatePaths = Constants.MyShapesFolder;
            Application.DocumentCreated += DelegateCreateDocumentEvent;
            Application.DocumentOpened += Application_DocumentOpenendEvent;
            Application.ShapeAdded += Application_ShapeAddedEvent;
            Application.QueryCancelSelectionDelete += Application_QueryCancelSelectionDelete;
            Application.BeforeShapeDelete += Application_DeleteShapeEvent;
            Application.CellChanged += Application_CellChangedEvent;
            Application.TextChanged += Application_TextChangedEvent;
            Application.NoEventsPending += NoEventsPendingEventHandler;

            Application.BeforePageDelete += Application_BeforePageDeleteEvent;
            Application.WindowActivated += Application_WindowActivatedEvent;

            Application.MouseDown += Application_MouseDown;

            RegisterDeleteEventHandlers();
            RegisterQueryDeleteEventHandlers();
            RegisterMarkerEventHandlers();
            RegisterTextChangedEventHandlers();

            Log.Info("Eventhandlers registered succesfully");


            showRationallyUpdatePopup = NewVersionAvailable = CheckRationallyVersion();
        }

        private void Application_MouseDown(int Button, int KeyButtonState, double x, double y, ref bool CancelDefault)
        {
            if (Button != 1) //if other than the left mouse button was clicked
            {
                return;
            }
            
            PlanningContainer planningContainer = (View.Children.FirstOrDefault(c => c is PlanningContainer) as PlanningContainer);
            //locate all checkbox elements in the view
            List<CheckBoxStateComponent> candidates = planningContainer?.Children //map all planning items to their checkbox child, and that checkbox to its state component
                .Select(planningItemComponent => ((PlanningItemComponent)planningItemComponent).Children
                    .First(c => c is CheckBoxComponent)).Cast<CheckBoxComponent>()
                .Select(checkBox => (CheckBoxStateComponent)checkBox.Children.First()).ToList();

            if (candidates == null)
            {
                return;
            }
            CheckBoxStateComponent stateComponent = null;
            //for all the candidates, check if the clicked location was within its bounds. Stop as soon as a match if found.
            foreach (CheckBoxStateComponent candidate in candidates)
            {
                if (candidate.WasClicked(x, y))
                {
                    candidate.Toggle(); //actual changing of the clicked checkbox's state
                    stateComponent = candidate;
                    break;
                }
            }
            if (stateComponent == null)
            {
                return;
            }

            //locate parent of stateComponent
            PlanningItemComponent toStrikeThrough = planningContainer?.Children.Cast<PlanningItemComponent>().First(item => (item.Children.First(c => c is CheckBoxComponent) as CheckBoxComponent).Children.Contains(stateComponent));
            toStrikeThrough.Children.First(c => c is PlanningItemTextComponent).StrikeThrough = !toStrikeThrough.Children.First(c => c is PlanningItemTextComponent).StrikeThrough;

        }
        

        private static void RegisterDeleteEventHandlers()
        {
            DeleteEventHandlerRegistry.Register("alternative", new DeleteAlternativeEventHandler());
            DeleteEventHandlerRegistry.Register("alternatives", new DeleteAlternativesEventHandler());

            DeleteEventHandlerRegistry.Register("relatedUrlUrl", new DeletedRelatedUrlUrlEventHandler());
            DeleteEventHandlerRegistry.Register("relatedDocumentContainer", new DeleteRelatedDocumentEventHandler());
            DeleteEventHandlerRegistry.Register("relatedDocuments", new DeleteRelatedDocumentsEventHandler());

            DeleteEventHandlerRegistry.Register("forces", new DeleteForcesEventHandler());
            DeleteEventHandlerRegistry.Register("forceContainer", new DeleteForceEventHandler());

            DeleteEventHandlerRegistry.Register("decisionName", new DeleteTitleEventHandler());

            DeleteEventHandlerRegistry.Register("informationBox", new DeleteInformationBoxEventHandler());
            DeleteEventHandlerRegistry.Register("informationAuthor", new DeleteInformationComponentEventHandler());
            DeleteEventHandlerRegistry.Register("informationDate", new DeleteInformationComponentEventHandler());
            DeleteEventHandlerRegistry.Register("informationVersion", new DeleteInformationComponentEventHandler());
            DeleteEventHandlerRegistry.Register("informationAuthorLabel", new DeleteInformationComponentEventHandler());
            DeleteEventHandlerRegistry.Register("informationDateLabel", new DeleteInformationComponentEventHandler());
            DeleteEventHandlerRegistry.Register("informationVersionLabel", new DeleteInformationComponentEventHandler());

            DeleteEventHandlerRegistry.Register("stakeholder", new DeleteStakeholderEventHandler());
            DeleteEventHandlerRegistry.Register("stakeholders", new DeleteStakeholdersEventHandler());

            DeleteEventHandlerRegistry.Register("planningItem", new DeletePlanningItemEventHandler());
            DeleteEventHandlerRegistry.Register("planning", new DeletePlanningContainerEventHandler());
        }

        private static void RegisterQueryDeleteEventHandlers()
        {
            QueryDeleteEventHandlerRegistry.Register("forceConcern", new QDForceComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("forceDescription", new QDForceComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("forceValue", new QDForceComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("forceContainer", new QDForceContainerEventHandler());
            QueryDeleteEventHandlerRegistry.Register("forces", new QDForcesContainerEventHandler());

            QueryDeleteEventHandlerRegistry.Register("alternativeState", new QDAlternativeComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("alternativeIdentifier", new QDAlternativeComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("alternativeTitle", new QDAlternativeComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("alternativeDescription", new QDAlternativeComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("alternative", new QDAlternativeContainerEventHander());

            QueryDeleteEventHandlerRegistry.Register("relatedUrlUrl", new QDRelatedDocumentComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("relatedUrl", new QDRelatedDocumentComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("relatedFile", new QDRelatedDocumentComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("relatedDocumentTitle", new QDRelatedDocumentComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("relatedDocumentContainer", new QDRelatedDocumentContainerEventHandler());

            QueryDeleteEventHandlerRegistry.Register("stakeholderRole", new QDStakeholderComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("stakeholderName", new QDStakeholderComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("stakeholder", new QDStakeholderContainerEventHandler());

            QueryDeleteEventHandlerRegistry.Register("planningItemTextComponent", new QDPlanningItemComponentEventHandler());
            QueryDeleteEventHandlerRegistry.Register("planningItem", new QDPlanningContainerEventHandler());
        }

        private static void RegisterMarkerEventHandlers()
        {
            MarkerEventHandlerRegistry.Register("alternatives.add", new AddAlternativeEventHandler());

            MarkerEventHandlerRegistry.Register("relatedDocuments.addRelatedFile", new AddRelatedDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrlUrl.addRelatedFile", new AddRelatedDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrl.addRelatedFile", new AddRelatedDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.addRelatedFile", new AddRelatedDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedDocumentTitle.addRelatedFile", new AddRelatedDocumentHandler());

            MarkerEventHandlerRegistry.Register("relatedDocuments.addRelatedUrl", new AddRelatedUrlHandler());
            MarkerEventHandlerRegistry.Register("relatedUrlUrl.addRelatedUrl", new AddRelatedUrlHandler());
            MarkerEventHandlerRegistry.Register("relatedUrl.addRelatedUrl", new AddRelatedUrlHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.addRelatedUrl", new AddRelatedUrlHandler());
            MarkerEventHandlerRegistry.Register("relatedDocumentTitle.addRelatedUrl", new AddRelatedUrlHandler());

            MarkerEventHandlerRegistry.Register("relatedDocumentContainer.moveUp", new MoveUpDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrlUrl.moveUp", new MoveUpDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrl.moveUp", new MoveUpDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.moveUp", new MoveUpDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedDocumentTitle.moveUp", new MoveUpDocumentHandler());

            MarkerEventHandlerRegistry.Register("relatedDocumentContainer.moveDown", new MoveDownDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrlUrl.moveDown", new MoveDownDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedUrl.moveDown", new MoveDownDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.moveDown", new MoveDownDocumentHandler());
            MarkerEventHandlerRegistry.Register("relatedDocumentTitle.moveDown", new MoveDownDocumentHandler());

            MarkerEventHandlerRegistry.Register("relatedDocumentContainer.delete", new MarkerDeleteRelatedDocumentEventHandler());
            MarkerEventHandlerRegistry.Register("relatedUrlUrl.delete", new MarkerDeleteRelatedDocumentEventHandler());
            MarkerEventHandlerRegistry.Register("relatedUrl.delete", new MarkerDeleteRelatedDocumentEventHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.delete", new MarkerDeleteRelatedDocumentEventHandler());
            MarkerEventHandlerRegistry.Register("relatedDocumentTitle.delete", new MarkerDeleteRelatedDocumentEventHandler());

            MarkerEventHandlerRegistry.Register("alternative.add", new AddAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeState.add", new AddAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeIdentifier.add", new AddAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeTitle.add", new AddAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeDescription.add", new AddAlternativeEventHandler());

            MarkerEventHandlerRegistry.Register("alternative.delete", new MarkerDeleteAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeState.delete", new MarkerDeleteAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeIdentifier.delete", new MarkerDeleteAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeTitle.delete", new MarkerDeleteAlternativeEventHandler());
            MarkerEventHandlerRegistry.Register("alternativeDescription.delete", new MarkerDeleteAlternativeEventHandler());

            MarkerEventHandlerRegistry.Register("alternativeState.change", new EditAlternativeStateEventHandler());
            MarkerEventHandlerRegistry.Register("relatedFile.edit", new EditRelatedFileHandler());

            MarkerEventHandlerRegistry.Register("alternative.moveUp", new MoveUpAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeState.moveUp", new MoveUpAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeIdentifier.moveUp", new MoveUpAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeTitle.moveUp", new MoveUpAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeDescription.moveUp", new MoveUpAlternativeHandler());

            MarkerEventHandlerRegistry.Register("alternative.moveDown", new MoveDownAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeState.moveDown", new MoveDownAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeIdentifier.moveDown", new MoveDownAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeTitle.moveDown", new MoveDownAlternativeHandler());
            MarkerEventHandlerRegistry.Register("alternativeDescription.moveDown", new MoveDownAlternativeHandler());

            MarkerEventHandlerRegistry.Register("forces.add", new AddForceHandler());
            MarkerEventHandlerRegistry.Register("forceContainer.add", new AddForceHandler());
            MarkerEventHandlerRegistry.Register("forceConcern.add", new AddForceHandler());
            MarkerEventHandlerRegistry.Register("forceValue.add", new AddForceHandler());
            MarkerEventHandlerRegistry.Register("forceDescription.add", new AddForceHandler());

            MarkerEventHandlerRegistry.Register("forceContainer.delete", new StartDeleteForceEventHandler());
            MarkerEventHandlerRegistry.Register("forceConcern.delete", new StartDeleteForceEventHandler());
            MarkerEventHandlerRegistry.Register("forceValue.delete", new StartDeleteForceEventHandler());
            MarkerEventHandlerRegistry.Register("forceDescription.delete", new StartDeleteForceEventHandler());

            MarkerEventHandlerRegistry.Register("forceContainer.moveUp", new MoveUpForceHandler());
            MarkerEventHandlerRegistry.Register("forceConcern.moveUp", new MoveUpForceHandler());
            MarkerEventHandlerRegistry.Register("forceValue.moveUp", new MoveUpForceHandler());
            MarkerEventHandlerRegistry.Register("forceDescription.moveUp", new MoveUpForceHandler());

            MarkerEventHandlerRegistry.Register("forceContainer.moveDown", new MoveDownForceHandler());
            MarkerEventHandlerRegistry.Register("forceConcern.moveDown", new MoveDownForceHandler());
            MarkerEventHandlerRegistry.Register("forceValue.moveDown", new MoveDownForceHandler());
            MarkerEventHandlerRegistry.Register("forceDescription.moveDown", new MoveDownForceHandler());

            MarkerEventHandlerRegistry.Register("informationAuthor.openWizard", new OpenWizardEventHandler());
            MarkerEventHandlerRegistry.Register("informationDate.openWizard", new OpenWizardEventHandler());
            MarkerEventHandlerRegistry.Register("informationVersion.openWizard", new OpenWizardEventHandler());
            MarkerEventHandlerRegistry.Register("decisionName.openWizard", new OpenWizardEventHandler());

            MarkerEventHandlerRegistry.Register("stakeholders.add", new AddStakeholderEventHandler());
            MarkerEventHandlerRegistry.Register("stakeholder.add", new AddStakeholderEventHandler());
            MarkerEventHandlerRegistry.Register("stakeholderName.add", new AddStakeholderEventHandler());
            MarkerEventHandlerRegistry.Register("stakeholderRole.add", new AddStakeholderEventHandler());

            MarkerEventHandlerRegistry.Register("stakeholder.delete", new StartDeleteStakeholderEventHandler());
            MarkerEventHandlerRegistry.Register("stakeholderName.delete", new StartDeleteStakeholderEventHandler());
            MarkerEventHandlerRegistry.Register("stakeholderRole.delete", new StartDeleteStakeholderEventHandler());

            MarkerEventHandlerRegistry.Register("stakeholder.moveUp", new MoveUpStakeholderHandler());
            MarkerEventHandlerRegistry.Register("stakeholderName.moveUp", new MoveUpStakeholderHandler());
            MarkerEventHandlerRegistry.Register("stakeholderRole.moveUp", new MoveUpStakeholderHandler());
            MarkerEventHandlerRegistry.Register("stakeholderRole.moveDown", new MoveDownStakeholderHandler());
            MarkerEventHandlerRegistry.Register("stakeholder.moveDown", new MoveDownStakeholderHandler());
            MarkerEventHandlerRegistry.Register("stakeholderName.moveDown", new MoveDownStakeholderHandler());

            MarkerEventHandlerRegistry.Register("planning.add", new AddPlanningItemEventHandler());
            MarkerEventHandlerRegistry.Register("planningItemTextComponent.add", new AddPlanningItemEventHandler());

            MarkerEventHandlerRegistry.Register("planningItem.moveUp", new MoveUpPlanningItemHandler());
            MarkerEventHandlerRegistry.Register("planningItemTextComponent.moveUp", new MoveUpPlanningItemHandler());
            MarkerEventHandlerRegistry.Register("planningItemTextComponent.moveDown", new MoveDownPlanningItemHandler());
            MarkerEventHandlerRegistry.Register("planningItem.moveDown", new MoveDownPlanningItemHandler());


            MarkerEventHandlerRegistry.Register("planning.delete", new MarkerDeletePlanningItemEventHandler());
            MarkerEventHandlerRegistry.Register("planningItem.delete", new MarkerDeletePlanningItemEventHandler());
            MarkerEventHandlerRegistry.Register("planningItemTextComponent.delete", new MarkerDeletePlanningItemEventHandler());
        }

        private static void RegisterTextChangedEventHandlers()
        {
            TextChangedEventHandlerRegistry.Register("forceValue", new ForceValueTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("forceConcern", new ForceConcernTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("forceDescription", new ForceDescriptionTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("alternativeState", new AlternativeStateTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("alternativeTitle", new AlternativeTitleTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("informationAuthor", new InformationAuthorTextChangedHandler());
            TextChangedEventHandlerRegistry.Register("informationDate", new InformationDateTextChangedHandler());
            TextChangedEventHandlerRegistry.Register("informationVersion", new InformationVersionTextChangedHandler());
            TextChangedEventHandlerRegistry.Register("decisionName", new DecisionNameTextChangedHandler());
            TextChangedEventHandlerRegistry.Register("relatedDocumentTitle", new RelatedDocumentTitleTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("relatedUrlUrl", new RelatedUrlUrlTextChangedHandler());
            TextChangedEventHandlerRegistry.Register("stakeholderName", new StakeholderNameTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("stakeholderRole", new StakeholderRoleTextChangedEventHandler());
            TextChangedEventHandlerRegistry.Register("planningItemTextComponent", new PlanningTextChangedEventHandler());
        }

        //Fired when any text is changed
        private void Application_TextChangedEvent(Shape shape)
        {
            if (shape.Document.Template.Contains(Constants.TemplateName) && (shape.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists))
            {
                try
                {
                    Log.Debug("TextChanged: shapeName: " + shape.Name);
                    string rationallyType = shape.CellsU[CellConstants.RationallyType].ResultStr["Value"];
                    TextChangedEventHandlerRegistry.HandleEvent(rationallyType, View, shape);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        //Fired when the user clicks on the main window from a different window.
        private void Application_WindowActivatedEvent(Window w)
        {
            if ((w.Type == (short)VisWinTypes.visDrawing) && w.Document.Template.Contains(Constants.TemplateName)) //VisDrawing is the main sheet
            {
                try
                {
                    Log.Debug("Window activated event handler enter");
                    View.Page = Application.ActivePage;
                    RebuildTree(w.Document);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void NoEventsPendingEventHandler(Application app) //Executed after all other events. Ensures we are never insides an undo scope
        {
            if ((app?.ActiveDocument?.Template.Contains(Constants.TemplateName) ?? false) && !app.IsUndoingOrRedoing && rebuildTree)
            {
                try
                {
                    Log.Debug("No events pending event handler entered. Rebuilding tree...");
                    RebuildTree(app.ActiveDocument);
                    rebuildTree = false;
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void Application_MarkerEvent(Application application, int sequence, string context)
        {
            if (application.ActiveDocument.Template.Contains(Constants.TemplateName))
            {
                try
                {
                    Selection selection = Application.ActiveWindow.Selection; //event must originate from selected element

                    foreach (Shape s in selection)
                    {
                        if (s.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                        {
                            string identifier = context;
                            if (context.Contains("."))
                            {
                                identifier = context.Split('.')[1];
                                context = context.Split('.')[0];
                            }
                            Log.Debug("Marker event being handled for: " + s.Name);
                            MarkerEventHandlerRegistry.HandleEvent(s.CellsU[CellConstants.RationallyType].ResultStr["Value"] + "." + context, s, identifier);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void Application_CellChangedEvent(Cell cell)
        {
            Shape changedShape = cell.Shape;
            // ReSharper disable once MergeSequentialChecksWhenPossible
            if ((changedShape == null) || !changedShape.Document.Template.Contains(Constants.TemplateName) || (changedShape.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] != Constants.CellExists)) //No need to continue when the shape is not part of our model.
            {
                return;
            }
            try
            {
                if (RelatedUrlComponent.IsRelatedUrlComponent(changedShape.Name) && cell.LocalName.Equals("Hyperlink.Row_1.Address")) //Link has updated
                {
                    Log.Debug("Cell changed of hyperlink shape:" + changedShape.Name);
                    //find the container that holds all Related Documents
                    RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)View.Children.First(c => c is RelatedDocumentsContainer);
                    //find the related document holding the changed shape (one of his children has RShape equal to changedShape)
                    RelatedDocumentContainer relatedDocumentContainer = relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(dc => dc.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0);
                    //update the text of the URL display component to the new url
                    RelatedURLURLComponent relatedURLURLComponent = (RelatedURLURLComponent)relatedDocumentContainer.Children.First(c => c is RelatedURLURLComponent);
                    relatedURLURLComponent.Text = changedShape.Hyperlink.Address;
                }
                else if (Application.IsUndoingOrRedoing && ForceContainer.IsForceContainer(changedShape.Name) && cell.LocalName.Equals(CellConstants.Index))
                {
                    Log.Debug("Forceindex cell changed of forcecontainer. shape:" + changedShape.Name);
                    RationallyComponent forcesComponent = View.Children.FirstOrDefault(x => x is ForcesContainer);
                    if (forcesComponent != null)
                    {
                        rebuildTree = true; //Wait with the rebuild till the undo is done
                    }
                }
                else if (Application.IsUndoingOrRedoing && AlternativeContainer.IsAlternativeContainer(changedShape.Name) && cell.LocalName.Equals(CellConstants.Index))
                {
                    Log.Debug("Alternative index cell changed of alternativecontainer. shape:" + changedShape.Name);
                    RationallyComponent alternativesComponent = View.Children.FirstOrDefault(x => x is AlternativesContainer);
                    if (alternativesComponent != null)
                    {
                        rebuildTree = true; //Wait with the rebuild till the undo is done
                    }
                }
                else if (Application.IsUndoingOrRedoing && RelatedDocumentContainer.IsRelatedDocumentContainer(changedShape.Name) && cell.LocalName.Equals("User.index"))
                {
                    Log.Debug("Document index cell changed of documentcontainer. shape:" + changedShape.Name);
                    RationallyComponent docComponent = View.Children.FirstOrDefault(x => x is RelatedDocumentsContainer);
                    if (docComponent != null)
                    {
                        rebuildTree = true; //Wait with the rebuild till the undo is done
                    }
                }
                else if (Application.IsUndoingOrRedoing && StakeholderContainer.IsStakeholderContainer(changedShape.Name) && cell.LocalName.Equals(CellConstants.Index))
                {
                    Log.Debug("Stakeholder index cell changed of stakeholdercontainer. shape:" + changedShape.Name);
                    RationallyComponent stakeholderComponent = View.Children.FirstOrDefault(x => x is StakeholdersContainer);
                    if (stakeholderComponent != null)
                    {
                        rebuildTree = true; //Wait with the rebuild till the undo is done
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex);
#if DEBUG
                throw;
#endif
            }
        }

        public void RebuildTree(IVDocument d) //Completely rebuild the model
        {
            Log.Debug("entered rebuild tree");
            try
            {
                Log.Debug("State before reset: ViewChildren: " + View.Children.Count + ", Model.Aternatives:" + Model.Alternatives.Count + ", Model.Documents:" + Model.Documents.Count + ", Model.Forces:" + Model.Forces.Count + ", Model.Stakeholders:" + Model.Stakeholders.Count);
                View.Children.Clear();
                Model.Alternatives.Clear();
                Model.Documents.Clear();
                Model.Forces.Clear();
                Model.Stakeholders.Clear();
                Model.PlanningItems.Clear();

                foreach (Page page in d.Pages)
                {
                    foreach (Shape shape in page.Shapes)
                    {
                        View.AddToTree(shape, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex);
#if DEBUG
                throw;
#endif
            }
        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            Log.Debug("Shape added with name: " + s.Name);
            if (s.Document.Template.Contains(Constants.TemplateName) && (s.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists) && !View.ExistsInTree(s))
            {
                try
                {
                    switch (s.CellsU[CellConstants.RationallyType].ResultStr["Value"])
                    {
                        case "alternativeAddStub":
                            if (!Application.IsUndoingOrRedoing)
                            {
                                int scopeId = Application.BeginUndoScope("Add alternative");
                                s.Delete();
                                AlternativesContainer alternativesContainer = Globals.RationallyAddIn.View.Children.FirstOrDefault(ch => ch is AlternativesContainer) as AlternativesContainer;
                                alternativesContainer?.AddAlternative("Title", Model.AlternativeStateColors.Keys.FirstOrDefault());

                                Application.EndUndoScope(scopeId, true);
                            }
                            break;
                        case "forceAddStub":
                            if (!Application.IsUndoingOrRedoing)
                            {
                                int scopeId = Application.BeginUndoScope("Add force");
                                s.Delete();
                                MarkerEventHandlerRegistry.HandleEvent("forces.add", null, null);
                                Application.EndUndoScope(scopeId, true);
                            }
                            break;
                        case "relatedDocumentAddStub":
                            if (!Application.IsUndoingOrRedoing)
                            {
                                int scopeId = Application.BeginUndoScope("Add related file");
                                s.Delete();
                                MarkerEventHandlerRegistry.HandleEvent("relatedDocuments.addRelatedFile", null, null);
                                Application.EndUndoScope(scopeId, true);
                            }
                            break;
                        case "relatedUrlAddStub":
                            if (!Application.IsUndoingOrRedoing)
                            {
                                int scopeId = Application.BeginUndoScope("Add related url");
                                s.Delete();
                                MarkerEventHandlerRegistry.HandleEvent("relatedDocuments.addRelatedUrl", null, null);
                                Application.EndUndoScope(scopeId, true);
                            }
                            break;
                        case "stakeholderAddStub":
                            if (!Application.IsUndoingOrRedoing)
                            {
                                int scopeId = Application.BeginUndoScope("Add stakeholder");
                                s.Delete();
                                StakeholdersContainer stakeholdersContainer = Globals.RationallyAddIn.View.Children.FirstOrDefault(ch => ch is StakeholdersContainer) as StakeholdersContainer;
                                stakeholdersContainer?.AddStakeholder("<<name>>", "<<role>>");

                                Application.EndUndoScope(scopeId, true);
                            }
                            break;
                        default:
                            View.AddToTree(s, true);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private bool Application_QueryCancelSelectionDelete(Selection e) //Fired before a shape is deleted. Shape still exists here
        {
            List<Shape> toBeDeleted = e.Cast<Shape>().ToList();
            if (!e.Document.Template.Contains(Constants.TemplateName))
            {
                return false;
            }
            try
            {
                Log.Debug("before shape deleted event for " + e.Count + " shapes.");
                if (toBeDeleted.Any(s => ((s.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                                          && (s.CellsU[CellConstants.RationallyType].ResultStr["Value"] == "forceHeaderRow")) || (s.CellsU[CellConstants.RationallyType].ResultStr["Value"] == "forceTotalsRow")))
                {
                    if (toBeDeleted.All(s => s.CellsU[CellConstants.RationallyType].ResultStr["Value"] != "forces"))
                    {
                        MessageBox.Show("Deleting the header or totals row is not allowed.", "Delete forbidden", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return true;
                    }
                }

                //store the rationally type of the last shape, which is responsible for ending the undo scope
                if (string.IsNullOrEmpty(LastDelete) && (StartedUndoState == 0))
                {
                    LastDelete = toBeDeleted.Last().Name;
                    Globals.RationallyAddIn.StartedUndoState = Globals.RationallyAddIn.Application.BeginUndoScope("Delete shape");
                }

                //all shapes in the selection are already bound to be deleted. Mark them, so other pieces of code don't also try to delete them, if they are in the tree.
                toBeDeleted.Where(s => View.ExistsInTree(s)).ToList().ForEach(tbd => View.GetComponentByShape(tbd).Deleted = true);
                foreach (Shape s in e)
                {
                    Log.Debug("deleted shape name: " + s.Name);
                    if (s.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        string rationallyType = s.CellsU[CellConstants.RationallyType].ResultStr["Value"];

                        QueryDeleteEventHandlerRegistry.HandleEvent(rationallyType, View, s);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex);
#if DEBUG
                throw;
#endif
            }
            return false;
        }
        private void Application_BeforePageDeleteEvent(Page p)
        {
            if (p.Document.Template.Contains(Constants.TemplateName))
            {
                try
                {
                    Log.Debug("page delete event handler entered");
                    foreach (Shape shape in p.Shapes)
                    {
                        View.DeleteFromTree(shape);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void Application_DeleteShapeEvent(Shape s) //Fired when a shape is deleted. Shape now no longer exists
        {

            if (s.Document.Template.Contains(Constants.TemplateName))
            {
                try
                {
                    Log.Debug("shape deleted event for: " + s.Name);
                    if (s.CellExistsU[CellConstants.Stub, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        return;
                    }
                    if (s.CellExistsU[CellConstants.RationallyType, (short)VisExistsFlags.visExistsAnywhere] == Constants.CellExists)
                    {
                        string rationallyType = s.CellsU[CellConstants.RationallyType].ResultStr["Value"];

                        //mark the deleted shape as 'deleted' in the view tree
                        RationallyComponent deleted = View.GetComponentByShape(s);
                        if (deleted != null)
                        {
                            deleted.Deleted = true;
                        }
                        DeleteEventHandlerRegistry.HandleEvent(rationallyType, Model, s);
                    }
                    else
                    {
                        if (StartedUndoState == 0)
                        {
                            RebuildTree(s.ContainingPage.Document);
                        }
                    }
                    if ((StartedUndoState != 0) && (s.Name == LastDelete))
                    {
                        Log.Debug("ending undo scope");
                        Application.EndUndoScope(StartedUndoState, true);
                        StartedUndoState = 0;
                        LastDelete = "";
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        /// <summary>
        /// Method that performs a get request to the Github api in order to check the version of the latest release.
        /// </summary>
        /// <returns>A boolean representing whether there is an update available online</returns>
        private bool CheckRationallyVersion()
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("User-Agent", "Rationally-Addin");
                try
                {
                    string result = webClient.DownloadString("https://api.github.com/repos/rationally/rationally_visio/releases/latest");
                    JObject json = JObject.Parse(result);
                    addInOnlineVersion = new Version(json["tag_name"].ToString());
                    return addInOnlineVersion > AddInLocalVersion;
                }
                catch (WebException)
                {
                    Log.Warn("Latest version could not be retrieved.");
                    return false;
                }
            }
        }

        //Designer method. Called when application is started.
        private void InternalStartup() => Startup += RationallyAddIn_Startup;

        private static void DelegateCreateDocumentEvent(IVDocument d)
        {

            if (d.Template.Contains(Constants.TemplateName))
            {
                try
                {
                    Log.Debug("Rationally template detected => firing document created handler.");
                    DocumentCreatedEventHandler.Execute(d);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }

        private void Application_DocumentOpenendEvent(IVDocument d)
        {
            Log.Debug("DocumentOpenedEvent detected.");
            if (Application.ActiveDocument.Template.Contains(Constants.TemplateName) && showRationallyUpdatePopup)
            {
                Log.Debug("Rationally template and update required detected.");
                try
                {
                    UpdateAvailable upd = new UpdateAvailable(AddInLocalVersion, addInOnlineVersion);
                    upd.Show();
                    showRationallyUpdatePopup = false;
                    Log.Debug("Shown update popup successfully.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, ex);
#if DEBUG
                    throw;
#endif
                }
            }
        }
    }
}
