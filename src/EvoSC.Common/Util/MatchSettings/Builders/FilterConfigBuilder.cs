using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

/// <summary>
/// Fluent builder for the filter section of a MatchSettings.
/// </summary>
public class FilterConfigBuilder
{
    private bool _isLan = true;
    private bool _isInternet = true;
    private bool _isSolo;
    private bool _isHotseat;
    private int _sortIndex = 1000;
    private bool _randomMapOrder;

    public FilterConfigBuilder(){}
    
    /// <summary>
    /// Create the building from an existing filter object.
    /// </summary>
    /// <param name="filter">The filter object to start off with.</param>
    public FilterConfigBuilder(MatchSettingsFilter filter)
    {
        _isLan = filter.IsLan;
        _isInternet = filter.IsInternet;
        _isSolo = filter.IsSolo;
        _isHotseat = filter.IsHotseat;
        _sortIndex = filter.SortIndex;
        _randomMapOrder = filter.RandomMapOrder;
    }

    /// <summary>
    /// Set whether this filter is lan or not.
    /// </summary>
    /// <param name="isLan">Is lan or not.</param>
    /// <returns></returns>
    public FilterConfigBuilder AsLan(bool isLan)
    {
        _isLan = isLan;
        return this;
    }

    /// <summary>
    /// Set whether this filter is internet or not.
    /// </summary>
    /// <param name="isInternet">Internet or not.</param>
    /// <returns></returns>
    public FilterConfigBuilder AsInternet(bool isInternet)
    {
        _isInternet = isInternet;
        return this;
    }

    /// <summary>
    /// Set whether this filter is solo or not.
    /// </summary>
    /// <param name="isSolo">Solo or not.</param>
    /// <returns></returns>
    public FilterConfigBuilder AsSolo(bool isSolo)
    {
        _isSolo = isSolo;
        return this;
    }

    /// <summary>
    /// Set whether this filter is hotseat or not.
    /// </summary>
    /// <param name="isHotseat">Is hotseat or not.</param>
    /// <returns></returns>
    public FilterConfigBuilder AsHotseat(bool isHotseat)
    {
        _isHotseat = isHotseat;
        return this;
    }

    /// <summary>
    /// Set sorting index for this server.
    /// </summary>
    /// <param name="index">The sort index.</param>
    /// <returns></returns>
    public FilterConfigBuilder WithSortIndex(int index)
    {
        _sortIndex = index;
        return this;
    }

    /// <summary>
    /// Whether to use random map order or not.
    /// </summary>
    /// <param name="random">Random map order or not.</param>
    /// <returns></returns>
    public FilterConfigBuilder AsRandomMapOrder(bool random)
    {
        _randomMapOrder = random;
        return this;
    }

    /// <summary>
    /// Create the filter with the current values.
    /// </summary>
    /// <returns></returns>
    public MatchSettingsFilter Build() => new MatchSettingsFilter
    {
        IsLan = _isLan,
        IsInternet = _isInternet,
        IsSolo = _isSolo,
        IsHotseat = _isHotseat,
        SortIndex = _sortIndex,
        RandomMapOrder = _randomMapOrder
    };
}
