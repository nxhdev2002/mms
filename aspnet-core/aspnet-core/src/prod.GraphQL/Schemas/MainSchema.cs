using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using prod.Queries.Container;
using System;

namespace prod.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}