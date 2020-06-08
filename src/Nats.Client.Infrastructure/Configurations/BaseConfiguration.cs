using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nats.Client.Infrastructure.Configurations
{
    public abstract class BaseConfiguration<T> where T : new()
    {
        protected readonly T Config = new T();

        protected BaseConfiguration(string sectionName, IConfiguration configuration)
        {
            configuration.GetSection(sectionName).Bind(Config);
        }
    }
}
