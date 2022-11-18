using System;
using System.Threading.Tasks;
using EvoSC.Common.Controllers.Context;
using EvoSC.Common.Middleware;
using Xunit;

namespace EvoSC.Common.Tests;

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
                context.CustomData["first"] = true;
                return next(context);
            };
        });
        
        pipeline.AddComponent(next =>
        {
            return context =>
            {
                context.CustomData["second"] = true;
                return next(context);
            };
        });
        
        pipeline.AddComponent(next =>
        {
            return context =>
            {
                context.CustomData["third"] = true;
                return next(context);
            };
        });

        var chain = pipeline.Build();
        var context = new GenericControllerContext();
        await chain(context);
        
        Assert.Equal(true, context.CustomData["first"]);
        Assert.Equal(true, context.CustomData["second"]);
        Assert.Equal(true, context.CustomData["third"]);
    }

    [Fact]
    public async Task Execute_Combined_Pipeline_From_Pipeline_Builder()
    {
        var builder = new ActionPipelineBuilder()
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    context.CustomData["first"] = true;
                    return next(context);
                };
            }))
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    context.CustomData["second"] = true;
                    return next(context);
                };
            }))
            .AddPipeline(p => p.AddComponent(next =>
            {
                return context =>
                {
                    context.CustomData["third"] = true;
                    return next(context);
                };
            }));

        var chain = builder.Build();
        var context = new GenericControllerContext();
        await chain(context);
        
        Assert.Equal(true, context.CustomData["first"]);
        Assert.Equal(true, context.CustomData["second"]);
        Assert.Equal(true, context.CustomData["third"]);
    }

    [Fact]
    public async Task Pipeline_Builder_Fails_With_No_Pipelines()
    {
        var builder = new ActionPipelineBuilder();
        
        var buildAction = () => builder.Build();

        Assert.Throws<InvalidOperationException>(buildAction);
    }
}
