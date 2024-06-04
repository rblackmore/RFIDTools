namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using Microsoft.Extensions.Logging;

public class ReaderManagementVM : ViewModel,
  IReaderManagementVM,
  IRecipient<ReaderRegistered>
{
  private ILogger<ReaderManagementVM> logger;
  private IMessenger messenger;
  private IReaderManager readerManager;

  public ReaderManagementVM(
    ILogger<ReaderManagementVM> logger,
    IMessenger messenger,
    IReaderManager readerManager)
  {
    this.logger = logger;
    this.messenger = messenger;
    this.readerManager = readerManager;
    this.Readers = [];
    foreach (var reader in this.readerManager.GetReaderDefinitions())
    {
      this.Readers.Add(new ObservableReaderDetails(reader));
    }
  }

  public ObservableReaderDetailsCollection Readers { get; private set; }

  public void Receive(ReaderRegistered message)
  {
    this.Readers.Add(new ObservableReaderDetails(message.ReaderDefinition));
  }
}
