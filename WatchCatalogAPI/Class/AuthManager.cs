using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;

namespace WatchCatalogAPI.Class
{
    public class AuthManager : IAuthManager
    {
        public readonly Connection _connection;
        //private readonly IConfiguration _configuration;
        public AuthManager(Connection connection, IConfiguration configuration)
        {
            _connection = connection;
            //_configuration = configuration;
        }
        public ServiceResponse<JWTAuthModel> Auth(JWTAuthModel auth)
        {
            var response = new ServiceResponse<JWTAuthModel>();
            try
            {
                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    var param = new DynamicParameters();
                    param.Add("username", auth.apiUsername, DbType.String);
                    param.Add("password", auth.apiPassword, DbType.String);
                    param.Add("apikey", auth.apiKey, DbType.String);

                    var result = con.Query<JWTAuthModel>("usp_GetAccount", param, commandType: CommandType.StoredProcedure);
                    response.data = result.FirstOrDefault();
                    if (result.Count() > 0)
                    {
                        response.code = 200;
                        response.token = GenerateJWT(auth).Data;
                        response.DeveloperMessage = "Ok";
                    }
                    else
                    {
                        response.code = 0;
                        response.DeveloperMessage = "No record found";
                    }
                }
            }
            catch (SqlException sql)
            {
                response.code = 901;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.code = 500;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public Response<string> GenerateJWT(JWTAuthModel auth) 
        { 
            var response = new Response<string>();
            try
            {
                var key = _connection.CreateAuthKey();
                var issuer = _connection.CreateAuthIssuer();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_connection.CreateAuthKey()));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //claim is used to add identity to JWT token
                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, auth.apiKey),
                new Claim("username", auth.apiUsername),
                new Claim("password", auth.apiPassword),
                new Claim("Date", DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var token = new JwtSecurityToken(_connection.CreateAuthIssuer(),
                  _connection.CreateAuthIssuer(),
                  claims,
                  expires: DateTime.Now.AddMinutes(5),
                  signingCredentials: credentials);

                //return access token
                response.Data = new JwtSecurityTokenHandler().WriteToken(token);
                response.HttpCode = ResponseStatusCode.Success;
                response.DeveloperMessage = "Token generated";
            }
            catch (Exception ex)
            {
                response.DeveloperMessage = ex.Message;
                response.DeveloperMessage = "The server has encountered an Internal Server Error. Please try again. \n If problem persist, please contact Helpdesk.";
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.ProcessException;
            }
            return response;
        }
    }
}
