using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Repository
{
    //Referencia - ConfigureAwait(false):  https://medium.com/bynder-tech/c-why-you-should-use-configureawait-false-in-your-library-code-d7837dce3d7f
    //Referencia - Documentación: https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/asynchronous-programming
    //Referencia - Ejemplos: https://gist.github.com/imranbaloch/10895917

    /// <summary>
    ///  - ExeQuery: Ejecuta una sentencia que finalmente retorna un SELECT [También ejecuta INSERT, UPDATE y DELETE, pero al final debe devolver SELECT].
    ///  - Function: Ejecuta una función dentro de MSSQL.
    ///  - NonQuery: Ejecuta una sentencia que no retonar SELECT [Solo para INSERT, UPDATE y DELETE].
    /// </summary>
    internal sealed class DbHelper
    {
        private readonly string _cnString;
        private const int SQL_COMMAND_TIME_OUT = 200; //30 (default)

        public enum EnumExecute : short { ExeQuery, Function, NonQuery };

        public DbHelper(string connectionString)
        {
            _cnString = connectionString;
        }

        public async Task<int> DoNonQuery(string query, params object[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_cnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = SQL_COMMAND_TIME_OUT; // seconds
                    await connection.OpenAsync().ConfigureAwait(false); //Evita que se llame Result para esperar el resultado de forma síncrona.

                    if (parameters?.Length != 0) AddParameters(cmd, parameters);
                    return await cmd.ExecuteNonQueryAsync(); //Retorna -1 si hay error, sino devuelve el número de filas afectadas en INSERT, UPDATE y DELETE.
                }
            }
        }

        public async Task<object> DoExecuteScalar(string query, params object[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_cnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = SQL_COMMAND_TIME_OUT; // seconds
                    await connection.OpenAsync().ConfigureAwait(false);

                    if (parameters?.Length != 0) AddParameters(cmd, parameters);
                    return await cmd.ExecuteScalarAsync();
                }
            }
        }

        public async Task<dynamic> DoExecuteReader(string query, params object[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_cnString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.CommandTimeout = SQL_COMMAND_TIME_OUT; // seconds
                    await connection.OpenAsync().ConfigureAwait(false); //Evita que se llame Result para esperar el resultado de forma síncrona.

                    if (parameters?.Length != 0) AddParameters(cmd, parameters);
                    using (SqlDataReader dataReader = await cmd.ExecuteReaderAsync())
                    {
                        DataTable data = new DataTable();
                        data.Load(dataReader);
                        return data;
                    }
                }
            }
        }

        private void AddParameters(SqlCommand cmd, params object[] parameters) //IEnumerable<object> parameters
        {
            cmd.CommandType = CommandType.StoredProcedure; //Solo los procedimientos almacenados se les envía parámetro
            SqlCommandBuilder.DeriveParameters(cmd);

            int index = 0;
            foreach (SqlParameter sqlparam in cmd.Parameters)
            {
                if (!sqlparam.ParameterName.Equals("@RETURN_VALUE"))
                {
                    sqlparam.Value = parameters[index] ?? DBNull.Value;
                    index++;
                }
            }
        }

    }
}
