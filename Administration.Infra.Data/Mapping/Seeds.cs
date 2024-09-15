using Administration.Domain.Entities.Users;
using System;

namespace Administration.Infra.Data.Mapping
{
    public static class Seeds
    {
        public static class SUser
        {
            public static class Users
            {
                public static ApplicationUser Administrator = new ApplicationUser
                {
                    Id = Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),
                    Name = "UserTest",
                    UserName = "UserTest",
                    NormalizedUserName = "USERTEST",
                    CreatedOnUtc = DateTime.UtcNow,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedById = Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),
                    Password = "Test@123",
                    PasswordHash = "AKy5kPOzNly1NqE9Clp5WUdzisavXuf7OSppJCOGqGsLWfBRyjFcfBglGzz999GCRg==",
                    SecurityStamp= "7f5f31f5-b241-4743-9ba3-8c57952e2c42",
                    Email = "test@gmail.com",
                    NormalizedEmail = "TEST@GMAIL.COM",
                    IsActive = true,
                };

                /// <summary>
                /// This user should be used when no one is logged in.
                /// </summary>
                public static ApplicationUser User = new ApplicationUser
                {
                    Id = Guid.Parse("EC574D54-0798-438A-8F34-829177F11AF5"),
                    Name = "Alban",
                    UserName = "Alban",
                    NormalizedUserName = "ALBAN",
                    CreatedOnUtc = DateTime.UtcNow,
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    CreatedById = Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),
                    Password = "Alban@123",
                    PasswordHash = "AQAAAAEAACcQAAAAEK4A24Guko/d4kb5MkX3w6DmNnoy9XeYaGIxISc+Y4tqweZ7WNibPIKektFAtcdedA==",
                    SecurityStamp= "5FHRMDY5CVLGCFEMWCGDJJAWDGQQUIW3",
                    Email = "albanstambolliu@gmail.com",
                    NormalizedEmail= "ALBANSTAMBOLLIU@GMAIL.COM",
                    IsActive = true,

                };
            }

            public static class Roles
            {
                public static ApplicationRole AdministratorRole = new ApplicationRole
                {
                    Id = Guid.Parse("9CC758FA-0231-417A-9533-1FF8572A712B"),
                    Name= "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    IsActive = true,
                    CreatedById= Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),

                };

                public static ApplicationRole UserRole = new ApplicationRole
                {
                    Id = Guid.Parse("021E4E36-8C88-48DF-A3E7-BBBAC08A012E"),
                    Name = "Employee",
                    NormalizedName= "EMPLOYEE",
                    IsActive = true,
                    CreatedById = Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),

                };
            }

            public static class UserRoles
            {
                public static ApplicationUserRole UserRoleAdministrator = new ApplicationUserRole
                {
                    UserId = Guid.Parse("1DBB51D9-D174-442C-A91B-80E3B4C33421"),
                    RoleId = Guid.Parse("9CC758FA-0231-417A-9533-1FF8572A712B")

                };

                /// <summary>
                /// This user should be used when no one is logged in.
                /// </summary>
                public static ApplicationUserRole UserRoleUser = new ApplicationUserRole
                {
                    UserId = Guid.Parse("EC574D54-0798-438A-8F34-829177F11AF5"),
                    RoleId = Guid.Parse("021E4E36-8C88-48DF-A3E7-BBBAC08A012E")
                };
            }
        }
    }
}
