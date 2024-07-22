using AutoMapper;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Authentication;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Services.Common;
using kokshengbi.Application.Users.Common;
using kokshengbi.Domain.UserAggregate;
using MediatR;

namespace kokshengbi.Application.Users.Queries.Login
{
    public class UserLoginQueryHandler :
        IRequestHandler<UserLoginQuery, UserSafetyResult?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserLoginQueryHandler(IUserRepository userRepository, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UserSafetyResult?> Handle(UserLoginQuery query, CancellationToken cancellationToken)
        {
            string userAccount = query.userAccount;
            string userPassword = query.userPassword;

            // 2. check user is exist
            if (await _userRepository.GetUserByUserAccount(userAccount) is not User user)
            {
                throw new BusinessException(ErrorCode.NULL_ERROR, "找不到该用户");
            }

            if (user.isDelete == true)
            {
                throw new BusinessException(ErrorCode.NULL_ERROR, "找不到该用户 用户已被删除");
            }
            if (!EncryptionService.VerifyPassword(user.userPassword, userPassword))
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "账户密码不对");
            }

            // 3. 用户脱敏 desensitization
            UserSafetyResult safetyUser = _mapper.Map<UserSafetyResult>(user);

            // 4. JWT Token
            var token = _jwtTokenGenerator.GenerateToken(user.Id.Value, user.userName, user.userRole);
            // Assign the generated token
            safetyUser = safetyUser with { token = token };

            return safetyUser;
        }
    }
}
