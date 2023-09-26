using NattiEvents.Options;
using Serilog;

namespace NattiEvents.Installers;

public static class SerilogInstaller
{
    public static WebApplicationBuilder InstallSerilog(this WebApplicationBuilder builder)
    {
        var serilogOptions = builder.Configuration
            .GetSection(SerilogOptions.Serilog)
            .Get<SerilogOptions>();

        ArgumentNullException.ThrowIfNull(serilogOptions);

        var seqOptions = builder.Configuration.GetSection(SeqOptions.Seq).Get<SeqOptions>();

        ArgumentNullException.ThrowIfNull(seqOptions);

        Log.Logger = new LoggerConfiguration().ReadFrom
            .Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Conditional(
                _ => serilogOptions.EnableConsole,
                configuration => configuration.Console()
            )
            .WriteTo.Conditional(
                _ => serilogOptions.EnableSeq,
                configuration => configuration.Seq(seqOptions.ServerUrl, apiKey: seqOptions.ApiKey)
            )
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}
