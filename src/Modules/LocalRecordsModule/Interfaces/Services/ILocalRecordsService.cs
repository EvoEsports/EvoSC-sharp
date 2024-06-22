﻿using EvoSC.Common.Interfaces.Models;

namespace EvoSC.Modules.Official.LocalRecordsModule.Interfaces.Services;

public interface ILocalRecordsService
{
    public Task<IEnumerable<ILocalRecord>> GetLocalsOfCurrentMapFromPosAsync();
    public Task ShowWidgetAsync(IPlayer player);
    public Task ShowWidgetToAllAsync();
}
