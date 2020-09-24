using BancoDigital.Models;
using BancoDigital.Repository;
using System.Collections.Generic;
using System.Linq;

namespace BancoDigital.Tests.FakeRepository
{
    public class ContaRepositoryFake : IContaRepository
    {
        public List<ContaCliente> _contasSalvas = new List<ContaCliente>();
        public ContaCliente BuscarContaPorNumero(int numeroConta)
        {
            return _contasSalvas.FirstOrDefault(x => x.Conta == numeroConta);
        }

        public ContaCliente Salvar(ContaCliente conta)
        {
            _contasSalvas.Add(conta);
            return conta;
        }
    }
}
