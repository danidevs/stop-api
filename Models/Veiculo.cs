using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StopApi.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime HoraSaida { get; set; }
        public bool Ativo { get; set; } = true;
      
    }
}