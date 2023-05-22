using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        //Task<int> MemberRegistration(Register register);
        Task<UserProfile> GetUserProfile(int userId);
        Task<List<ChitPlans>> GetAllChitPlans(bool groupClosed, int userId);
        Task<List<AcDetails>> GetUserAcCopy(int userId, int groupId);
        Task<List<ChitPlans>> GetChitsDropDown();
    }
}
