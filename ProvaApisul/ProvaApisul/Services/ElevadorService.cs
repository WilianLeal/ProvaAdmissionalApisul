using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using ProvaApisul.Classes;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Immutable;
using System.Diagnostics.Metrics;
using System.Text;

namespace ProvaApisul.Services
{
    public class ElevadorService : IElevadorService
    {
        #region contadores
        int countTV = 0;
        int countTN = 0;
        int countTM = 0;
        int countEleA = 0;
        int countEleB = 0;
        int countEleC = 0;
        int countEleD = 0;
        int countEleE = 0;
        #endregion contadores

        public async Task<string> GetValueInt(int tipo, int maiorMenor)
        {
            string result = "";

            var arrayDeserializado = GetListInput();

            #region Listas
            List<int> listAndar = new List<int>();
            List<string> listElevador = new List<string>();
            List<string> listPeriodo = new List<string>();
            #endregion Listas

            #region Arrays
            int[] arrayAndares = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            string[] arrayElevadores = new string[] { "A", "B", "C", "D", "E" };
            string[] arrayTurnos = new string[] { "V", "N", "M" };
            #endregion Arrays

            if (arrayDeserializado != null)
            {
                foreach (var array in arrayDeserializado)
                {
                    listAndar.Add(array.andar);
                    listElevador.Add(array.elevador);
                    listPeriodo.Add(array.turno);
                }
            }

            switch (tipo)
            {
                case 1:
                    string comMenosUso = GetMenorValor(listAndar);
                    string semUso = GetSemUso(listAndar, arrayAndares);
                    if (semUso != null)
                    {
                        result = semUso + ". " + comMenosUso;
                    }
                    else
                    {
                        result = comMenosUso;
                    }
                    break;
                case 2:
                    var resultUsoMaior = GetUsoElevador(listElevador, maiorMenor);
                    var resultTurnMaior = GetUsoTurno(listElevador, maiorMenor);
                    var resFullMaior = resultUsoMaior + resultTurnMaior;
                    result = resFullMaior;
                    break;
                case 3:
                    var resultUsoMenor = GetUsoElevador(listElevador, maiorMenor);
                    var resultTurnMenor = GetUsoTurno(listElevador, maiorMenor);
                    var resFullMenor = resultUsoMenor + resultTurnMenor;
                    result = resFullMenor;
                    break;
                case 4:
                    var resultTurn = GetUsoTurno(listElevador, maiorMenor);
                    result = resultTurn;
                    break;
                case 5:
                    result = GetPorcentagemElevador(listElevador);
                    break;
            }

            return await Task.Run(() => { return result; });
        }

        public string GetSemUso(List<int> b, int[] a)
        {
            b.Sort();
            string valida = "Andares que não foram usados: ";
            for (var i = 0; i < b.Count(); i++)
            {
                if (!a.Contains(b[i]))
                {
                    valida += a[i];
                }
            }

            return valida;
        }

        public List<Input>? GetListInput()
        {
            StreamReader r = new StreamReader("JsonDados/input.json");
            string jsonString = r.ReadToEnd();
            List<Input>? arrayDeserializado = JsonConvert.DeserializeObject<List<Input>>(jsonString);

            return arrayDeserializado;
        }

        public async Task<string> GetAndarMenosUtilizadoPorUsuariosService()
        {
            var result = GetValueInt(1, 3);
            return await result;
        }

        public async Task<string> GetElevadorMaisUtilizadoPeriodoFluxoService()
        {
            var result = GetValueInt(2, 0);
            return await result;
        }

        public async Task<string> GetElevadorMenosUtilizadoPeriodoFluxoService()
        {
            var result = GetValueInt(3, 1);
            return await result;
        }

        public async Task<string> GetPeriodoMaiorFluxoService()
        {
            var result = GetValueInt(4, 2);
            return await result;
        }

        public async Task<string> GetPercentualUsoElevadorService()
        {
            var result = GetValueInt(5, 3);
            return await result;
        }

        public string GetMenorValor(List<int> b)
        {
            b.Sort();
            var m = b.GroupBy(p => p).Where(a => a.Count() <= 1).SelectMany(a => a);
            string andaresMenosUsados = "Andares menos usados: ";

            foreach (var m1 in m)
            {
                if (m1 != m.Last())
                {
                    andaresMenosUsados += m1.ToString() + ", ";
                }
                else
                {
                    andaresMenosUsados += m1.ToString();
                }
            }

            return andaresMenosUsados;
        }

