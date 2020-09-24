using BancoDigital.Models;
using BancoDigital.Service;
using BancoDigital.Tests.FakeRepository;
using NUnit.Framework;
using static BancoDigital.Models.ResultadoOperacao;

namespace BancoDigital.Tests.UniTests
{
    public class ContaServiceTest
    {
        [Test]
        public void Sacar_Deve_Retornar_Saldo_Atualizado()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 100 });

            //Act
            var resultado = contaService.Sacar(numeroConta, 20);

            //Assert
            Assert.AreEqual(80, resultado.Conta.Saldo);
        }

        [Test]
        public void Sacar_Valor_Maior_Que_Saldo_Deve_Retornar_Erro()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 100 });

            //Act
            var resultado = contaService.Sacar(numeroConta, 200);

            //Assert
            Assert.AreEqual(enResultado.Falha, resultado.Resultado);
            Assert.AreEqual("Saldo insuficiente.", resultado.Mensagem);
            Assert.IsNull(resultado.Conta);
        }

        [Test]
        public void Depositar_Deve_Retornar_Valor_Atualizado()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 0 });

            //Act
            var resultado = contaService.Depositar(numeroConta, 200.50);

            //Assert
            Assert.AreEqual(200.50, resultado.Conta.Saldo);
        }

        [Test]
        public void Deve_Exibir_Saldo_Corretamente()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 100 });

            //Act
            var resultado = contaService.Saldo(numeroConta);

            //Assert
            Assert.AreEqual(100, resultado.Conta.Saldo);
        }

        [Test]
        public void NaoDeve_Depositar_ValorNegativo()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 500 });

            //Act
            var resultado = contaService.Depositar(numeroConta, -2);

            //Assert
            Assert.AreEqual(enResultado.Falha, resultado.Resultado);
            Assert.AreEqual("Não é possível depositar valores negativos ou iguais a zero.", resultado.Mensagem);
        }

        [Test]
        public void NaoDeve_Depositar_ValorZerado()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 500 });

            //Act
            var resultado = contaService.Depositar(numeroConta, 0);

            //Assert
            Assert.AreEqual(enResultado.Falha, resultado.Resultado);
            Assert.AreEqual("Não é possível depositar valores negativos ou iguais a zero.", resultado.Mensagem);
        }

        [Test]
        public void NaoDeve_Sacar_ComNumeroContaInvalido()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 500 });

            //Act
            var resultado = contaService.Sacar(45, -2);

            //Assert
            Assert.AreEqual(enResultado.Falha, resultado.Resultado);
            Assert.AreEqual("Conta não localizada. Efetue o primeiro depósito para cadastrar.", resultado.Mensagem);
        }

        [Test]
        public void NaoDeve_ConsultarSaldo_ComNumeroContaInvalido()
        {
            //Arrange
            var numeroConta = 123;
            var contaContextFake = new ContaRepositoryFake();
            var contaService = new ContaService(contaContextFake);

            contaContextFake._contasSalvas.Add(new ContaCliente() { Conta = numeroConta, Saldo = 500 });

            //Act
            var resultado = contaService.Saldo(45);

            //Assert
            Assert.AreEqual(enResultado.Falha, resultado.Resultado);
            Assert.AreEqual("Conta não localizada. Efetue o primeiro depósito para cadastrar.", resultado.Mensagem);
        }
    }
}