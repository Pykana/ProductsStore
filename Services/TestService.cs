using BACKEND_STORE.Interfaces.IRepository;
using BACKEND_STORE.Interfaces.IService;
using BACKEND_STORE.Models.GET;
using BACKEND_STORE.Repositories;

namespace BACKEND_STORE.Services
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

        public async Task<StoreConfig> VerVaribalesDeEntorno()
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


