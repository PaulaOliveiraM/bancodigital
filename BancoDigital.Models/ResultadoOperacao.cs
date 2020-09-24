using System;

namespace BancoDigital.Models
{
    public abstract class ResultadoOperacao
    {
        public ResultadoOperacao(enResultado resultado, string mensagem)
        {
            Resultado = resultado;
            Mensagem = mensagem;
        }
        public enum enResultado
        {
            Sucesso,
            Falha
        }
        public enResultado Resultado { get; set; }
        public string Mensagem { get; set; }
    }
}
