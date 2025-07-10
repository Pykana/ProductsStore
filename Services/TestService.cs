using BACKEND_STORE.Interfaces;
using BACKEND_STORE.Models.DB;
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
    }
}


