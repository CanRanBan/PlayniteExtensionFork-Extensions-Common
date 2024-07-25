using System.Collections.Generic;
using System.Windows;
using ItchioLibrary.Models;
using Playnite.SDK;

namespace ItchioLibrary
{
    public class StartupSelectionViewModel
    {
        private readonly Window window;

        public List<PickActionAction> Actions { get; set; }
        public int SelectedActionIndex { get; private set; } = -1;

        public RelayCommand<PickActionAction> SelectCommand
        {
            get => new RelayCommand<PickActionAction>((a) =>
            {
                SelectedActionIndex = a.actionIndex;
                window.Close();
            });
        }

        public RelayCommand CancelCommand
        {
            get => new RelayCommand(() => { window.Close(); });
        }

        public StartupSelectionViewModel(Window window, List<PickActionAction> actions)
        {
            this.window = window;
            Actions = actions;
        }
    }
}
