using MongoDB.Bson;
using System;

namespace BancoDigital.Models
{
    public class ContaCliente
    {
        public ObjectId _id { get; set; }
        public int Conta { get; set; }
        public double Saldo { get; set; }
    }
}
