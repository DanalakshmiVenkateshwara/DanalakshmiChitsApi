using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static partial class SqlQueries
    {
        public const string Member_Registration = @"INSERT INTO MemberRegistration(Name,Phone,State)
                                                        VALUES(@Name,@Phone,@State)";

        public const string GET_ADMIN_PROFILE = @"SELECT ADDRESS, LANDLINENO,MOBILENO FROM ADMINPROFILE";

        public const string Add_APP_USER = @"Insert into ApplicationUsers (Name,PHONE,State,Date)values(@Name,@PHONE,@State,@Date)";



       
        public const string GET_APP_USERS = @"SELECT Name, Phone,State, Date FROM ApplicationUsers";

        public const string RegisteUsers = @"Insert into UserRegistration
                                             (Name,Phone,EMail,Aadhar,Password,Address,City,State,Date)
                                             values(@Name,@Phone,@EMail,@Aadhar,@Password,@Address,@City,@State,@Date)";

        //public const string Get_Users_By_Id = @"Select Name ,phone,Address from  UserRegistration where id = @userId";

        public const string Get_Users_By_Id = @"Select * from UserRegistration where id = @userId";

        //public const string Get_All_Users = @"Select Name,Phone,Email,Password, CONCAT(Address,', ', City, ', ',state)[Address] from  UserRegistration";
        public const string Get_All_Users = @"Select * from  UserRegistration";

        public const string GET_USER_PROFILE = @"Select * from USERPROFILE where Id = @UserId";

        public const string Get_User_Ac_Copy = @"select paymentDate, Totalamount, dueamount, paymentMonth from UserPayments where groupId = @groupId and userId = @userId";

        public const string EnrollMent = @"Insert into Enrollments (UserId,GroupId,enrollmentDate) values (@UserId,@GroupId,@enrollmentDate)";

        public const string Get_EnrollMents_By_UserId_GroupId = @"select Date [NextAuctionDate], '4/20'[PaidUpto] ,  C.groupName,C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId
                                                           where c.ID = @groupId and u.Id = @UserId";

        public const string Get_EnrollMents_By_GroupId = @"select u.name[UserName], E.EnrollmentDate, C.groupName,C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId
                                                           where c.ID = @groupId";

        public const string Get_All_EnrollMents = @"select u.name[UserName], E.EnrollmentDate, C.groupName,C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId";

        public const string Get_All_ChitPlans = @"Select * from ChitGroups";

        public const string Get_All_ChitPlans_By_Group = @"Select * from ChitGroups where groupClosed = @groupClosed";

        public const string Add_ChitPlan = @"Insert into ChitGroups 
                                            (GroupName,Amount,Duration,InstallmentAmount,NoOfMembers,Existed,StartDate,MembersInCircle)
                                           values(@GroupName,@Amount,@Duration,@InstallmentAmount,@NoOfMembers,@Existed,@StartDate,@MembersInCircle)";

        public const string Update_ChitPlan = @"update ChitGroups set Existed = @Existed, StartDate = @StartDate where Id = @GroupID";

        public const string Closed_ChitPlan = @"update ChitGroups set GroupClosed = @GroupClosed, StartDate = @StartDate where Id = @GroupID";

        public const string AuctionDetailsByGroup = @"Insert into GroupWiseDetails 
                                                     (UserId,GroupId,AuctionDate,AuctionAmount,AuctionToBePaid,NoOfMonthsCompleted,DueDate,Status)
                                                     values(@UserId,@GroupId,@AuctionDate,@AuctionAmount,@AuctionToBePaid,@NoOfMonthsCompleted,@DueDate,Status)";

        public const string UserPayments = @"Insert into UserPayments 
                                          (UserId,GroupId,CurrentMonthEmi,Divident,TotalAmount,DueAmount,AuctionDate,PaymentDate,FullyPaid,PaymentMonth,Raised)
                                          values(@UserId,@GroupId,@CurrentMonthEmi,@Divident,@TotalAmount,@DueAmount,@AuctionDate,@PaymentDate,@FullyPaid,@PaymentMonth,@Raised)";
        
        public const string Get_UserOutStandings_By_GroupId = @"Select * from userpayments where groupid=@groupid";

        public const string Get_All_UserOutStandings = @"Select * from userpayments";

        public const string Add_Auction_Details = @"Insert into AuctionDetails (UserId,GroupId,AuctionAmount,AuctionDate,Dividend) values (@UserId,@GroupId,@AuctionAmount,@AuctionDate,@Dividend)";
    }
}




