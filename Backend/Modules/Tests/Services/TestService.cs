using BACKEND_STORE.Modules.Tests.Interfaces;
using BACKEND_STORE.Modules.Tests.Models;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Modules.Tests.Services
{
    public class TestService : ITestService
    {
        private readonly ITestRepository _testRepository;
        public TestService(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<Test> ProbarConexion()
        {
            return await _testRepository.ProbarConexion();
        }

        public async Task<VariablesEntorno> VerVaribalesDeEntorno()
        {
            return await _testRepository.VerVaribalesDeEntorno();
        }
        public async Task<string> VerificarEncriptamiento(string contraseña)
        {
            return await _testRepository.VerificarEncriptamiento(contraseña);
        }

        public async Task<string> VerificarLogs(string Mensaje)
        {
            return await _testRepository.VerificarLogs(Mensaje);
        }
      

    }
}


