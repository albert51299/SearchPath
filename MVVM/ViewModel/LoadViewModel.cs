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
                }));
            }
        }

        private MyRelayCommand loadCommand;
        public MyRelayCommand LoadCommand {
            get {
                return loadCommand ?? (loadCommand = new MyRelayCommand(obj => {
                    IsConfirmed = true;
                    // close window
                }));
            }
        }

        private MyRelayCommand cancelCommand;
        public MyRelayCommand CancelCommand {
            get {
                return cancelCommand ?? (cancelCommand = new MyRelayCommand(obj => {
                    // close window
                }));
            }
        }

        private MyRelayCommand removeCommand;
        public MyRelayCommand RemoveCommand {
            get {
                return removeCommand ?? (removeCommand = new MyRelayCommand(obj => {
                    
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
