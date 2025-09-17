using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aDealerEDVMS.Service.ToanHH
{
    public interface ISystemUserAccountService
    {
        Task<SystemUserAccount> GetUserAccountAsync(string username, string password); 
    }
    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystemUserAccountService()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<SystemUserAccount> GetUserAccountAsync(string userName, string password)
        {
            return await _unitOfWork.SystemUserAccountRepository.GetUserAccountAsync(userName, password);
        }
    }
}
