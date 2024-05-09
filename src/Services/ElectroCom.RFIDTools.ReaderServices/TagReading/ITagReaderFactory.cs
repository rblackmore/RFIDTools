namespace ElectroCom.RFIDTools.ReaderServices;

public interface ITagReaderFactory
{
  ITagReader Create(TagReaderOptions options);
}