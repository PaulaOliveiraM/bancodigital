using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using BancoDigital.API;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace BancoDigital.Tests.IntegrationTests.graphQL
{
    public class BancoDigitalGraphQLIntegrationTest
    {
        private readonly HttpClient _client;

        public BancoDigitalGraphQLIntegrationTest()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            var webHostBuilder = new WebHostBuilder();

            webHostBuilder.ConfigureAppConfiguration(o =>
            {
                o.AddJsonFile(configPath);
            });
            webHostBuilder.UseStartup<Startup>();

            var server = new TestServer(webHostBuilder);

            // Necessário para fazer requisições assíncronas
            server.AllowSynchronousIO = true;

            _client = server.CreateClient();
        }

        [Test]
        public async Task Cliente_Deve_Depositar_Valor()
        {
            //DADO QUE eu consuma a API
            const string mutation = @"{
                ""query"": ""mutation{ depositar(conta:1, valor:250.65){ conta saldo }}""
            }";
            var content = new StringContent(mutation, Encoding.UTF8, "application/json");

            //QUANDO eu chamar a mutation depositar informando o número da conta e um valor válido
            var response = await _client.PostAsync("/graphql", content);

            //ENTÃO a mutation atualizará o saldo da conta no banco de dados
            //E a mutation retornará o saldo atualizado.
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var dadosConta = JObject.Parse(responseString);

            Assert.NotNull(dadosConta);
            Assert.AreEqual(1, (int)dadosConta["data"]["depositar"]["conta"]);
            Assert.NotZero((decimal)dadosConta["data"]["depositar"]["saldo"]);
        }

        [Test]
        public async Task Cliente_Deve_Sacar_Valor()
        {
            //DADO QUE eu consuma a API
            const string mutation = @"{
                ""query"": ""mutation{ sacar(conta:1, valor:10){ conta saldo }}""
            }";
            var content = new StringContent(mutation, Encoding.UTF8, "application/json");

            //QUANDO eu chamar a mutation sacar informando o número da conta e um valor válido
            var response = await _client.PostAsync("/graphql", content);

            //ENTÃO o saldo da minha conta no banco de dados diminuirá de acordo
            // E a mutation retornará o saldo atualizado.
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var dadosConta = JObject.Parse(responseString);

            Assert.NotNull(dadosConta);
            Assert.AreEqual(1, (int)dadosConta["data"]["sacar"]["conta"]);
            Assert.NotZero((decimal)dadosConta["data"]["sacar"]["saldo"]);
        }

        [Test]
        public async Task Cliente_Nao_Deve_Sacar_Valor_Maior_Que_Saldo()
        {
            //DADO QUE eu consuma a API
            const string mutation = @"{
                ""query"": ""mutation{ sacar(conta:1, valor:10000000000000000){ conta saldo }}""
            }";
            var content = new StringContent(mutation, Encoding.UTF8, "application/json");

            //QUANDO eu chamar a mutation sacar informando o número da conta e um valor maior do que o meu saldo
            var response = await _client.PostAsync("/graphql", content);

            //ENTÃO a mutation me retornará um erro do GraphQL informando que eu não tenho saldo suficiente
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var dadosConta = JObject.Parse(responseString);

            Assert.NotNull(dadosConta);
            Assert.AreEqual("Saldo insuficiente.", (string)dadosConta["errors"][0]["message"]);
        }

        [Test]
        public async Task Cliente_Deve_Consultar_Saldo()
        {
            //DADO QUE eu consuma a API
            const string query = @"{
                ""query"": ""query{ saldo(conta:1)}""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            //QUANDO eu chamar a query saldo informando o número da conta
            var response = await _client.PostAsync("/graphql", content);

            //ENTÃO a query retornará o saldo atualizado.
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var saldo = JObject.Parse(responseString);

            Assert.NotNull(saldo);
            Assert.NotZero((decimal)saldo["data"]["saldo"]);
        }

    }
}
