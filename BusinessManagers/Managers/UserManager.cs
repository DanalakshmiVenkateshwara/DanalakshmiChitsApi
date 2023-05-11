using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessManagers.Interfaces;
using BusinessObjects;
using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;

namespace DataAccess.Repositories.Managers
{
    public  class UserManager : IUserManager
    {
        private IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        //public async Task<int> MemberRegistration(Register register)
        //{
        //    return await _userRepository.MemberRegistration(register);
        //}
        public async Task<UserProfile> GetUserProfile(int userId)
        {
            return await _userRepository.GetUserProfile(userId);
        }
        public async Task<List<ChitPlans>> GetAllChitPlans(bool groupClosed, int userId)
        {
            return await _userRepository.GetAllChitPlans(groupClosed, userId);
        }
        public async Task<List<AcDetails>> GetUserAcCopy(int userId, int groupId)
        {
            return await _userRepository.GetUserAcCopy(userId, groupId);
        }
    }
}
