using System.Xml;
using EvoSC.Common.Interfaces.Util;
using EvoSC.Common.Util.MatchSettings.Models;

namespace EvoSC.Common.Util.MatchSettings.Builders;

public class FilterConfigBuilder
{
    private bool _isLan = true;
    private bool _isInternet = true;
    private bool _isSolo = false;
    private bool _isHotseat = false;
    private int _sortIndex = 1000;
    private bool _randomMapOrder = false;


    public FilterConfigBuilder(){}
    
    public FilterConfigBuilder(MatchSettingsFilter filter)
    {
        _isLan = filter.IsLan;
        _isInternet = filter.IsInternet;
        _isSolo = filter.IsSolo;
        _isHotseat = filter.IsHotseat;
        _sortIndex = filter.SortIndex;
        _randomMapOrder = filter.RandomMapOrder;
    }

    public FilterConfigBuilder AsLan(bool isLan)
    {
        _isLan = isLan;
        return this;
    }

    public FilterConfigBuilder AsInternet(bool isInternet)
    {
        _isInternet = isInternet;
        return this;
    }

    public FilterConfigBuilder AsSolo(bool isSolo)
    {
        _isSolo = isSolo;
        return this;
    }

    public FilterConfigBuilder AsHotseat(bool isHotseat)
    {
        _isHotseat = isHotseat;
        return this;
    }

    public FilterConfigBuilder WithSortIndex(int index)
    {
        _sortIndex = index;
        return this;
    }

    public FilterConfigBuilder AsRandomMapOrder(bool random)
    {
        _randomMapOrder = random;
        return this;
    }

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
