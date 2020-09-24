using BancoDigital.Models;
using GraphQL.Types;

namespace BancoDigital.API.GraphQL.Types
{
    public class ContaType : ObjectGraphType<ContaCliente>
    {
        public ContaType()
        {
            Field(x => x._id, type: typeof(IdGraphType))
                .Description("Id da conta");
            Field(x => x.Conta)
                .Description("Número da conta");
            Field(x => x.Saldo)
                .Description("Saldo da conta");

        }
    }
}
