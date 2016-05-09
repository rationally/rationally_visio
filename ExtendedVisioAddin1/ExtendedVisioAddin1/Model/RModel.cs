using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    public class RModel : IObservable<RModel>
    {
        public Document RationallyDocument { get; }
        public ObservableCollection<Alternative> Alternatives { get; set; }
        public List<string> AlternativeStates { get; set; }

        private readonly List<IObserver<RModel>> observers; 

        public string Author { get; set; }

        public string DecisionName { get; set; }

        public string Date { get; set; }

        public string Version { get; set; }

        public RModel()
        {
            Alternatives = new ObservableCollection<Alternative>();
            Alternatives.CollectionChanged += AlternativesChangedHandler;
            observers = new List<IObserver<RModel>>();
            AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"};
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\DecisionsStencil.vssx";
            RationallyDocument = Globals.ThisAddIn.Application.Documents.OpenEx(docPath, (short)VisOpenSaveArgs.visAddHidden); //todo: handling for file is open
        }

        private void AlternativesChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyObservers();
        }

        public virtual void AddObserver(IObserver<RModel> observer)
        {
            observers.Add(observer);
        }

        public virtual void RemoveObserver(IObserver<RModel> observer)
        {
            observers.Remove(observer);
        }

        void NotifyObservers()
        {
            observers.ForEach(obs => obs.Notify(this));
        }


    }
}
