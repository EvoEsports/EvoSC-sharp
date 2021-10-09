using System;
using GameHost.V3.Injection;
using NLog;

namespace EvoSC.Injection
{
    /// <summary>
    /// Transient <see cref="DynamicDependency"/> that create a logger for us.
    /// </summary>
    /// <remarks>
    /// It search in the context for an object, which will be the source for the logger.
    /// </remarks>
    public class TransientLogger : DynamicDependency<ILogger>
    {
        public override ILogger CreateT<TContext>(TContext context)
        {
            if (!context.TryGet(out object source))
            {
                throw new InvalidCastException("no source");
            }

            var type = source.GetType();
            var logger = LogManager.LogFactory.GetLogger($"{type.Namespace}.{type.Name}");

            return logger;
        }
    }
}