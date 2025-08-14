using AutoMapper;
using JiraProject.Business.Abstract;
using JiraProject.Business.Dtos;
using JiraProject.Business.Exceptions;
using JiraProject.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JiraProject.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserManager(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            var existingUser = (await _userRepository.FindAsync(u => u.Email.ToLower() == dto.Email.ToLower())).FirstOrDefault();
            if (existingUser != null)
            {
                throw new ConflictException("Bu e-posta adresi zaten kullanılıyor.");
            }

            var userEntity = _mapper.Map<User>(dto);
            userEntity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            userEntity.Role = UserRole.BusinessUser;

            await _userRepository.AddAsync(userEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<UserDto>(userEntity);
        }

        public async Task<UserDto?> LoginAsync(string email, string password)
        {
            var user = (await _userRepository.FindAsync(u => u.Email.ToLower() == email.ToLower())).FirstOrDefault();
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException($"'{id}' ID'li kullanıcı bulunamadı.");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var userFromDb = await _userRepository.GetByIdAsync(id);
            if (userFromDb == null) throw new NotFoundException("Güncellenecek kullanıcı bulunamadı.");

            _mapper.Map(dto, userFromDb);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<UserDto>(userFromDb);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("Silinecek kullanıcı bulunamadı.");

            _userRepository.Remove(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}