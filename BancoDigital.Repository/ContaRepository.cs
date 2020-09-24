
using BancoDigital.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace BancoDigital.Repository
{
    public interface IContaRepository
    {
        ContaCliente BuscarContaPorNumero(int numeroConta);
        ContaCliente Salvar(ContaCliente conta);
    }

    public class ContaRepository : IContaRepository
    {
        private IMongoDatabase _database;

        public ContaRepository(string connectionString)
        {
            var client = new MongoClient("connectionString");
            _database = client.GetDatabase("BancoDigital");
        }

        public ContaRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("BancoDigitalConnection"));
            _database = client.GetDatabase("BancoDigital");
        }

        public ContaCliente Salvar(ContaCliente contaCliente)
        {
            var filtro = Builders<ContaCliente>.Filter.Eq("Conta", contaCliente.Conta);
            var opcoes = new FindOneAndUpdateOptions<ContaCliente, ContaCliente>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
            var contaSalvar = Builders<ContaCliente>.Update
                 .Set(m => m.Saldo, contaCliente.Saldo);


            var contaSalva = _database.GetCollection<ContaCliente>("ContaCliente").FindOneAndUpdate(filtro, contaSalvar, opcoes);

            return contaSalva;
        }

        public ContaCliente BuscarContaPorNumero(int numeroConta)
        {
            var filtro = Builders<ContaCliente>.Filter.Eq("Conta", numeroConta);
            return _database.GetCollection<ContaCliente>("ContaCliente").Find(filtro).FirstOrDefault();
        }

    }
}
