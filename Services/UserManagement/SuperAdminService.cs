using AutoMapper;
using MicroFinance.Dtos;
using MicroFinance.Dtos.UserManagement;
using MicroFinance.Exceptions;
using MicroFinance.Models.UserManagement;
using MicroFinance.Repository.UserManagement;
using MicroFinance.Token;

namespace MicroFinance.Services.UserManagement
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly ILogger<SuperAdminService> _logger;
        private readonly ISuperAdminRepository _superAdminRepo;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IEmployeeRepository _employeeRepository;

        public SuperAdminService
        (
            ILogger<SuperAdminService> logger,
            ISuperAdminRepository superAdminRepo,
            IEmployeeRepository employeeRepository,
            IEmployeeService employeeService,
            IMapper mapper,
            ITokenService tokenService
        )
        {
            _logger = logger;
            _superAdminRepo = superAdminRepo;
            _employeeService = employeeService;
            _mapper = mapper;
            _tokenService = tokenService;
            _employeeRepository = employeeRepository;
        }


        public async Task<TokenResponseDto> LoginService(SuperAdminLoginDto superAdminLoginDto)
        {
            var superadmin = await _superAdminRepo.GetSuperAdminByUserName(superAdminLoginDto.UserName);
            if(superadmin!=null){
                var result = await _superAdminRepo.Login(superadmin, superAdminLoginDto.Password);
                if(result.Succeeded)
                {
                    _logger.LogInformation($"{DateTime.Now}: SuperAdmin Logged In > {superAdminLoginDto.UserName}");
                    var tokenDto = new TokenDto()
                    {
                        UserName=superadmin.UserName,
                        UserId = superadmin.Id,
                        Role=superadmin.Role.Name,
                        IsActive = superadmin.IsActive.ToString(),
                        Email=superadmin.Email,
                        BranchCode="-100"
                    };
                    var token = _tokenService.CreateToken(tokenDto);
                    return new TokenResponseDto(){Token=token};
                }
                _logger.LogError($"{DateTime.Now}: Unable to authenticate {superAdminLoginDto.UserName} due to {result.ToString()}");

            }
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }

        public async Task<SuperAdminDto> GetUserByIdService(string id)
        {
           var user = await _superAdminRepo.GetSuperAdminById(id);
            if (user == null)
            {
                return new SuperAdminDto()
                {
                    Message = $"No Data found for User id: {id}"
                };
            }
            var superAdminDto = new SuperAdminDto()
            {
                Message="Success",
                UserName=user.UserName,
                Role=user.Role.Name,
                IsActive=user.IsActive
            };
            return superAdminDto;
        }

        public async Task<ResponseDto> UpdatePasswordService(SuperAdminUpdatePasswordDto superAdminUpdatePasswordDto, string userName)
        {
            var superAdmin = await _superAdminRepo.GetSuperAdminByUserName(userName);
            if(superAdmin!=null)
            {
                bool passwordUpdateStatus = await _superAdminRepo.UpdatePassword(superAdmin, superAdminUpdatePasswordDto.OldPassword, superAdminUpdatePasswordDto.NewPassword);
                if(passwordUpdateStatus)
                    return new ResponseDto()
                    {
                        Message="Password Update Successfull",
                        StatusCode="200",
                        Status=true
                    };
                _logger.LogError($"{DateTime.Now}: Failed to update password of super admin > {userName}");
                throw new BadRequestExceptionHandler($"Failed to update the password");
                
            }
            _logger.LogError($"{DateTime.Now}: Attempting to change password of unknown user in super admin database > {userName}");
            throw new UnAuthorizedExceptionHandler("UnAuthorized");
        }


        // Handle MicroFinance
        public async Task<ResponseDto> CreateAdminService(CreateAdminBySuperAdminDto createAdminBySuperAdminDto, string createdBy)
        {
            var employeeExist = await _employeeRepository.GetEmployeeByEmail(createAdminBySuperAdminDto.Email);
            if(employeeExist!=null)
                throw new BadRequestExceptionHandler("Employee Already Exist for given email");
            var employee = _mapper.Map<Employee>(createAdminBySuperAdminDto);
            employee.CreatedBy = createdBy;
            employee.CreatedOn= DateTime.Now;
            var user = _mapper.Map<User>(createAdminBySuperAdminDto);
            user.CreatedBy = createdBy;
            user.CreatedOn = DateTime.Now;
            await _superAdminRepo.CreateAdminProfile(user, employee, createAdminBySuperAdminDto.Password);
            return new ResponseDto()
            {
                Message=$"Successfully Created Admin. Username: {user.UserName}, Password: {createAdminBySuperAdminDto.Password}",
                Status=true,
                StatusCode="200"
            };
           
        }
        public async Task<ResponseDto> ActivateDeactivateMicroFinanceUserService(string userName, bool isActive)
        {
           var user = await _employeeRepository.GetUserByUsername(userName);
           // Check if user exist and is the status is same or not
           if(user!=null && user.IsActive!=isActive)
           {
                user.IsActive=isActive;
                var updateStatus = await _employeeRepository.ActivateOrDeactivateUser(user);
                if(updateStatus.Succeeded)
                {
                    return new ResponseDto()
                        {
                            Message="Active Status update successfull",
                            StatusCode="200",
                            Status=true
                        };
                }
                return new ResponseDto()
                {
                    Message=$"Update failed. Error: {updateStatus.Errors}",
                    StatusCode="500",
                    Status=false
                };
           }
           else if(user?.IsActive==isActive)
           {
                return new ResponseDto()
                {
                    Message="No changes made",
                    StatusCode="200",
                    Status=true
                };
           }
           return new ResponseDto()
            {
                    Message="User Not Found",
                    StatusCode="404",
                    Status=false
            };
        }

        

        public async Task<List<UserDetailsDto>> GetMicroFinanceUserSerivce()
        {
            return await _employeeService.GetUsersDetailsService();
        }

        
    }
}