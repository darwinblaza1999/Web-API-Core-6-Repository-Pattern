using Dapper;
using System.Data.SqlClient;
using System.Data;
using WatchCatalogAPI.Class;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;

namespace WatchCatalogAPI.Repository
{
    public class WatchRepository:IWatch
    {
        private readonly Connection connectionString;
        public WatchRepository(Connection connectionString) => this.connectionString = connectionString;

        public async Task<Response<object>> Add(WatchDetails details)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                var property = details.GetType().GetProperties();
                foreach (var item in property)
                {
                    param.Add(item.Name, item.GetValue(details));
                }
                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = this.connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.QueryAsync("usp_AddItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    if(param.Get<int>("retval") == 100) 
                    { 
                        response.Code = ResponseCode.Success;
                        response.DeveloperMessage = "Success";
                    }
                    else if(param.Get<int>("retval") == 2) 
                    { 
                        response.Code = ResponseCode.Duplicate;
                        response.DeveloperMessage = "Already exists";
                    }
                    else 
                    { 
                        response.Code = ResponseCode.Default;
                        response.DeveloperMessage = "Failed";
                    }
                    con.Close();
                }
            }
            catch (SqlException sql)
            {
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.SqlError;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response<object>> Delete(int id)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemNo", id, dbType: DbType.Int32);
                param.Add("retval", dbType: DbType.Int32, direction:ParameterDirection.Output);
                using (var con = this.connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.QueryAsync("usp_DeleteItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Code = param.Get<int>("retval") == 100 ? ResponseCode.Success : ResponseCode.Default;
                    response.DeveloperMessage = param.Get<int>("retval") == 100 ? "Ok" : "Failed";
                    con.Close();
                }
            }
            catch (SqlException sql)
            {
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.SqlError;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response<object>> GetAllAsync()
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemNo", 0, DbType.Int32);
                param.Add("type", "", DbType.String);
                using (var con = connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync("usp_SelectItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Data = result;
                    response.Code = result.Any() ? ResponseCode.Success : ResponseCode.NotFound;
                    response.DeveloperMessage = result.Any() ? "Ok": "No record Found";
                    con.Close();
                }
            }
            catch (SqlException sql)
            {
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.SqlError;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response<object>> GetAsync(int id)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemNo", id, DbType.Int32);
                param.Add("type", "BYID", DbType.String);
                using (var con = connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync("usp_SelectItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Data = result;
                    response.Code = result.Any() ? ResponseCode.Success : ResponseCode.NotFound;
                    response.DeveloperMessage = result.Any() ? "Ok" : "No record Found";
                    con.Close();
                }
            }
            catch (SqlException sql)
            {
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.SqlError;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }

        public async Task<Response<object>> Update(WatchDetails details)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                var property = details.GetType().GetProperties();
                foreach (var item in property)
                {
                    param.Add(item.Name, item.GetValue(details));
                }
                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = this.connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.ExecuteAsync("usp_UpdateItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    if(param.Get<int>("retval") == 100) 
                    { 
                        response.Code = ResponseCode.Success;
                        response.DeveloperMessage = "Success";
                    }
                    else if(param.Get<int>("retval") == -1) 
                    {
                        response.Code = ResponseCode.NotFound;
                        response.DeveloperMessage = "Id not found";
                    }
                    else
                    {
                        response.Code = ResponseCode.Default;
                        response.DeveloperMessage = "Failed";
                    }
                    response.DeveloperMessage = param.Get<int>("retval") == 100 ? "Success" : "Failed";
                    con.Close();
                }
            }
            catch (SqlException sql)
            {
                response.HttpCode = ResponseStatusCode.InternalError;
                response.Code = ResponseCode.SqlError;
                response.DeveloperMessage = sql.Message;
            }
            catch (Exception ex)
            {
                response.HttpCode = ResponseStatusCode.BadRequest;
                response.Code = ResponseCode.InternalException;
                response.DeveloperMessage = ex.Message;
            }
            return response;
        }
    }
}
