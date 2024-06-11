namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using System.ComponentModel;

using CommunityToolkit.Mvvm.ComponentModel;

public interface IViewModel : INotifyPropertyChanged
{

}

public abstract class ViewModel : ObservableObject, IViewModel
{

}
