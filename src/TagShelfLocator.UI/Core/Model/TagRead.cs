namespace TagShelfLocator.UI.Core.Model;

using System.Collections.Generic;

using TagShelfLocator.UI.Core.Shared;

public class TagRead : ValueObject
{
 
  public string EPC { get; set; }
  public string TID { get; set; }




  protected override IEnumerable<object> GetEqualityComponents()
  {
    throw new System.NotImplementedException();
  }
}
