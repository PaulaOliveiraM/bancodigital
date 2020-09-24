using BancoDigital.API.GraphQL.Types;
using BancoDigital.Service;
using BancoDigital.Models;
using GraphQL;
using GraphQL.Types;

namespace BancoDigital.API.GraphQL.Queries
{
    public class BancoDigitalQuery : ObjectGraphType
    {
        public BancoDigitalQuery(IContaService contaService)
        {
            Name = "Query";
            Field<DecimalGraphType>("saldo",
                                    arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "conta" }),
                                    resolve: context =>
                                    {
                                        var resultado = contaService.Saldo(context.GetArgument<int>("conta"));
                                        return resultado.Resultado == ResultadoOperacao.enResultado.Sucesso ? resultado.Conta.Saldo : throw new ExecutionError(resultado.Mensagem);
                                    });

        }
    }
}
