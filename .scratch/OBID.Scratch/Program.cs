using FEDM;

using OBID.Scratch;

var reader = new ReaderModule(RequestMode.UniDirectional);
var usbListener = new UsbListener(reader);
usbListener.ReaderConnected += Run;
await usbListener.StartAsync();



static void Run(object? sender, EventArgs e)
{
  
}


