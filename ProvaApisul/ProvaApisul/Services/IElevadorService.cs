using ProvaApisul.Classes;

namespace ProvaApisul.Services
{
    public interface IElevadorService
    {
        List<Input>? GetListInput();

        Task<string> GetValueInt(int tipo, int maiorMenor);

        Task<string> GetAndarMenosUtilizadoPorUsuariosService();

        Task<string> GetElevadorMaisUtilizadoPeriodoFluxoService();

        Task<string> GetElevadorMenosUtilizadoPeriodoFluxoService();

        Task<string> GetPeriodoMaiorFluxoService();

        Task<string> GetPercentualUsoElevadorService();
    }
}
