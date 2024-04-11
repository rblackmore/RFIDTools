namespace TagShelfLocator.UI.Services.ReaderManagement;
using FEDM;

public class ReaderManager : IReaderManager
{
  private ReaderModule selectedReader;

  public ReaderManager()
  {
    this.selectedReader = new ReaderModule(RequestMode.UniDirectional);
  }

  public ReaderModule SelectedReader => this.selectedReader;
}
// This is code for logging the reader protocol details to a file.
// This was originally configured in teh App Startup, but this is likely where it should belong int he future.
//var timestamp = DateTime.Now.ToString("dd-HHmmss");

//string logFile = $"protocollog{timestamp}.log";
//var appLoggingParams = AppLoggingParam.createFileLogger(logFile);

//if (builder.Environment.IsDevelopment())
//{
//  Serilog.Log.Logger.Information("Protocol Log File: {logfile}", logFile);
//  reader.log().startLogging(appLoggingParams);
//}
