namespace TagShelfLocator.UI.Model;

using FEDM.TagHandler;

public class OBIDTagData : ObservableTagDetails
{
  private readonly ThBase th;

  public OBIDTagData(ThBase th)
  {
    this.th = th;
  }

  public override string IDD => this.th.iddToHexString();

  public override string EPC => this.thEpcClass1Gen2?.epcToHexString() ?? string.Empty;

  public override string TID => this.thEpcClass1Gen2?.tidToHexString() ?? string.Empty;

  private ThEpcClass1Gen2? thEpcClass1Gen2 => th as ThEpcClass1Gen2;

  public static OBIDTagData CreateFrom(ThBase th) => new OBIDTagData(th);
}
