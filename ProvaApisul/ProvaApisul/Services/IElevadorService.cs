using ProvaApisul.Classes;

namespace ProvaApisul.Services
{
    public interface IElevadorService
    {
        List<Input>? GetListInput();

        Task<string> GetValueInt(int tipo, int maiorMenor);

        string GetSemUso(List<int> b, int[] a);

        Task<string> GetAndarMenosUtilizadoPorUsuariosService();

        Task<string> GetElevadorMaisUtilizadoPeriodoFluxoService();

        Task<string> GetElevadorMenosUtilizadoPeriodoFluxoService();

        Task<string> GetPeriodoMaiorFluxoService();

        Task<string> GetPercentualUsoElevadorService();

        string GetMenorValor(List<int> b);

        string GetUsoTurno(List<string> b, int maiorMenor);

        string GetUsoElevador(List<string> b, int maiorMenor);

        string GetPorcentagemElevador(List<string> b);
    }
}
