using BancoDigital.Models;
using BancoDigital.Repository;
using static BancoDigital.Models.ResultadoOperacao;

namespace BancoDigital.Service
{
    public interface IContaService
    {
        ResultadoOperacaoContaService Depositar(int numeroConta, double valor);
        ResultadoOperacaoContaService Sacar(int numeroConta, double valor);
        ResultadoOperacaoContaService Saldo(int numeroConta);
    }

    public class ContaService : IContaService
    {
        private IContaRepository _contaContext;
        private const string _mensagemContaNaoLocalizada = "Conta não localizada. Efetue o primeiro depósito para cadastrar.";

        public ContaService(IContaRepository contaContext)
        {
            _contaContext = contaContext;
        }

        public ResultadoOperacaoContaService Depositar(int numeroConta, double valor)
        {
            if (numeroConta <= 0)
                return new ResultadoOperacaoContaService(enResultado.Falha, "Número de conta inválido", null);

            if (valor <= 0)
                return new ResultadoOperacaoContaService(enResultado.Falha, "Não é possível depositar valores negativos ou iguais a zero.", null);

            var contaLocalizada = _contaContext.BuscarContaPorNumero(numeroConta);
            var contaSalva = _contaContext.Salvar(new ContaCliente { Conta = numeroConta, Saldo = contaLocalizada != null ? contaLocalizada.Saldo + valor : valor });

            return new ResultadoOperacaoContaService(enResultado.Sucesso, null, contaSalva);
        }

        public ResultadoOperacaoContaService Sacar(int numeroConta, double valor)
        {
            var contaLocalizada = _contaContext.BuscarContaPorNumero(numeroConta);

            if (contaLocalizada == null)
                return new ResultadoOperacaoContaService(enResultado.Falha, _mensagemContaNaoLocalizada, null);

            if (valor > contaLocalizada.Saldo)
                return new ResultadoOperacaoContaService(enResultado.Falha, "Saldo insuficiente.", null);

            var contaSalva = _contaContext.Salvar(new ContaCliente { Conta = numeroConta, Saldo = contaLocalizada.Saldo - valor });

            return new ResultadoOperacaoContaService(enResultado.Sucesso, null, contaSalva);
        }

        public ResultadoOperacaoContaService Saldo(int numeroConta)
        {
            var contaLocalizada = _contaContext.BuscarContaPorNumero(numeroConta);

            if (contaLocalizada == null)
                return new ResultadoOperacaoContaService(enResultado.Falha, _mensagemContaNaoLocalizada, null);

            return new ResultadoOperacaoContaService(enResultado.Sucesso, null, contaLocalizada);

        }

    }

    public class ResultadoOperacaoContaService : ResultadoOperacao
    {
        public ContaCliente Conta { get; set; }
        public ResultadoOperacaoContaService(enResultado enResultado, string mensagem, ContaCliente conta) : base(enResultado, mensagem)
        {
            Conta = conta;
        }
    }
}
