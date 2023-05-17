using Dapper;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Text;
using WatchCatalogAPI.Model;

namespace WatchCatalogAPI.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private IJsonSerializer _serializer = new JsonNetSerializer();
        private IDateTimeProvider _provider = new UtcDateTimeProvider();
        private IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
        private IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();

        public IConfiguration _configuration { get; }

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            context.Items["expired_token"] = false;
            if (token != null)
                await attachAccountToContext(context, token);

            await _next(context);
        }
        private async Task attachAccountToContext(HttpContext context, string token)
        {
            try
            {
                ServiceResponse<string> apiResponse = new ServiceResponse<string>();
                #region Check token expiry
                try
                {
                    var prob = _provider;
                    IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                    IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                    var token1 = decoder.DecodeToObject<JWTToken>(token);
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token1.exp);
                    var token_expiry = dateTimeOffset.LocalDateTime;

                    if (DateTime.Now > token_expiry)
                    {
                        context.Items["expired_token"] = true;
                    }
                    else
                    {
                        context.Items["expired_token"] = false;
                        var tokenHandler = new JwtSecurityTokenHandler();

                        tokenHandler.ValidateToken(token, new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = _configuration["AuthManagerIssuer"],
                            ValidAudience = _configuration["AuthManagerIssuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthManagerKey"]))
                        }, out SecurityToken validatedToken);

                        var jwtToken = (JwtSecurityToken)validatedToken;

                        JWTAuthModel auth = new JWTAuthModel();

                        auth.apiKey = jwtToken.Claims.First(x => x.Type == "sub").Value;
                        auth.apiUsername = jwtToken.Claims.First(x => x.Type == "username").Value;
                        auth.apiPassword = jwtToken.Claims.First(x => x.Type == "password").Value;

                        // attach account to context on successful jwt validation
                        context.Items["JWTAuthModel"] = auth; // await dataContext.Accounts.FindAsync(accountId);

                    }
                    //return dateTimeOffset.LocalDateTime;
                }
                catch (TokenExpiredException)
                {
                    //return DateTime.MinValue;
                }
                catch (SignatureVerificationException)
                {
                    //return DateTime.MinValue;
                }
                catch (Exception ex)
                {
     
                }
                #endregion


            }
            catch (Exception ex)
            {

            }
        }
    }
}
