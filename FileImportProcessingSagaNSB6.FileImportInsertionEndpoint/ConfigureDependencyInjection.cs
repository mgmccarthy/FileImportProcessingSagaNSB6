using FileImportProcessingSagaNSB6.FileImportInsertionEndpoint.Data;
using NServiceBus;

namespace FileImportProcessingSagaNSB6.FileImportInsertionEndpoint
{
    public class ConfigureDependencyInjection : INeedInitialization
    {
        public void Customize(EndpointConfiguration configuration)
        {
            configuration.RegisterComponents(reg => reg.ConfigureComponent<DataStore>(DependencyLifecycle.InstancePerCall));
        }
    }
}
