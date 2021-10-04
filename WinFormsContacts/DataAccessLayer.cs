using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsContacts
{
    class DataAccessLayer
    {
        private static string conexionName = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Contacts;Data Source=DESKTOP-VBU73D2\\SQLEXPRESS";
        private SqlConnection conn = new SqlConnection(conexionName);

        public void InsertContacto(Contacto contacto)
        {
            // define return value - newly inserted ID
            // int returnValue = -1;

            try
            {
                conn.Open();
                string query = @"
                    INSERT INTO Contacts (FirstName, LastName, Phone, Address)
                    VALUES (@Nombre, @Apellidos, @Telefono, @Direccion);
                ";

                SqlParameter nombre = new SqlParameter();
                nombre.ParameterName = "@Nombre";
                nombre.Value = contacto.Nombre;
                nombre.DbType = System.Data.DbType.String;

                SqlParameter apellidos = new SqlParameter("@Apellidos", contacto.Apellidos);
                SqlParameter telefono = new SqlParameter("@Telefono", contacto.Telefono);
                SqlParameter direccion = new SqlParameter("@Direccion", contacto.Direccion);

                SqlCommand sqlCommand = new SqlCommand(query, conn);
                
                sqlCommand.Parameters.Add(nombre);
                sqlCommand.Parameters.Add(apellidos);
                sqlCommand.Parameters.Add(telefono);
                sqlCommand.Parameters.Add(direccion);

                sqlCommand.ExecuteNonQuery();
                /*
                object returnObj = sqlCommand.ExecuteScalar();
                if (returnObj != null)
                {
                    int.TryParse(returnObj.ToString(), out returnValue);
                }
                */
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            //return returnValue;
        }

        public void UpdateContacto(Contacto contacto)
        {
            try
            {
                conn.Open();
                string query = @"
                    UPDATE Contacts SET
                        FirstName = @Nombre,
                        LastName = @Apellidos, 
                        Phone = @Telefono, 
                        Address = @Direccion
                    WHERE ID = @Id
                ";

                SqlParameter id = new SqlParameter("@Id", contacto.Id);
                SqlParameter nombre = new SqlParameter("@Nombre", contacto.Nombre);
                SqlParameter apellidos = new SqlParameter("@Apellidos", contacto.Apellidos);
                SqlParameter telefono = new SqlParameter("@Telefono", contacto.Telefono);
                SqlParameter direccion = new SqlParameter("@Direccion", contacto.Direccion);

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(id);
                command.Parameters.Add(nombre);
                command.Parameters.Add(apellidos);
                command.Parameters.Add(telefono);
                command.Parameters.Add(direccion);

                command.ExecuteNonQuery();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
       
        public List<Contacto> GetContactos(string searchString = null)
        {
            List<Contacto> contactos = new List<Contacto>();

            try
            {
                conn.Open();
                string query = @"
                    SELECT ID, FirstName, LastName, Phone, Address
                    FROM Contacts
                ";

                SqlCommand sqlCommand = new SqlCommand(query, conn);

                if (!string.IsNullOrEmpty(searchString))
                {
                    query += @"
                        WHERE FirstName LIKE @searchString
                        OR LastName LIKE @searchString
                        OR Phone LIKE @searchString
                        OR Address LIKE @searchString
                    ";

                    sqlCommand.Parameters.Add(new SqlParameter("@SearchString", $"%{searchString}%"));
                }

                sqlCommand.CommandText = query;
                sqlCommand.Connection = conn;

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    contactos.Add(new Contacto
                    {
                        Id = int.Parse(reader["ID"].ToString()),
                        Nombre = reader["FirstName"].ToString(),
                        Apellidos = reader["LastName"].ToString(),
                        Telefono = reader["Phone"].ToString(),
                        Direccion = reader["Address"].ToString()
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

            return contactos;
        }

        public void DeleteContacto(int id)
        {
            try
            {
                conn.Open();
                string query = @"DELETE FROM Contacts WHERE ID = @Id";

                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.Add(new SqlParameter("@Id", id));
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
