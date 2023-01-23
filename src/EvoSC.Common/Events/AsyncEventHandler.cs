namespace EvoSC.Common.Events;

public delegate Task AsyncEventHandler<in TArgs>(object sender, TArgs e) where TArgs : EventArgs;
public delegate Task AsyncEventHandler(object sender, EventArgs e);
