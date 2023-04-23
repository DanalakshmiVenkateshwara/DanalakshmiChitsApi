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

        public const string GET_USER_PROFILE = @"Select * from USERPROFILE where Id = @UserId";

        public const string Get_All_ChitPlans = @"Select * from ChitPlans";

        public const string Add_ChitPlan = @"Insert into Chit_Groups 
                                            (GroupId,GroupName,Amount,Duration,InstallmentAmount,NoOfMembers,Existed,StartDate,MembersInCircle)
                                           values(@GroupId,@GroupName,@Amount,@Duration,@InstallmentAmount,@NoOfMembers,@Existed,@StartDate,@MembersInCircle)";

        public const string EnrollMent = @"Insert into Enrollments(UserId,GroupId,enrolementDate, groupStatus)value (@UserId,@GroupId,@enrolementDate, @groupStatus)";

        public const string RegisteUsers = @"Insert into RegisteUsers
                                             (Name,Phone,EMail,Password,Address,City,State)
                                             values(Name,Phone,EMail,Password,Address,City,State)";

        public const string AuctionDetailsByGroup = @"Insert into GroupWiseDetails 
                                                     (UserId,GroupId,AuctionDate,AuctionAmount,AuctionToBePaid,NoOfMonthsCompleted,DueDate,Status)
                                                     values(@UserId,@GroupId,@AuctionDate,@AuctionAmount,@AuctionToBePaid,@NoOfMonthsCompleted,@DueDate,Status)";

        public const string UserPayments = @"Insert into UserPayments 
                                          (UserId,GroupId,CurrentMonthEmi,Divident,TotalAmount,DueAmount,AuctionDate,PaymentDate,FullyPiad,PaymentMonth,Raised)
                                          values(@UserId,@GroupId,@CurrentMonthEmi,@Divident,@TotalAmount,@DueAmount,@AuctionDate,@PaymentDate,@FullyPiad,@PaymentMonth,@Raised)";

        public const string Add_Auction_Details = @"Insert into AuctionDetails (UserId,GroupId,AuctionAmount,AuctionDate,Dividend) values (@UserId,@GroupId,@AuctionAmount,@AuctionDate,@Dividend)";
    }
}




