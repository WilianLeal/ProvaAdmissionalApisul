using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using ProvaApisul.Services;

namespace ProvaApisul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IElevadorService _services;

        public HomeController(IElevadorService services)
        {
            _services = services;
        }

        [HttpPost]
        [Route("menosUsadoUsuario")]
        public async Task<string> GetAndarMenosUtilizadoPorUsuarios()
        {
            return await _services.GetAndarMenosUtilizadoPorUsuariosService();
        }

        [HttpPost]
        [Route("maisUsadoFluxo")]
        public async Task<string> GetElevadorMaisUtilizadoPeriodoFluxo()
        {
            return await _services.GetElevadorMaisUtilizadoPeriodoFluxoService();
        }

        [HttpPost]
        [Route("menosUsadoFluxo")]
        public async Task<string> GetElevadorMenosUtilizadoPeriodoFluxo()
        {
            return await _services.GetElevadorMenosUtilizadoPeriodoFluxoService();
        }

        [HttpPost]
        [Route("maiorPeriodoFluxo")]
        public async Task<string> GetPeriodoMaiorFluxo()
        {
            return await _services.GetPeriodoMaiorFluxoService();
        }

        [HttpPost]
        [Route("percentualUsoFluxo")]
        public async Task<string> GetPercentualUsoElevador()
        {
            return await _services.GetPercentualUsoElevadorService();
        }
    }
}
