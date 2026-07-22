using Microsoft.EntityFrameworkCore;
using UzMarket.Core;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.RepositoryLayer.Dtos.UserDtos;
using UzMarket.RepositoryLayer.Entity;
using UzMarket.ServiceLayer.Services.UserServices.QueryObjects;

namespace UzMarket.ServiceLayer.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserListDto>> GetListAsync(UserFilterDto filter)
        {
            var user = await _context.Users.Select(x => new UserListDto
            {
                Id = x.Id,
                UserName = x.UserName,
                FullName = x.FullName,
                ShortName = x.ShortName,
                Email = x.Email,
                Pinfl = x.Pinfl,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                DateOfBirth = x.DateOfBirth,
                PassportSeries = x.PassportSeries,
            }).SortFilter(filter)
            .ToListAsync();

            return user;
        }

        public async Task<UserDto> GetAsync(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user is null)
                throw new Exception($"User was not found : {id}");

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.HashPassword,
                FullName = user.FullName,
                ShortName = user.ShortName,
                Email = user.Email,
                Pinfl = user.Pinfl,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                PassportSeries = user.PassportSeries,
            };
        }

        public async Task<UserDto> CreateAsync(CreateUserDlDto dto, CancellationToken cancellation)
        {
            var user = new User
            {
                UserName = dto.UserName,
                HashPassword = dto.Password,
                FullName = dto.FullName,
                ShortName = dto.ShortName,
                Pinfl = dto.Pinfl,
                Email = dto.Email,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PassportSeries = dto.PassportSeries,
            };

            await _context.Users.AddAsync(user, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return new UserDto
            {
                Id = (int)user.Id,

                UserName = dto.UserName,
                Password = dto.Password,
                FullName = dto.FullName,
                ShortName = dto.ShortName,
                Pinfl = dto.Pinfl,
                Email = dto.Email,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PassportSeries = dto.PassportSeries,
            };
        }

        public async Task<UserDto> UpdateAsync(UpdateUserDlDto dto, CancellationToken cancellation)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
                throw new Exception($"User not found : {dto.Id}");

            if (!string.IsNullOrWhiteSpace(dto.UserName))
                entity.UserName = dto.UserName;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                entity.HashPassword = dto.Password;
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                entity.FullName = dto.FullName;
            if (!string.IsNullOrWhiteSpace(dto.ShortName))
                entity.ShortName = dto.ShortName;
            if (!string.IsNullOrWhiteSpace(dto.Pinfl))
                entity.Pinfl = dto.Pinfl;
            if (!string.IsNullOrWhiteSpace(dto.Email))
                entity.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
                entity.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(dto.Address))
                entity.Address = dto.Address;
            if (!string.IsNullOrWhiteSpace(dto.DateOfBirth))
                entity.DateOfBirth = dto.DateOfBirth;
            if (!string.IsNullOrWhiteSpace(dto.PassportSeries))
                entity.PassportSeries = dto.PassportSeries;

            await _context.SaveChangesAsync(cancellation);

            return new UserDto
            {
                Id = (int)entity.Id,

                UserName = dto.UserName,
                Password = dto.Password,
                FullName = dto.FullName,
                ShortName = dto.ShortName,
                Pinfl = dto.Pinfl,
                Email = dto.Email,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                PassportSeries = dto.PassportSeries,
            };
        }

        public async Task<string> DeleteAsync(long id, CancellationToken cancellation)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellation);

            if (user is null)
                throw new Exception($"User was not found : {id}");

            if (user.StatusId == (int)StatusIdConst.DELETED)
                throw new Exception($"User with ID {id} is already deleted.");

            user.StatusId = (int)StatusIdConst.DELETED;
            await _context.SaveChangesAsync(cancellation);

            return $"User with ID {user.Id} has been deleted successfully.";
        }
    }
}
