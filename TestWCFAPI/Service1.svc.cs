using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using TestWCFAPI.Model;

namespace TestWCFAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public async Task<PackageDetails> GetPackageData(int value)
        {
            PackageDetails pkgDetails = new PackageDetails();

            return await GetPackagerDetailsAsync(value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public async Task<PackageDetails> GetPackagerDetailsAsync(int PackageID)
        {
            string connectionString = "User Id=SYSTEM;Password=admin;Data Source=DESKTOP-6PD0NT9:1521/XE:SYSTEM";
            OracleConnection oracleConnection = null;
            PackageDetails pkgrDetail = new PackageDetails();
            try
            {
                using (oracleConnection = new OracleConnection(connectionString))
                {
                    oracleConnection.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = oracleConnection;
                        cmd.CommandText = "select * from packagedetails where id = " + PackageID;//"PKG_PAYU_MANAGER.PROC_PAYU_IS_CUSTOMER_EXISTS";
                                                                                                   // cmd.Parameters.Add("i_CUSTOMERID", OracleDbType.Int32, CustomerID, ParameterDirection.Input);

                        DbDataReader reader = await cmd.ExecuteReaderAsync();

                        while (await reader.ReadAsync())
                        {
                            PackageDetails _user = new PackageDetails();
                            pkgrDetail.PackageId = await reader.IsDBNullAsync(0) ? 0 : Convert.ToInt32(reader.GetString(0));
                            pkgrDetail.Name = await reader.IsDBNullAsync(1) ? null : reader.GetString(1);
                            pkgrDetail.Language = await reader.IsDBNullAsync(2) ? null : reader.GetString(2);
                            pkgrDetail.Channels = await reader.IsDBNullAsync(3) ? null : reader.GetString(3);
                            pkgrDetail.TypeId = await reader.IsDBNullAsync(4) ? 0 : Convert.ToInt32(reader.GetString(4));
                            pkgrDetail.Description = await reader.IsDBNullAsync(5) ? null : reader.GetString(5);

                            //userList.Add(_user);
                        }
                        //await reader.DisposeAsync();
                        return pkgrDetail;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (oracleConnection != null)
                { oracleConnection.Close(); }
            }

        }
    }
}
