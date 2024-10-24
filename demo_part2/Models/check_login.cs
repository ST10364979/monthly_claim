using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace monthly_claims.Models
{
    public class check_login
    {

        public string email { get; set; }
        public string role { get; set; }
        public string password { get; set; }

        //connection string
        connection connect = new connection();

        //method to check the user
        public string login_user(string emails, string roles, string passwords)
        {
            //temp message
            string message = "";
            Console.WriteLine(email + " and " + password);
            try
            {
                //connect and open
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {

                    //open connection
                    connects.Open();

                    //query
                    string query = "select * from users where email='" + emails + "' and password='" + passwords + "';";

                    //prepare to execute
                    using (SqlCommand prepare = new SqlCommand(query, connects))
                    {

                        //read the data
                        using (SqlDataReader find_user = prepare.ExecuteReader())
                        {

                            //then check if the user is found
                            if (find_user.HasRows)
                            {


                                //then assign message
                                message = "found";

                            }
                            else
                            {

                                message = "not";
                                Console.Write(message);
                            }


                        }

                    }
                    connects.Close();
                    if (message == "found")
                    {
                        update_active(email);
                    }
                }
            }
            catch (IOException error_db)
            {

                //return message
                message = error_db.Message;
            }
            return message;

        }
        //update active method
        public void update_active(string email)
        {
            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();

                    string query = "update active set email='" + email + "'";


                    using (SqlCommand done = new SqlCommand(query, connects))
                    {

                        done.ExecuteNonQuery();

                    }
                    connects.Close();
                }

            }
            catch (IOException error)
            {
                Console.WriteLine("erorr " + error.Message);
            }

        }
    }
}