using BancoDigital.API.GraphQL.Types;
using BancoDigital.Models;
using BancoDigital.Service;
using GraphQL;
using GraphQL.Types;

namespace BancoDigital.API.GraphQL.Mutations
{
    public class BancoDigitalMutation : ObjectGraphType
    {
        public BancoDigitalMutation(IContaService contaService)
        {
            Name = "Mutation";
            Field<ContaType>("depositar",
                             arguments: new QueryArguments(
                                 new QueryArgument<IntGraphType> { Name = "conta" },
                                 new QueryArgument<DecimalGraphType> { Name = "valor" }
                             ),
                             resolve: context =>
                             {
                                 var numeroConta = context.GetArgument<int>("conta");
                                 var valor = context.GetArgument<decimal>("valor");
                                 var resultado = contaService.Depositar(numeroConta, double.Parse(valor.ToString()));
                                 return resultado.Resultado == ResultadoOperacao.enResultado.Sucesso ? resultado.Conta : throw new ExecutionError(resultado.Mensagem);
                             });

            Field<ContaType>("sacar",
                             arguments: new QueryArguments(
                                 new QueryArgument<IntGraphType> { Name = "conta" },
                                 new QueryArgument<DecimalGraphType> { Name = "valor" }
                             ),
                             resolve: context =>
                             {
                                 var numeroConta = context.GetArgument<int>("conta");
                                 var valor = context.GetArgument<decimal>("valor");
                                 var resultado = contaService.Sacar(numeroConta, double.Parse(valor.ToString()));
                                 return resultado.Resultado == ResultadoOperacao.enResultado.Sucesso ? resultado.Conta : throw new ExecutionError(resultado.Mensagem);
                             });

        }
    }
}
