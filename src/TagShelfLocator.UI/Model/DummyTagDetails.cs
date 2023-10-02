namespace TagShelfLocator.UI.Model;

using System;

public class DummyTagDetails : ObservableTagDetails
{
  public override string IDD => "12345ABCDEF";

  public override string EPC => "12345";

  public override string TID => "ABCDEF";

}
