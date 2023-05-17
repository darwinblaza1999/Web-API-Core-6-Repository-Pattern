using Dapper;
using System.Data.SqlClient;
using System.Data;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;
using WatchCatalogAPI.Class;

namespace WatchCatalogAPI.Repository
{
    public class GenericRepository<T> : IGeneric<T> where T : class
    {
        private readonly Connection _connection;
        public GenericRepository(Connection connection)
        {
            _connection = connection;
        }

        public async Task<Response<T>> AddAsync(string sp, T Entity)
        {
            var response = new Response<T>();
            try
            {
                var param = new DynamicParameters();
                var property = Entity.GetType().GetProperties();
                foreach (var item in property)
                {
                    param.Add(item.Name, item.GetValue(Entity));
                }
                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync(sp, param, commandType: CommandType.StoredProcedure);
                    response.Data = result.FirstOrDefault();
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Code = param.Get<int>("retval") == 100 ? ResponseCode.Success : param.Get<int>("retval");
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

        public async Task<Response<T>> DeleteAsync(string sp, T id)
        {
            var response = new Response<T>();
            try
            {
                var param = new DynamicParameters();
                var property = id.GetType().GetProperties();
                foreach ( var item in property)
                {
                    var name = item.Name;
                    var value = item.GetValue(id);
                    param.Add(name, value);
                }

                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync<T>(sp, param, commandType: CommandType.StoredProcedure);
                    response.Data = result.FirstOrDefault();
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Code = param.Get<int>("retval") == 100 ? ResponseCode.Success : param.Get<int>("retval");
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

        public async Task<Response<List<T>>> GetAllAsync(string sp)
        {
            var response = new Response<List<T>>();
            try
            {
                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync<T>(sp, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Data = result.ToList();
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

        public async Task<Response<T>> GetByIdAsync(string sp, T id)
        {
            var response = new Response<T>();
            try
            {
                var param = new DynamicParameters();
                var property = id.GetType().GetProperties();
                foreach (var item in property)
                {
                    var name = item.Name;
                    var value = item.GetValue(id);
                    param.Add(name, value);
                }

                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync(sp, param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Data = result.FirstOrDefault();
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

        public async Task<Response<T>> UpdateAsync(string sp, T Entity)
        {
            var response = new Response<T>();
            try
            {
                var param = new DynamicParameters();
                var property = Entity.GetType().GetProperties();
                foreach (var item in property)
                {
                    var name = item.Name;
                    var value = item.GetValue(Entity);
                    param.Add(name, value);
                }

                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = _connection.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync<T>(sp, param, commandType: CommandType.StoredProcedure);
                    response.Data = result.FirstOrDefault();
                    response.HttpCode = ResponseStatusCode.Success;
                    response.Code = param.Get<int>("retval") == 100 ? ResponseCode.Success : param.Get<int>("retval");
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
