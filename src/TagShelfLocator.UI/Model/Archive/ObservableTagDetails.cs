namespace TagShelfLocator.UI.Model;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract class ObservableTagDetails : ObservableObject
{
  public abstract string IDD { get; }

  public abstract string EPC { get; }
  public abstract string TID { get; }

}
