using BACKEND_STORE.Interfaces.IRepository.Version0;
using BACKEND_STORE.Interfaces.IService.Version0;
using BACKEND_STORE.Models;
using static BACKEND_STORE.Config.EnvironmentVariableConfig;

namespace BACKEND_STORE.Services.Version0
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


