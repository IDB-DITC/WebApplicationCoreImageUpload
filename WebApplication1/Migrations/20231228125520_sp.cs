using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class sp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string getSpEmployee = @"
create proc SpGetEmployee

@EmployeeId int = 0

as


Select *
from [dbo].[tblEmployee]
where EmployeeId = @EmployeeId or @EmployeeId = 0


";
            migrationBuilder.Sql(getSpEmployee);


            string creatSpEmployee = @"
create proc SpCreateEmployee

@EmployeeName nvarchar(max),
@ImagePath nvarchar(max)



as


INSERT INTO [dbo].[tblEmployee]
           ([EmployeeName]
           ,[ImagePath])
     VALUES
           (@EmployeeName,@ImagePath)

 return @@Rowcount;

";


            migrationBuilder.Sql(creatSpEmployee);




            string updateEmp = @"create proc SpUpdateEmployee

@EmployeeId int,
@EmployeeName nvarchar(max),
@ImagePath nvarchar(max)



as

UPDATE [dbo].[tblEmployee]
   SET [EmployeeName] = @EmployeeName
      ,[ImagePath] = @ImagePath
 WHERE EmployeeId = @EmployeeId

 return @@Rowcount;

";



            migrationBuilder.Sql(updateEmp);

            string deleteEmp = @"create proc SpDeleteEmployee

@EmployeeId int


as

delete from [dbo].[tblEmployee] 
 WHERE EmployeeId = @EmployeeId
 return @@Rowcount;
";



            migrationBuilder.Sql(deleteEmp);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc SpCreateEmployee");
            migrationBuilder.Sql("drop proc SpUpdateEmployee");
            migrationBuilder.Sql("drop proc SpDeleteEmployee"); 
            migrationBuilder.Sql("drop proc SpGetEmployee"); 
        }
    }
}
