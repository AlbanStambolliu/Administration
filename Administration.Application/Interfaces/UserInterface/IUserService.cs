using Administration.Application.Results;
using Administration.Application.ViewModels;
using Administration.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Administration.Application.Interfaces.UserInterface
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users according to filters
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<HashSet<UserViewModel>> GetUsers(string firstName, string lastName, string email, string role, bool? isActive);

        /// <summary>
        /// Get all roles.
        /// </summary>
        /// <returns>.</returns>
        IQueryable<RoleViewModel> GetRoles();

        /// <summary>
        /// Get user role.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>.</returns>
        Task<string> GetUserRoleAsync(ApplicationUser user);

        /// <summary>
        /// Get logged in user details.
        /// </summary>
        /// <returns>.</returns>
        Task<UserViewModel> GetCurrentUserDetailsAsync();

        /// <summary>
        /// Get user details by id.
        /// </summary>
        /// <param name="id">.</param>
        /// <returns>User View Model.</returns>
        UserViewModel GetUserById(Guid id);

        Task<UserViewModel> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Get current user id.
        /// </summary>
        /// <returns>.</returns>
        Guid GetCurrentUserId();

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>.</returns>
        Task<Result> UpdateUser(UserViewModel user);

        /// <summary>
        /// Delete an existing user.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>Fail if the user cannot be found and Ok if the method is completed successfully.</returns>
        Task<Result> DeleteUser(Guid Id);
        /// <summary>
        /// Change user picture
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Result> ChangePicture(ChangePictureViewModel model);

        /// <summary>
        /// Upload profile image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<Result>UploadProfileImage(IFormFile file);
    }
}
