using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace MVVM {
    class AppViewModel {
        private RelayCommand canvasMouseUpCommand;
        public RelayCommand CanvasMouseUpCommand {
            get {
                return canvasMouseUpCommand ??
                    (canvasMouseUpCommand = new RelayCommand(obj => {
                        MainWindow mainWindow = obj as MainWindow;
                        
                    }));
            }
        }

        public ObservableCollection<Circle> Circles { get; set; }

        public AppViewModel() {
            Circles = new ObservableCollection<Circle> {
                new Circle(),
                new Circle(),
                new Circle()
            };
            Circles[0].X = 20.0;
            Circles[0].Y = 20.0;
            Circles[1].X = 50.0;
            Circles[1].Y = 50.0;
            Circles[2].X = 80.0;
            Circles[2].Y = 80.0;
            Circles[2].Name = "AAA";
        }
    }
}
