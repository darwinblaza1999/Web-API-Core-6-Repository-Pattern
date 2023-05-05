using Dapper;
using System.Data.SqlClient;
using System.Data;
using WatchCatalogAPI.Model;
using WatchCatalogAPI.Repository.Interface;
using Newtonsoft.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WatchCatalogAPI.Class
{
    public class WatchRepository : IWatch
    {
        private readonly Connection _connectionString;
        public WatchRepository(Connection connectionString) => _connectionString = connectionString;

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
                using (var con = _connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.QueryAsync("usp_AddItem", param, commandType: CommandType.StoredProcedure);
                    response.HttpCode = ResponseStatusCode.Success;
                    if (param.Get<int>("retval") == 100)
                    {
                        response.Code = ResponseCode.Success;
                        response.DeveloperMessage = "Success";
                    }
                    else if (param.Get<int>("retval") == 2)
                    {
                        response.Code = ResponseCode.Duplicate;
                        response.DeveloperMessage = "Unable to save, Item name already exists.";
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
                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = _connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.QueryAsync("usp_DeleteItem", param, commandType: CommandType.StoredProcedure);
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

        public async Task<Response<object>> GetAllAsync()
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemNo", 0, DbType.Int32);
                param.Add("type", "", DbType.String);
                using (var con = _connectionString.CreateConnectionCatalogDB())
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

        public async Task<Response<object>> GetAsync(int id)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemNo", id, DbType.Int32);
                param.Add("type", "BYID", DbType.String);
                using (var con = _connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    var result = await con.QueryAsync("usp_SelectItem", param, commandType: CommandType.StoredProcedure);
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

        public async Task<Response<object>> Update(WatchDetails1 details)
        {
            var response = new Response<object>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@itemName", details.ItemName, DbType.String);
                param.Add("@itemNo", details.ItemNo, DbType.Int32);
                param.Add("@shortDescription", details.ShortDescription, DbType.String);
                param.Add("@fullDescription", details.FullDescription, DbType.AnsiString);
                param.Add("@price", details.Price, DbType.Decimal);
                param.Add("@caliber", details.Caliber, DbType.String);
                param.Add("@movement", details.Movement, DbType.String);
                param.Add("@chronograph", details.Chronograph, DbType.String);
                param.Add("@weight", details.Weight, DbType.Decimal);
                param.Add("@height", details.Height, DbType.Decimal);
                param.Add("@diameter", details.Diameter, DbType.String);
                param.Add("@thickness", details.Thickness, DbType.Decimal);
                param.Add("@jewel", details.Jewel, DbType.Int32);
                param.Add("@caseMaterial", details.CaseMaterial, DbType.String);
                param.Add("@strapMaterial", details.StrapMaterial, DbType.String);
                param.Add("retval", dbType: DbType.Int32, direction: ParameterDirection.Output);
                using (var con = _connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = await con.ExecuteAsync("usp_UpdateItem", param, commandType: CommandType.StoredProcedure);
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

        public Response<WatchDetails1> UpdateImage(WatchImage model)
        {
            var response = new Response<WatchDetails1>();
            try
            {
                var param = new DynamicParameters();
                param.Add("itemno", model.ItemNo, DbType.Int32);
                param.Add("image", model.Image, DbType.String);
                param.Add("retval", DbType.Int32, direction:ParameterDirection.Output);
                using (var con = _connectionString.CreateConnectionCatalogDB())
                {
                    con.Open();
                    response.Data = con.Query<WatchDetails1>("usp_UpdateImage", param, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
