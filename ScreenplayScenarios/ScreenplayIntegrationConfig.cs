using CSF.Screenplay;
using CSF.Screenplay.Reporting;

namespace ScreenplayScenarios
{
    public class ScreenplayIntegrationConfig : CSF.Screenplay.Integration.IIntegrationConfig
    {
        public void Configure(CSF.Screenplay.Integration.IIntegrationConfigBuilder builder)
        {
            builder.ServiceRegistrations.PerTestRun.Add(i => i.RegisterFactory(() => new MyAbility()));
        }
    }
}
