﻿using System;
using System.Collections.Generic;
using System.Linq;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.EventHandlers.DeleteEventHandlers;
using Rationally.Visio.EventHandlers.MarkerEventHandlers;
using Rationally.Visio.EventHandlers.QueryDeleteEventHandlers;
using Rationally.Visio.EventHandlers.TextChangedEventHandlers;
using Rationally.Visio.Model;
using Rationally.Visio.View;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Application = Microsoft.Office.Interop.Visio.Application;
using Shape = Microsoft.Office.Interop.Visio.Shape;
using log4net;

//Main class for the visio add in. Everything is managed from here.
//Developed by Ruben Scheedler and Ronald Kruizinga for the University of Groningen

namespace Rationally.Visio
{
    public partial class ThisAddIn
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RModel Model { get; set; }
        public RView View { get; set; }

        //Variables responsible for undo-scope handling
        public int StartedUndoState;
        public string LastDelete = "";

        //Variable to use for undo/redo handling
        private bool _rebuildTree;

        public readonly string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";

        public const string TemplateName = "Rationally Template";

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            //init for logger
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Rationally started!");
            Model = new RModel();
            View = new RView(Application.ActivePage);
            _rebuildTree = false;
            Application.MarkerEvent += Application_MarkerEvent;
            Application.TemplatePaths = FolderPath;
            Application.DocumentCreated += DelegateCreateDocumentEvent;
            Application.ShapeAdded += Application_ShapeAddedEvent;
            Application.QueryCancelSelectionDelete += Application_QueryCancelSelectionDelete;
            Application.BeforeShapeDelete += Application_DeleteShapeEvent;
            Application.CellChanged += Application_CellChangedEvent;
            Application.TextChanged += Application_TextChangedEvent;
           Application.NoEventsPending += NoEventsPendingEventHandler;

            Application.BeforePageDelete += Application_BeforePageDeleteEvent;
            Application.WindowActivated += Application_WindowActivatedEvent;

            

            RegisterDeleteEventHandlers();
            RegisterQueryDeleteEventHandlers();
            RegisterMarkerEventHandlers();
            RegisterTextChangedEventHandlers();

