namespace TagShelfLocator.UI.Services.ReaderManagement.Model;

using FEDM;

public class ReaderDescription
{

  public ReaderDescription(ReaderModule readerModule)
  {
    ReaderModule = readerModule;
  }

  public ReaderModule ReaderModule { get; init; }
  public uint DeviceID { get; set; }
  public string DeviceName { get; set; }

}
