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


        //we need to change edit scope is for email and phone only
        public const string UpdateUsers_ById = @"Update UserRegistration set Name= @Name,Phone=@Phone,EMail=@EMail,Aadhar=@Aadhar,Password=@Password,Address=@Address,City=@City,State=@State where Id= @Id";


        public const string DeleteUsers_ById = @"Update UserRegistration set IsActive = 0 where id = @userId";
        //public const string Get_Users_By_Id = @"Select Name ,phone,Address from  UserRegistration where id = @userId";

        public const string Get_Users_By_Id = @"Select * from UserRegistration where id = @userId";

        //public const string Get_All_Users = @"Select Name,Phone,Email,Password, CONCAT(Address,', ', City, ', ',state)[Address] from  UserRegistration";
        public const string Get_All_Users = @"Select * from  UserRegistration where IsActive =@isActive";

        public const string GET_USER_PROFILE = @"Select * from USERPROFILE where Id = @UserId";

        public const string Get_User_Ac_Copy = @"select groupId,UserId, paymentDate, Totalamount, dueamount, paymentMonth from UserPayments where groupId = @groupId and userId = @userId";

        public const string Get_User_Pending_Payments = @"Select groupId,UserId,InstallMentAmount[DueAmount], NoOfMonthsCompleted[PaymentMonth] from GroupWiseDetails 
                                                          Where GroupId = @groupId AND NoOfMonthsCompleted not in 
                                                          (Select PaymentMonth From UserPayments Where GroupId = @groupId AND USerID = @userId)";

        public const string Delete_EnrollMent = @"Update Enrollments set isActive = @isActive , closeDate = GetDate() where userId = @userId and groupId = @groupId";

        public const string EnrollMent = @"Insert into Enrollments (UserId,GroupId,enrollmentDate) values (@UserId,@GroupId,@enrollmentDate)";

        public const string Get_EnrollMents_By_UserId = @"select Date [NextAuctionDate],c.Id[GroupId], g.status[UserChitSatus], g.NoOfMonthsCompleted[PaidUpto],c.Duration[TotalInstallMents] ,C.Duration,  C.groupName,C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId
														   inner join GroupWiseDetails g on g.userId = u.Id
                                                           where u.Id = @UserId";

        public const string Get_EnrollMents_By_GroupId = @"select u.name[UserName], E.EnrollmentDate, C.groupName,C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId
                                                           where c.ID = @groupId";

        public const string Get_All_EnrollMents = @"select u.Id[UserId],u.name[UserName],E.IsActive, E.EnrollmentDate,E.CloseDate, C.groupName, C.Id[GroupId],C.amount from Enrollments E
                                                           inner join ChitGroups C on c.Id= e.GroupId
                                                           inner join UserRegistration u on u.id =e.UserId where E.isActive = @isActive";


        public const string Get_AuctionDetails_By_GroupId = @" select top 1   u.Id[UserId],c.InstallmentAmount[TotalAmount],u.name[UserName],G.NoOfMonthsCompleted[PaidUpTo],
                                                              G.dividend/c.NoOfMembers[Dividend],C.groupName, C.Id[GroupId],C.amount from GroupWiseDetails G 
															  inner join ChitGroups C on c.Id= g.GroupId
                                                           inner join UserRegistration u on u.id =G.UserId where g.groupId =4 order by g.id desc";

        public const string Get_AuctionDetails = @"select u.Id[UserId], c.InstallmentAmount[TotalAmount], u.name[UserName], G.NoOfMonthsCompleted[PaidUpTo],
                                                            G.dividend/c.NoOfMembers[Dividend], C.groupName, C.Id[GroupId], C.amount from GroupWiseDetails G
                                                            inner join ChitGroups C on c.Id= g.GroupId
                                                             inner join UserRegistration u on u.id = G.UserId"
;
        //public const string Get_AuctionDetails_By_GroupId = @"select u.Id[UserId],c.InstallmentAmount[TotalAmount],u.name[UserName],G.NoOfMonthsCompleted[PaidUpTo],
        //                                                      G.dividend/c.NoOfMembers[Dividend],C.groupName, C.Id[GroupId],C.amount from Enrollments E
        //                                                   inner join ChitGroups C on c.Id= e.GroupId
        //                                                   inner join UserRegistration u on u.id =e.UserId
        //						   inner join GroupWiseDetails G on G.groupId =e.GroupId where e.GroupId = @GroupId";
        public const string Get_All_ChitPlans = @"Select * from ChitGroups";

        public const string Get_All_ChitPlans_By_Group = @"Select * from ChitGroups where groupClosed = @groupClosed";
        public const string Get_Upcoming_ChitPlans = @"select * from ChitGroups where existed = 0 and groupclosed =0 and isDelete =0";

        public const string Add_ChitPlan = @"Insert into ChitGroups 
                                            (GroupName,Amount,Duration,InstallmentAmount,NoOfMembers,Existed,StartDate,MembersInCircle)
                                           values(@GroupName,@Amount,@Duration,@InstallmentAmount,@NoOfMembers,@Existed,@StartDate,@MembersInCircle)";

        public const string Update_ChitPlan = @"update ChitGroups set Existed = @Existed, StartDate = @StartDate where Id = @GroupID";

        public const string Closed_ChitPlan = @"update ChitGroups set GroupClosed = @GroupClosed, IsDelete = @IsDelete, EndDate = @EndDate where Id = @GroupID";

        public const string AuctionDetailsByGroup = @"Insert into GroupWiseDetails 
                                                     (UserId,GroupId,AuctionDate,AuctionAmount,AuctionToBePaid,NoOfMonthsCompleted,DueDate,Status)
                                                     values(@UserId,@GroupId,@AuctionDate,@AuctionAmount,@AuctionToBePaid,@NoOfMonthsCompleted,@DueDate,Status)";

        public const string UserPayments = @"Insert into UserPayments 
                                          (UserId,GroupId,CurrentMonthEmi,Dividend,TotalAmount,DueAmount,AuctionDate,PaymentDate,FullyPaid,PaymentMonth,Raised)
                                          values(@UserId,@GroupId,@CurrentMonthEmi,@Dividend,@TotalAmount,@DueAmount,@AuctionDate,@PaymentDate,@FullyPaid,@PaymentMonth,@Raised)";
        
        public const string Get_UserOutStandings_By_GroupId = @"select UR.Name[Username],C.GroupName,u.UserId,u.GroupId,u.CurrentMonthEmi,u.Dividend,u.TotalAmount,u.DueAmount,u.AuctionDAte,u.PaymentDate,u.PaymentMonth,U.Raised from UserPayments u
                                                                  inner join chitGroups c on c.id=u.groupId
                                                                  inner join userRegistration UR on UR.id=u.userId where groupid=@groupid";

        public const string Get_All_UserOutStandings = @"select UR.Name[Username],C.GroupName,u.UserId,u.GroupId,u.CurrentMonthEmi,u.Dividend,u.TotalAmount,u.DueAmount,u.AuctionDAte,u.PaymentDate,u.PaymentMonth,U.Raised from UserPayments u
                                                          inner join chitGroups c on c.id=u.groupId
                                                          inner join userRegistration UR on UR.id=u.userId";


            //--select UR.Name[Username],C.GroupName,  * from UserPayments u
            //                                              --inner join chitGroups c on c.id=u.groupId
            //                                             -- inner join userRegistration UR on UR.id=u.userId";

        public const string Add_Auction_Details = @"Insert into AuctionDetails (UserId,GroupId,AuctionAmount,AuctionDate,Dividend) values (@UserId,@GroupId,@AuctionAmount,@AuctionDate,@Dividend)";
    }
}




