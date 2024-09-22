using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StopApi.Services
{
    public class CalculoEstacionamento
    {
        private readonly decimal _valorPorHora;

        public CalculoEstacionamento(decimal valorPorHora)
        {
            _valorPorHora = valorPorHora; 
        }

        public decimal CalcularValor(DateTime entrada, DateTime saida)
        {
            TimeSpan duracao = saida - entrada;
            decimal valorTotal = (decimal)duracao.TotalHours * _valorPorHora; 
            return valorTotal;
        }
    }

}