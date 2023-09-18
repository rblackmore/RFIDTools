using FEDM;

var reader = new ReaderModule(RequestMode.UniDirectional);
Console.WriteLine("Starting Discover");
UsbManager.startDiscover(new UsbConnectionListener(reader));

Console.Read();

UsbManager.stopDiscover();

public class UsbConnectionListener : IUsbListener
{
  private readonly ReaderModule reader;

  public UsbConnectionListener(ReaderModule reader)
  {
    this.reader = reader;
  }

  public void onUsbEvent()
  {
    Console.WriteLine("Usb Event");

    var scanInfo = UsbManager.popDiscover();

    while (scanInfo.isValid())
    {
      if (scanInfo.isNewReader())
      {
        var usbConnector = scanInfo.connector();
        this.reader.connect(usbConnector);
        Console.WriteLine($"Reader Connected: {scanInfo.deviceIdToHexString()}");
      }

      if (scanInfo.isReaderGone())
      {
        Console.WriteLine($"Reader Disconnected: {scanInfo.deviceIdToHexString()}");
      }

      scanInfo = UsbManager.popDiscover();
    }
  }
}
