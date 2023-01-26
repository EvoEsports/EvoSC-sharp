using System;
using System.Threading.Tasks;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Middleware;
using Xunit;

namespace EvoSC.Common.Tests.Middleware;

public class MiddlewareTests
{
    [Fact]
    public async Task Middleware_Chain_Is_Executed()
    {
        var pipeline = new ActionPipeline();

        pipeline.AddComponent(next =>
        {
            return context =>
            {
                ((GenericControllerContext)context).CustomData["first"] = true;
                return next(context);
            };
        });
        
        pipeline.AddComponent(next =>
        {
            return context =>
            {
                ((GenericControllerContext)context).CustomData["second"] = true;
                return next(context);
            };
        });
        
        pipeline.AddComponent(next =>
        {
            return context =>
            {
                ((GenericControllerContext)context).CustomData["third"] = true;
                return next(context);
            };
        });

        var chain = pipeline.Build();
        var context = new GenericControllerContext();
        await chain(context);
        
        Assert.True((bool)context.CustomData["first"]);
        Assert.True((bool)context.CustomData["second"]);
        Assert.True((bool)context.CustomData["third"]);
    }

    [Fact]
    public async Task Execute_Combined_Pipeline_From_Pipeline_Builder()
    {
        var builder = new ActionPipelineBuilder()
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    ((GenericControllerContext)context).CustomData["first"] = true;
                    return next(context);
                };
            }))
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    ((GenericControllerContext)context).CustomData["second"] = true;
                    return next(context);
                };
            }))
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    ((GenericControllerContext)context).CustomData["third"] = true;
                    return next(context);
                };
            }));

        var chain = builder.Build();
        var context = new GenericControllerContext();
        await chain(context);
        
        Assert.True((bool)context.CustomData["first"]);
        Assert.True((bool)context.CustomData["second"]);
        Assert.True((bool)context.CustomData["third"]);
    }

    [Fact]
    public async Task Pipeline_Builder_Fails_With_No_Pipelines()
    {
        var builder = new ActionPipelineBuilder();
        
        var buildAction = () => builder.Build();

        Assert.Throws<InvalidOperationException>(buildAction);
    }
}
