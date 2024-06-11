namespace ElectroCom.RFIDTools.UI.Logic.ViewModels;

using CommunityToolkit.Mvvm.Messaging;

using ElectroCom.RFIDTools.ReaderServices;

using Microsoft.Extensions.Logging;

public class ReaderManagementVM : ViewModel,
  IReaderManagementVM,
  IDisposable,
  IRecipient<ReaderRegistered>,
  IRecipient<ReaderUnregistered>
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
    this.messenger.RegisterAll(this);
    this.readerManager = readerManager;
    this.Readers = [];

    this.UpdateReaderList();
  }

  public ObservableReaderDetailsCollection Readers { get; private set; }

  public void Receive(ReaderRegistered message)
  {
    this.UpdateReaderList();
  }

  public void Receive(ReaderUnregistered message)
  {
    this.UpdateReaderList();
  }

  /// <summary>
  /// Updates <see cref="Readers"/> When a change in the readers on the readermanager changes.
  /// </summary>
  private void UpdateReaderList()
  {
    DispatcherHelper.CheckBeginInvokeOnUI(() =>
    {
      this.Readers.Clear();

      foreach (var reader in this.readerManager.GetReaderDefinitions())
      {
        reader.DetectReader();
        this.Readers.Add(new ObservableReaderDetails(reader));
      }
    });
  }

  public void Dispose()
  {
    this.messenger.UnregisterAll(this);
  }

}
