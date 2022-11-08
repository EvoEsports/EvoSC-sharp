using EvoSC.Common.Events.Arguments;

namespace EvoSC.Common.Events;

public delegate Task AsyncEventHandler<TArgs>(object sender, TArgs e) where TArgs : EventArgs;
public delegate Task AsyncEventHandler(object sender, EventArgs e);
