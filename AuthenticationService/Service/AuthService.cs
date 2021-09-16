using AuthenticationService.Exceptions;
using AuthenticationService.Models;
using AuthenticationService.Repository;
using System;
namespace AuthenticationService.Service
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e AuthService by inheriting IAuthService class 
    //which is used to implement all methods in the classs.
    public class AuthService:IAuthService
    {
        //define a private variable to represent repository
        IAuthRepository _authRepository;
        //Use constructor Injection to inject all required dependencies.
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }




        /* Implement all the methods of respective interface asynchronously*/

        //Implement the method  'RegisterUser' which is used to register a new user and 
        // handle the Custom Exception for UserAlreadyExistsException

        public bool RegisterUser(User user)
        {
            bool flag = _authRepository.CreateUser(user);
            if (flag)
                return flag;
            else
                throw new UserAlreadyExistsException($"This userId {user.UserId} already in use");
        }

        //Implement the method 'LoginUser' which is used to login existing user and also handle the Custom Exception 
        public bool LoginUser(User user)
        {
            bool flag = _authRepository.LoginUser(user);
            if (flag)
                return true;
            else
                throw new UnauthorizedAccessException("Invalid user id or password");
        }
    }
}
