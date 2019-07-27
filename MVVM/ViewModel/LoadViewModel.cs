using MVVM.Other;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MVVM.ViewModel {
    class LoadViewModel : INotifyPropertyChanged {
        public bool IsConfirmed { get; set; } = false;
        private ObservableCollection<string> sessions = new ObservableCollection<string>();
        public ObservableCollection<string> Sessions {
            get { return sessions; }
            set {
                sessions = value;
                OnPropertyChanged("Sessions");
            }
        }

        private bool haveSaves;
        public bool HaveSaves {
            get { return haveSaves; }
            set {
                haveSaves = value;
                OnPropertyChanged("HaveSaves");
            }
        }

        private string selectedSession;
        public string SelectedSession {
            get { return selectedSession; }
            set {
                selectedSession = value;
                OnPropertyChanged("SelectedSession");
            }
        }

        private MyRelayCommand loadedCommand;
        public MyRelayCommand LoadedCommand {
            get {
                return loadedCommand ?? (loadedCommand = new MyRelayCommand(obj => {
                    using (SearchPathContext db = new SearchPathContext()) {
                        var items = db.Sessions.Select(o => o.Name);
                        foreach (var item in items) {
                            Sessions.Add(item);
                        }
                    }
                    HaveSaves = (Sessions.Count != 0);
                }));
            }
        }

        private MyRelayCommand loadCommand;
        public MyRelayCommand LoadCommand {
            get {
                return loadCommand ?? (loadCommand = new MyRelayCommand(obj => {
                    IsConfirmed = true;
                }));
            }
        }

        private MyRelayCommand removeCommand;
        public MyRelayCommand RemoveCommand {
            get {
                return removeCommand ?? (removeCommand = new MyRelayCommand(obj => {
                    using (SearchPathContext db = new SearchPathContext()) {
                        var session = db.Sessions.FirstOrDefault(o => o.Name == SelectedSession);
                        db.Sessions.Remove(session);
                        db.SaveChanges();
                    }
                    Sessions.Remove(SelectedSession);
                    HaveSaves = (Sessions.Count != 0);
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
