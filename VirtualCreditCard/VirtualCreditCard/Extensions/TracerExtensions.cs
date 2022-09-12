using Jaeger;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using OpenTracing;
using OpenTracing.Util;
using System.Reflection;

namespace VirtualCreditCard.Extensions
{
    public static class TracerExtensions
    {
        public static void AddTraceService(this IServiceCollection services)
        {
            services.AddSingleton<ITracer>(serverProvider =>
            {
                string serviceName = Assembly.GetEntryAssembly()!.GetName().Name!;
                Environment.SetEnvironmentVariable("JAEGER_AGENT_HOST", "192.168.1.1");
                Environment.SetEnvironmentVariable("JAEGER_SERVICE_NAME", serviceName);
                Environment.SetEnvironmentVariable("JAEGER_AGENT_PORT", "6831");
                Environment.SetEnvironmentVariable("JAEGER_SAMPLER_TYPE", "const");

                //ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var loggerFactory = new LoggerFactory();

                ISampler sampler = new ConstSampler(sample: true);

                Jaeger.Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
                    .RegisterSenderFactory<ThriftSenderFactory>();

                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .Build();

                GlobalTracer.Register(tracer);

                return tracer;
            });
        }
    }
}
