using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIN.Services
{
    internal class Date
    {
        public static string TiempoTranscurrido(DateTime fecha)
        {
            TimeSpan tiempoTranscurrido = DateTime.Now - fecha;
            int segundos = (int)tiempoTranscurrido.TotalSeconds;

            if (segundos < 0)
            {
                return "Fecha en el futuro";
            }
            else if (segundos < 60)
            {
                return "Hace menos de un minuto";
            }
            else if (segundos < 120)
            {
                return "Hace un minuto";
            }
            else if (segundos < 2700)
            {
                return $"Hace {Math.Floor(tiempoTranscurrido.TotalMinutes)} minutos";
            }
            else if (segundos < 5400)
            {
                return "Hace una hora";
            }
            else if (segundos < 86400)
            {
                return $"Hace {Math.Floor(tiempoTranscurrido.TotalHours)} horas";
            }
            else if (segundos < 172800)
            {
                return "Ayer";
            }
            else if (segundos < 604800)
            {
                return $"Hace {Math.Floor(tiempoTranscurrido.TotalDays)} días";
            }
            else if (segundos < 1209600)
            {
                return "La semana pasada";
            }
            else
            {
                return $"Hace {Math.Floor(tiempoTranscurrido.TotalDays / 7)} semanas";
            }
        }

    }
}