        public string GetUsoTurno(List<string> b, int maiorMenor)
        {
            b.Sort();
            IEnumerable<string>? m;
            string elevadorMaiorUso = "No turno: ";

            if (maiorMenor == 0)
            {
                m = b.GroupBy(p => p).Where(a => a.Count() >= 1).SelectMany(a => a);
            }
            else if (maiorMenor == 1)
            {
                m = b.GroupBy(p => p).Where(a => a.Count() <= 1).SelectMany(a => a);
            }
            else if (maiorMenor == 2)
            {
                m = b.GroupBy(p => p).Where(a => a.Count() >= 1).SelectMany(a => a);
                elevadorMaiorUso = "Turno mais utilizado: ";
            }
            else
            {
                m = null;
                elevadorMaiorUso = "";
            }

            Dictionary<string, int> valida = new Dictionary<string, int>();

            foreach (var i in m)
            {
                switch (i)
                {
                    case "M":
                        countTM++;
                        break;
                    case "N":
                        countTN++;
                        break;
                    case "V":
                        countTV++;
                        break;
                }
            }

            valida.Add("M", countTM);
            valida.Add("N", countTN);
            valida.Add("V", countTV);

            var result = valida.Where(a => a.Value == valida.Select(a => a.Value).Max()).Select(a => a.Key).FirstOrDefault();

            return elevadorMaiorUso + result + ".";
        }

        public string GetUsoElevador(List<string> b, int maiorMenor)
        {
            b.Sort();
            IEnumerable<string>? m;
            string elevadorMaiorUso = "";
            if (maiorMenor == 0)
            {
                m = b.GroupBy(p => p).Where(a => a.Count() >= 1).SelectMany(a => a);
                elevadorMaiorUso = "Elevador com maior uso: ";
            }
            else if (maiorMenor == 1)
            {
                m = b.GroupBy(p => p).Where(a => a.Count() <= 1).SelectMany(a => a);
                elevadorMaiorUso = "Elevador com menor uso: ";
            }
            else
            {
                m = null;
                elevadorMaiorUso = "";
            }
            
            Dictionary<string, int> valida = new Dictionary<string, int>();

            foreach (var i in m)
            {
                switch (i)
                {
                    case "A":
                        countEleA += 1;
                        break;
                    case "B":
                        countEleB += 1;
                        break;
                    case "C":
                        countEleC += 1;
                        break;
                    case "D":
                        countEleD += 1;
                        break;
                    case "E":
                        countEleE += 1;
                        break;
                }
            }

            valida.Add("A", countEleA); 
            valida.Add("B", countEleB); 
            valida.Add("C", countEleC); 
            valida.Add("D", countEleD); 
            valida.Add("E", countEleE);

            var result = valida.Where(a => a.Value == valida.Select(a => a.Value).Max()).Select(a => a.Key).FirstOrDefault();

            return elevadorMaiorUso + result + ". ";
        }

        public string GetPorcentagemElevador(List<string> b)
        {
            b.Sort();
            string elevadorMaiorUso = "Porcentagens de uso: ";

            StringBuilder valida = new StringBuilder();

            foreach (var i in b)
            {
                switch (i)
                {
                    case "A":
                        countEleA += 1;
                        break;
                    case "B":
                        countEleB += 1;
                        break;
                    case "C":
                        countEleC += 1;
                        break;
                    case "D":
                        countEleD += 1;
                        break;
                    case "E":
                        countEleE += 1;
                        break;
                }
            }

            decimal ca = (countEleA * 100) / b.Count();
            decimal cb = (countEleB * 100) / b.Count();
            decimal cc = (countEleC * 100) / b.Count();
            decimal cd = (countEleD * 100) / b.Count();
            decimal ce = (countEleE * 100) / b.Count();

            valida.Append("A: " + (ca).ToString("N") + ", ");
            valida.Append("B: " + (cb).ToString("N") + ", ");
            valida.Append("C: " + (cc).ToString("N") + ", ");
            valida.Append("D: " + (cd).ToString("N") + ", ");
            valida.Append("E: " + (ce).ToString("N") + ", ");
            valida.Append("Total de serviçõs: " + b.Count());

            var result = valida;

            return elevadorMaiorUso + result + ". ";
        }
    }
}
