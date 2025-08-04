// Yer: JiraProject.Business/Concrete/UserManager.cs
using JiraProject.Business.Abstract;
using JiraProject.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateUserAsync(User user)
        {
            // İleride burada şifre hash'leme, email kontrolü gibi işlemler yapılacak.
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Remove(user);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            var userFromDb = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (userFromDb != null)
            {
                userFromDb.Username = user.Username;
                userFromDb.Email = user.Email;
                // Gerçek bir uygulamada şifre bu metotla güncellenmez.
                // Eğer yeni şifre geldiyse hash'lenerek güncellenir.
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    userFromDb.PasswordHash = user.PasswordHash;
                }
                userFromDb.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.CompleteAsync();
            }
        }
    }
}