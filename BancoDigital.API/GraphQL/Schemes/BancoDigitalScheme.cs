using BancoDigital.API.GraphQL.Mutations;
using BancoDigital.API.GraphQL.Queries;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BancoDigital.API.GraphQL.Schemes
{
    public class BancoDigitalScheme : Schema
    {
        public BancoDigitalScheme(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<BancoDigitalQuery>();
            Mutation = resolver.Resolve<BancoDigitalMutation>();
        }

    }
}
