using System;
using System.Threading.Tasks;
using Asakabank.Base;
using Asakabank.UserApi.Base;
using Asakabank.UserApi.Entities;
using Asakabank.UserApi.Models;
using Asakabank.UserApi.ServiceModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Asakabank.UserApi.Services {
    public class UserService : IUserService {
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public UserService(IDbRepository dbRepository, IMapper mapper) {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<UserCreateResponse> Create(UserCreate user) {
            var rnd = new Random();
            var entity = await _dbRepository.Get<DbUser>().FirstOrDefaultAsync(x => x.PhoneNumber == user.PhoneNumber);
            Guid userId;
            if (entity == null) {
                entity = new DbUser(Guid.NewGuid()) {
                    PhoneNumber = user.PhoneNumber,
                    OTP = rnd.Next(10000, 99999).ToString(),
                    OTPSentTime = DateTime.Now
                };
                userId = await _dbRepository.Add(entity);
            }
            else {
                entity.OTP = rnd.Next(10000, 99999).ToString();
                entity.OTPSentTime = DateTime.Now;
                userId = entity.Id;
                await _dbRepository.Update(entity);
            }

            await _dbRepository.SaveChangesAsync();

            return new UserCreateResponse {
                Code = 0,
                Message = $"СМС код ({entity.OTP}) отправлен на номер {user.PhoneNumber}",
                UserId = userId
            };
        }

        public async Task<UserConfirmResponse> Confirm(UserConfirm user) {
            var entity = await _dbRepository.Get<DbUser>().FirstOrDefaultAsync(x => x.Id == user.UserId);
            if (entity == null)
                return new UserConfirmResponse {
                    Code = -101,
                    Message = "Object not found"
                };
            if (Math.Abs((DateTime.Now - entity.OTPSentTime).TotalSeconds) > 30)
                return new UserConfirmResponse {
                    Code = -50,
                    Message = "Истекло время ожидания подтверждения СМС кода"
                };
            if (!entity.OTP.Equals(user.OTP))
                return new UserConfirmResponse {
                    Code = -51,
                    Message = "Введен неверный код из СМС"
                };
            entity.Password = user.Password;
            await _dbRepository.Update(entity);
            await _dbRepository.SaveChangesAsync();
            return new UserConfirmResponse {
                Code = 0,
                Message = "Успешно"
            };
        }

        public async Task<UserAuthResponse> Auth(UserAuth user) {
            var entity = await _dbRepository.Get<DbUser>().FirstOrDefaultAsync(x => x.Id == user.UserId);
            if (entity == null)
                return new UserAuthResponse {
                    Code = -101,
                    Message = "Object not found"
                };

            if (!entity.Password.Equals(user.Password))
                return new UserAuthResponse {
                    Code = -52,
                    Message = "Введен неверный пароль"
                };
            var model = _mapper.Map<User>(entity);
            return new UserAuthResponse {
                Code = 0,
                Message = "Успешно",
                User = model
            };
        }
    }
}