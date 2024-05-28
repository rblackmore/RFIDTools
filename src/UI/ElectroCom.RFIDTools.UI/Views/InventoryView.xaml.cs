namespace ElectroCom.RFIDTools.UI.Views;

using System.Windows;
using System.Windows.Controls;

using ElectroCom.Common.Controls.ItemsControls;


/// <summary>
/// Interaction logic for InventoryView.xaml
/// </summary>
public partial class InventoryView : UserControl
{
  public InventoryView()
  {
    InitializeComponent();

  }

  private void TagInventoryList_SetListOption_Clicked(object sender, RoutedEventArgs e)
  {
    this.slb_TagInventory.Layout = Layout.List;
  }

  private void TagInventoryList_SetTileOption_Clicked(object sender, RoutedEventArgs e)
  {
    this.slb_TagInventory.Layout = Layout.Tile;
  }
}