            Log.Info("Eventhandlers registered succesfully");
        }
        
        private static void RegisterDeleteEventHandlers()
        {
            DeleteEventHandlerRegistry registry = DeleteEventHandlerRegistry.Instance;
            registry.Register("forceContainer",new DeleteForceEventHandler());
            registry.Register("relatedDocumentContainer", new DeleteRelatedDocumentEventHandler());
            registry.Register("alternative", new DeleteAlternativeEventHandler());
            registry.Register("relatedUrlUrl", new DeletedRelatedUrlUrlEventHandler());
            registry.Register("informationBox", new DeleteInformationBoxEventHandler());
            registry.Register("relatedDocuments", new DeleteRelatedDocumentsEventHandler());
            registry.Register("forces", new DeleteForcesEventHandler());
            registry.Register("alternatives", new DeleteAlternativesEventHandler());
        }

        private static void RegisterQueryDeleteEventHandlers()
        {
            QueryDeleteEventHandlerRegistry registry = QueryDeleteEventHandlerRegistry.Instance;

            registry.Register("forceConcern",new QDForceComponentEventHandler());
            registry.Register("forceDescription", new QDForceComponentEventHandler());
            registry.Register("forceValue", new QDForceComponentEventHandler());
            registry.Register("forceContainer", new QDForceContainerEventHandler());
            registry.Register("forces", new QDForcesContainerEventHandler());

            registry.Register("alternativeState", new QDAlternativeComponentEventHandler());
            registry.Register("alternativeIdentifier", new QDAlternativeComponentEventHandler());
            registry.Register("alternativeTitle", new QDAlternativeComponentEventHandler());
            registry.Register("alternativeDescription", new QDAlternativeComponentEventHandler());
            registry.Register("alternative", new QDAlternativeContainerEventHander());

            registry.Register("relatedUrl", new QDRelatedDocumentComponentEventHandler());
            registry.Register("relatedFile", new QDRelatedDocumentComponentEventHandler());
            registry.Register("relatedDocumentTitle", new QDRelatedDocumentComponentEventHandler());
            registry.Register("relatedDocumentContainer", new QDRelatedDocumentContainerEventHandler());
        }

        private static void RegisterMarkerEventHandlers()
        {
            MarkerEventHandlerRegistry registry = MarkerEventHandlerRegistry.Instance;
            registry.Register("alternatives.add", new AddAlternativeEventHandler());

            registry.Register("relatedDocuments.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedUrlUrl.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedUrl.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedFile.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedDocumentTitle.addRelatedFile", new AddRelatedDocumentHandler());

            registry.Register("relatedDocuments.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("relatedUrlUrl.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("relatedUrl.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("relatedFile.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("relatedDocumentTitle.addRelatedUrl", new AddRelatedUrlHandler());

            registry.Register("relatedDocumentContainer.moveUp", new MoveUpDocumentHandler());
            registry.Register("relatedUrlUrl.moveUp", new MoveUpDocumentHandler());
            registry.Register("relatedUrl.moveUp", new MoveUpDocumentHandler());
            registry.Register("relatedFile.moveUp", new MoveUpDocumentHandler());
            registry.Register("relatedDocumentTitle.moveUp", new MoveUpDocumentHandler());

            registry.Register("relatedDocumentContainer.moveDown", new MoveDownDocumentHandler());
            registry.Register("relatedUrlUrl.moveDown", new MoveDownDocumentHandler());
            registry.Register("relatedUrl.moveDown", new MoveDownDocumentHandler());
            registry.Register("relatedFile.moveDown", new MoveDownDocumentHandler());
            registry.Register("relatedDocumentTitle.moveDown", new MoveDownDocumentHandler());

            registry.Register("relatedDocumentContainer.delete", new MarkerDeleteRelatedDocumentEventHandler());
            registry.Register("relatedUrlUrl.delete", new MarkerDeleteRelatedDocumentEventHandler());
            registry.Register("relatedUrl.delete", new MarkerDeleteRelatedDocumentEventHandler());
            registry.Register("relatedFile.delete", new MarkerDeleteRelatedDocumentEventHandler());
            registry.Register("relatedDocumentTitle.delete", new MarkerDeleteRelatedDocumentEventHandler());

            registry.Register("alternative.add", new AddAlternativeEventHandler());
            registry.Register("alternativeState.add", new AddAlternativeEventHandler());
            registry.Register("alternativeIdentifier.add", new AddAlternativeEventHandler());
            registry.Register("alternativeTitle.add", new AddAlternativeEventHandler());
            registry.Register("alternativeDescription.add", new AddAlternativeEventHandler());

            registry.Register("alternative.delete", new MarkerDeleteAlternativeEventHandler());
            registry.Register("alternativeState.delete", new MarkerDeleteAlternativeEventHandler());
            registry.Register("alternativeIdentifier.delete", new MarkerDeleteAlternativeEventHandler());
            registry.Register("alternativeTitle.delete", new MarkerDeleteAlternativeEventHandler());
            registry.Register("alternativeDescription.delete", new MarkerDeleteAlternativeEventHandler());
            

            registry.Register("alternativeState.change", new EditAlternativeStateEventHandler());
            registry.Register("relatedFile.edit", new EditRelatedFileHandler());

            registry.Register("alternative.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternativeState.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternativeIdentifier.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternativeTitle.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternativeDescription.moveUp", new MoveUpAlternativeHandler());

            registry.Register("alternative.moveDown", new MoveDownAlternativeHandler());
            registry.Register("alternativeState.moveDown", new MoveDownAlternativeHandler());
            registry.Register("alternativeIdentifier.moveDown", new MoveDownAlternativeHandler());
            registry.Register("alternativeTitle.moveDown", new MoveDownAlternativeHandler());
            registry.Register("alternativeDescription.moveDown", new MoveDownAlternativeHandler());

            registry.Register("forces.add", new AddForceHandler());
            registry.Register("forceContainer.add", new AddForceHandler());
            registry.Register("forceConcern.add", new AddForceHandler());
            registry.Register("forceValue.add", new AddForceHandler());
            registry.Register("forceDescription.add", new AddForceHandler());

            registry.Register("forceContainer.delete", new StartDeleteForceEventHandler());
            registry.Register("forceConcern.delete", new StartDeleteForceEventHandler());
            registry.Register("forceValue.delete", new StartDeleteForceEventHandler());
            registry.Register("forceDescription.delete", new StartDeleteForceEventHandler());

            registry.Register("forceContainer.moveUp", new MoveUpForceHandler());
            registry.Register("forceConcern.moveUp", new MoveUpForceHandler());
            registry.Register("forceValue.moveUp", new MoveUpForceHandler());
            registry.Register("forceDescription.moveUp", new MoveUpForceHandler());

            registry.Register("forceContainer.moveDown", new MoveDownForceHandler());
            registry.Register("forceConcern.moveDown", new MoveDownForceHandler());
            registry.Register("forceValue.moveDown", new MoveDownForceHandler());
            registry.Register("forceDescription.moveDown", new MoveDownForceHandler());
        }

        private static void RegisterTextChangedEventHandlers()
        {
            TextChangedEventHandlerRegistry registry = TextChangedEventHandlerRegistry.Instance;
            registry.Register("forceValue", new ForceTextChangedEventHandler());
            registry.Register("alternativeState", new AlternativeStateTextChangedEventHandler());
        }

        //Fired when any text is changed
        private void Application_TextChangedEvent(Shape shape)
        { 
            if (shape.Document.Template.Contains(TemplateName) && shape.CellExistsU["User.rationallyType", 0] != 0)
            {
                Log.Debug("TextChanged: shapeName: " + shape.Name);
                string rationallyType = shape.CellsU["User.rationallyType"].ResultStr["Value"];
                TextChangedEventHandlerRegistry.Instance.HandleEvent(rationallyType, View, shape);
            }
        }

        //Fired when the user clicks on the main window from a different window.
        private void Application_WindowActivatedEvent(Window w)
        {
            if (w.Type == (short)VisWinTypes.visDrawing && w.Document.Template.Contains(TemplateName)) //VisDrawing is the main sheet
            {
                Log.Debug("window activated event handler enter");
                View.Page = Application.ActivePage;
                RebuildTree(w.Document);
            }
        }
        
        private void NoEventsPendingEventHandler(Application app) //Executed after all other events. Ensures we are never insides an undo scope
        {
            if (!app.IsUndoingOrRedoing && _rebuildTree)
            {
                Log.Debug("no events pending event handler entered. Rebuilding tree...");
                RebuildTree(app.ActiveDocument);
                _rebuildTree = false;
            }
        }

        private void Application_MarkerEvent(Application application, int sequence, string context)
        {
            if (application.ActiveDocument.Template.Contains(TemplateName))
            {
                Selection selection = Application.ActiveWindow.Selection; //event must originate from selected element
                
                foreach (Shape s in selection)
                {
                    if (s.CellExistsU["User.rationallyType", 0] != 0)
                    {
                        string identifier = context;
                        if (context.Contains("."))
                        {
                            identifier = context.Split('.')[1];
                            context = context.Split('.')[0];
                        }
                        Log.Debug("marker event being handled for: " + s.Name);
                        MarkerEventHandlerRegistry.Instance.HandleEvent(s.CellsU["User.rationallyType"].ResultStr["Value"] + "." + context, Model, s, identifier);
                    }
                }
            }
        }

        private void Application_CellChangedEvent(Cell cell)
        {
            Shape changedShape = cell.Shape;
            if (changedShape == null || !changedShape.Document.Template.Contains(TemplateName) || changedShape.CellExistsU["User.rationallyType", 0] == 0) //No need to continue when the shape is not part of our model.
            {
                return;
            }
            if (RelatedUrlComponent.IsRelatedUrlComponent(changedShape.Name) && cell.LocalName.Equals("Hyperlink.Row_1.Address")) //Link has updated
            {
                Log.Debug("cell changed of hyperlink shape:" + changedShape.Name);
                //find the container that holds all Related Documents
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)View.Children.First(c => c is RelatedDocumentsContainer);
                //find the related document holding the changed shape (one of his children has RShape equal to changedShape)
                RelatedDocumentContainer relatedDocumentContainer = relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(dc => dc.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0);
                //update the text of the URL display component to the new url
                RelatedURLURLComponent relatedURLURLComponent = (RelatedURLURLComponent)relatedDocumentContainer.Children.First(c => c is RelatedURLURLComponent);
                relatedURLURLComponent.Text = changedShape.Hyperlink.Address;
            }
            else if (Application.IsUndoingOrRedoing && ForceContainer.IsForceContainer(changedShape.Name) && cell.LocalName.Equals("User.forceIndex")) 
            {
                Log.Debug("forceindex cell changed of forcecontainer. shape:" + changedShape.Name);
                RComponent forcesComponent = View.Children.FirstOrDefault(x => x is ForcesContainer);
                if (forcesComponent != null)
                {
                    _rebuildTree = true; //Wait with the rebuild till the undo is done
                }
            }
            else if (Application.IsUndoingOrRedoing && AlternativeContainer.IsAlternativeContainer(changedShape.Name) && cell.LocalName.Equals("User.alternativeIndex"))
            {
                Log.Debug("alternative index cell changed of alternativecontainer. shape:" + changedShape.Name);
                RComponent alternativesComponent = View.Children.FirstOrDefault(x => x is AlternativesContainer);
                if (alternativesComponent != null)
                {
                    _rebuildTree = true; //Wait with the rebuild till the undo is done
                }
            }
            else if (Application.IsUndoingOrRedoing && RelatedDocumentContainer.IsRelatedDocumentContainer(changedShape.Name) && cell.LocalName.Equals("User.documentIndex"))
            {
                Log.Debug("document index cell changed of documentcontainer. shape:" + changedShape.Name);
                RComponent docComponent = View.Children.FirstOrDefault(x => x is RelatedDocumentsContainer);
                if (docComponent != null)
                {
                    _rebuildTree = true; //Wait with the rebuild till the undo is done
                }
            }
        }

        public void RebuildTree(IVDocument d) //Completely rebuild the model
        {
            View.Children.Clear(); 
            Model.Alternatives.Clear();
            Model.Documents.Clear();
            Model.Forces.Clear();
            foreach (Page page in d.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    View.AddToTree(shape, false);
                }
            }
        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            if (s.CellExistsU["User.rationallyType", 0] != 0 && !View.ExistsInTree(s))
            {
                View.AddToTree(s, true);
            }
        }

        private bool Application_QueryCancelSelectionDelete(Selection e) //Fired before a shape is deleted. Shape still exists here
        {
            List<Shape> toBeDeleted = e.Cast<Shape>().ToList();
            Log.Debug("before shape deleted event for " + e.Count + " shapes.");
            //store the rationally type of the last shape, which is responsible for ending the undo scope
            if (string.IsNullOrEmpty(LastDelete) && StartedUndoState == 0)
            {
                LastDelete = toBeDeleted.Last().Name;
                Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("Delete shape");
            }
            //all shapes in the selection are already bound to be deleted. Mark them, so other pieces of code don't also try to delete them, if they are in the tree.
            toBeDeleted.Where(s => View.ExistsInTree(s)).ToList().ForEach(tbd => View.GetComponentByShape(tbd).Deleted = true);
            foreach (Shape s in e)
            {
                Log.Debug("deleted shape name: " + s.Name);
                if (s.CellExistsU["User.rationallyType", 0] != 0)
                {
                    string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];

                    QueryDeleteEventHandlerRegistry.Instance.HandleEvent(rationallyType, View, s);
                }
            }
            return false;
        }
        private void Application_BeforePageDeleteEvent(Page p)
        {
            Log.Debug("page delete event handler entered");
            if (p.Document.Template.Contains(TemplateName))
            {
                foreach (Shape shape in p.Shapes)
                {
                    View.DeleteFromTree(shape);
                }
            }
        }

        private void Application_DeleteShapeEvent(Shape s) //Fired when a shape is deleted. Shape now no longer exists
        {
            Log.Debug("shape deleted event for: " + s.Name);
            if (s.Document.Template.Contains(TemplateName))
            {
                if (s.CellExistsU["User.isStub", 0] != 0)
                {
                    return;
                }
                if (s.CellExistsU["User.rationallyType", 0] != 0)
                {
                    string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];

                    //mark the deleted shape as 'deleted' in the view tree
                    RComponent deleted = View.GetComponentByShape(s);
                    if (deleted != null)
                    {
                        deleted.Deleted = true;
                    }
                    DeleteEventHandlerRegistry.Instance.HandleEvent(rationallyType, Model, s);
                }
                else
                {
                    if (StartedUndoState == 0)
                    {
                        RebuildTree(s.ContainingPage.Document);
                    }
                }
                if (StartedUndoState != 0 && s.Name == LastDelete)
                {
                    Log.Debug("ending undo scope");
                   Application.EndUndoScope(StartedUndoState, true);
                    StartedUndoState = 0;
                    LastDelete = "";
                }
            }
        }

        //Designer method. Called when application is started.
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
        }
        
        private void DelegateCreateDocumentEvent(IVDocument d)
        {
            if (d.Template.Contains(TemplateName))
            {
                new DocumentCreatedEventHandler(d, Model);
            }
        }
    }
}
