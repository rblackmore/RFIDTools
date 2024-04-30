namespace ElectroCom.RFIDTools.ReaderServices.Model;
/// <summary>
/// See: https://enterprisecraftsmanship.com/posts/value-object-better-implementation/.
/// </summary>\
[Serializable]
public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
  private int? cachedHashCode;

  public static bool operator ==(ValueObject a, ValueObject b)
  {
    if (a is null && b is null)
      return true;

    if (a is null || b is null)
      return false;

    return a.Equals(b);
  }

  public static bool operator !=(ValueObject a, ValueObject b)
  {
    return !(a == b);
  }

  public override bool Equals(object? obj)
  {
    if (obj is not ValueObject other)
      return false;

    if (ReferenceEquals(this, other))
      return true;

    if (GetType() != other.GetType())
      return false;

    return GetEqualityComponents()
      .SequenceEqual(other.GetEqualityComponents());
  }

  public override int GetHashCode()
  {
    if (!cachedHashCode.HasValue)
    {
      var hashCode = new HashCode();

      foreach (var item in GetEqualityComponents())
        hashCode.Add(item.GetHashCode());

      cachedHashCode = hashCode.ToHashCode();
    }

    return cachedHashCode.Value;
  }

  public int CompareTo(object? obj)
  {
    return CompareTo(obj as ValueObject);
  }

  public int CompareTo(ValueObject? other)
  {
    if (other is null)
      return 1;

    var thisType = GetType();
    var otherType = other.GetType();

    if (thisType != otherType)
    {
      return string.Compare(
        thisType.ToString(),
        otherType.ToString(),
        StringComparison.Ordinal);
    }

    var components = GetEqualityComponents().ToArray();
    var otherComponents = other.GetEqualityComponents().ToArray();

    for (var i = 0; i < components.Length; i++)
    {
      var comparison = CompareComponents(components[i], otherComponents[i]);

      if (comparison != 0)
        return comparison;
    }

    return 0;
  }

  /// <summary>
  /// Implemented by sub classes to return all values used for equality comparisons.
  /// </summary>
  /// <returns>List of values to equate.</returns>
  protected abstract IEnumerable<object> GetEqualityComponents();

  private static int CompareComponents(object obj1, object obj2)
  {
    if (obj1 is null && obj2 is null)
      return 0;

    if (obj1 is null)
      return -1;

    if (obj2 is null)
      return 1;

    if (obj1 is IComparable comparable1 && obj2 is IComparable comparable2)
      return comparable1.CompareTo(comparable2);

    return obj1.Equals(obj2) ? 0 : -1;
  }
}
