using Administration.Application.Extensions;
using Administration.Application.Interfaces;
using Administration.Application.Interfaces.UserInterface;
using Administration.Application.Results;
using Administration.Application.ViewModels;
using Administration.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Application.Services.Users
{
    /// <summary>
    /// Contains methods used to manipulate user data.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IHttpContextAccessor _httpContext;
        private readonly IUploadService _uploadService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userManager">.</param>
        /// <param name="roleManager">.</param>
        /// <param name="uploadService">.</param>
        /// <param name="httpContext">The httpContext<see cref="IHttpContextAccessor"/>.</param>
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IHttpContextAccessor httpContext, IUploadService uploadService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContext = httpContext;
            _uploadService = uploadService;
        }

        /// <summary>
        /// Get all users according to filters
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<HashSet<UserViewModel>> GetUsers(string username, string Name, string email, string role, bool? isActive)
        {
            HashSet<UserViewModel> model = new HashSet<UserViewModel>();
            if (role != null)
            {
                var users = await _userManager.GetUsersInRoleAsync(role);
                var existingRole = await _roleManager.FindByNameAsync(role);
                var list = users
                .Where(x => x.IsActive)
                .If(username != null, y => y.Where(x => x.UserName.ToLower().Contains(Name.ToLower())))
                .If(Name != null, y => y.Where(x => x.Name.ToLower().Contains(username.ToLower())))
                .If(email != null, y => y.Where(x => x.Email.ToLower().Contains(email.ToLower())))
                .If(isActive != null, y => y.Where(x => x.IsActive == isActive));
                foreach (var user in users)
                {
                    UserViewModel userModel = new UserViewModel();
                    userModel.Id = user.Id;
                    userModel.UserName = user.UserName;
                    userModel.Name = user.Name;
                    userModel.IsActive = user.IsActive;
                    userModel.Email = user.Email;
                    userModel.CreatedOnUtc = user.CreatedOnUtc;
                    userModel.Roleid = existingRole?.Id;
                    userModel.Role = existingRole?.Name;
                    userModel.CreatedById = user.CreatedById;
                    userModel.UpdatedById = user.UpdatedById;
                    userModel.CreatedBy = _userManager.Users.First(x => x.Id == x.CreatedById).Name;
                    if (user.UpdatedById != null)
                    {
                        var updatedBy = user.UpdatedById.GetValueOrDefault();
                        userModel.UpdatedBy = _userManager.Users.Where(x => x.Id == updatedBy).FirstOrDefault().Name;
                    }
                    model.Add(userModel);
                }
            }
            else
            {
                var users = _userManager.Users
               .Where(x => x.IsActive)
               .If(username != null, y => y.Where(x => x.Name.ToLower().Contains(username.ToLower())))
               .If(Name != null, y => y.Where(x => x.Name.ToLower().Contains(Name.ToLower())))
               .If(email != null, y => y.Where(x => x.Email.ToLower().Contains(email.ToLower())))
               .If(isActive != null, y => y.Where(x => x.IsActive == isActive));

                foreach (var user in users)
                {
                    UserViewModel userModel = new UserViewModel();
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var roleData = userRoles.FirstOrDefault() != null ? await _roleManager.FindByNameAsync(userRoles.FirstOrDefault()) : null;
                    userModel.Id = user.Id;
                    userModel.UserName = user.UserName;
                    userModel.Name = user.Name;
                    userModel.IsActive = user.IsActive;
                    userModel.Email = user.Email;
                    userModel.CreatedOnUtc = user.CreatedOnUtc;
                    userModel.Roleid = roleData != null ? roleData.Id : null;
                    userModel.Role = roleData != null ? roleData.Name : null;
                    userModel.CreatedById = user.CreatedById;
                    userModel.UpdatedById = user.UpdatedById;
                    userModel.CreatedBy = _userManager.Users.First(x => x.Id == x.CreatedById).Name;
                    if (user.UpdatedById != null)
                    {
                        var updatedBy = user.UpdatedById.GetValueOrDefault();
                        userModel.UpdatedBy = _userManager.Users.Where(x => x.Id == updatedBy).FirstOrDefault().Name;
                    }
                    model.Add(userModel);
                }
            }
            return model;
        }

        /// <summary>
        /// Get all roles from the database.
        /// </summary>
        /// <returns>.</returns>
        public IQueryable<RoleViewModel> GetRoles()
        {
            var roles = _roleManager.Roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
            });

            return roles;
        }

        /// <summary>
        /// Get logged-in user details.
        /// </summary>
        /// <returns>.</returns>
        public async Task<UserViewModel> GetCurrentUserDetailsAsync()
        {
            var email = string.Empty;
            if (_httpContext.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                email = identity.FindFirst(ClaimTypes.Name).Value;
                var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == email);

                var currentUserDetails = new UserViewModel
                {
                    Id = currentUser.Id,
                    UserName = currentUser.UserName,
                    Name = currentUser.Name,
                    Email = currentUser.Email,
                    CreatedOnUtc = currentUser.CreatedOnUtc,
                    IsActive = currentUser.IsActive,
                    Role = await GetUserRoleAsync(currentUser),

                };
                return currentUserDetails;
            }
            else
                throw new ArgumentNullException("No users are logged in");
        }

        /// <summary>
        /// Get the specified user's role.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>Role name .</returns>
        public async Task<string> GetUserRoleAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id">.</param>
        /// <returns>User View Model.</returns>
        public UserViewModel GetUserById(Guid id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new ArgumentNullException("Përdoruesi nuk u gjet");
            }
            else
            {
                var userDetails = new UserViewModel(user);
                return userDetails;
            }
        }

        /// <summary>
        /// The GetUserByIdAsync.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/>.</param>
        /// <returns>The <see cref="Task{UserViewModel}"/>.</returns>
        public async Task<UserViewModel> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new ArgumentNullException("Përdoruesi nuk u gjet");
            }
            else
            {
                var userDetails = new UserViewModel(user);
                return userDetails;
            }
        }

        /// <summary>
        /// Get logged-in user id.
        /// </summary>
        /// <returns>.</returns>
        public Guid GetCurrentUserId()
        {
            var email = string.Empty;
            if (_httpContext.HttpContext.User.Identity is ClaimsIdentity identity)
            {
                email = identity.FindFirst(ClaimTypes.Name).Value;
                var currentUser = _userManager.Users.FirstOrDefault(x => x.UserName == email);
                return currentUser.Id;
            }
            else
                throw new ArgumentNullException("No users are logged in");
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>Result.</returns>
        public async Task<Result> UpdateUser(UserViewModel user)
        {
            var model = _userManager.Users.FirstOrDefault(x => x.Id == user.Id);
            if (model == null)
            {
                return Result.Fail("Përdoruesi nuk u gjet");
            }
            model.UserName = user.UserName;
            model.Name = user.Name;
            model.UserName = user.UserName;
            model.Email = user.Email;
            model.IsActive = user.IsActive;
            model.UpdatedById = GetCurrentUserId();
            model.UpdatedOnUtc = DateTime.UtcNow;

            if (user.Password != null)
            {
                model.PasswordHash = _userManager.PasswordHasher.HashPassword(model, user.Password);
            }

            await _userManager.UpdateAsync(model);

            if (user.Roleid != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(model);

                if (currentRoles != null)
                {
                    await _userManager.RemoveFromRolesAsync(model, currentRoles);
                }
                var addToRole = _roleManager.Roles.FirstOrDefault(x => x.Id == user.Roleid /*&& !x.IsDeleted*/);
                if (addToRole != null)
                {
                    await _userManager.AddToRoleAsync(model, addToRole.Name);
                }
            }

            return Result.Ok();
        }

        /// <summary>
        /// Soft delete an existing user.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>Fail if the user cannot be found and OK if the method is completed successfully.</returns>
        public async Task<Result> DeleteUser(Guid Id)
        {
            //var model = _userManager.Users.FirstOrDefault(x => x.Id == Id);
            //if (model == null)
            //{
            //    return Result.Fail("Perdoruesi nuk u gjet");
            //}
            //await _userManager.DeleteAsync(model);
            //return Result.Ok();

            try
            {
                //soft deleted
                var user = await _userManager.FindByIdAsync(Id.ToString());

                if (user == null)
                    return Result.Fail("User with this Id was not found");
                user.IsDeleted = true;

                await _userManager.UpdateAsync(user);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to delete this Project");
            }
            return Result.Ok();
        }


        public async Task<Result> UploadProfileImage(IFormFile file)
        {
            if (file == null)
                return Result.Fail("Empty model");


            using (MemoryStream ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var filePath = await _uploadService.SaveFile(ms.GetBuffer(), file.FileName);
            }


            //try
            //{
            //    await _userManager.InsertAsync();  
            //}
            //catch
            //{
            //    return Result.Fail("An error occurred while trying to add this ticket. Maybe it already exists.");
            //}
            return Result.Ok();

        }

        public Task<Result> ChangePicture(ChangePictureViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
