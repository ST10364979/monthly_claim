using System.Data.SqlClient;
using System.Numerics;

namespace monthly_claims.Models
{
    public class claim
    {

        public string user_email { get; set; }
        public string user_id { get; set; }
        public string hours_worked { get; set; }
        public string hour_rate { get; set; }
        public string description { get; set; }

        //connection
        connection connect = new connection();
        public string insert_claim(string module, string hour_work, string rate, string note, string filename)
        {
            //temp variable message
            string message = "";

            string user_id = get_id();
            string user_email = get_email();

            string total = "" + int.Parse(hour_work) * int.Parse(rate);

            string query = "insert into claiming values('" + user_email + "', '" + module + "','" + user_id + "', '" + hours_worked + "','" + rate + "','" + note + "','none','none','" + total + "','" + filename + "','pending');";

            try
            {

                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {
                    connects.Open();
                    using (SqlCommand done = new SqlCommand(query, connects))
                    {
                        done.ExecuteNonQuery();
                        message = "done";

                    }
                    connects.Close();
                }


            }
            catch (IOException error)
            {
                message = error.Message;
            }

            return message;

        }

        //get id
        public string get_id()
        {
            //hold id variable
            string hold_id = "";

            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {

                    connects.Open();

                    using (SqlCommand prepare = new SqlCommand("select * from active", connects))
                    {
                        using (SqlDataReader getID = prepare.ExecuteReader())
                        {

                            if (getID.HasRows)
                            {
                                //check all, but get one

                                while (getID.Read())
                                {
                                    //then get it
                                    hold_id = getID["id"].ToString();
                                }
                            }
                            getID.Close();
                        }
                    }
                    connects.Close();
                }

            }
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
                hold_id = error.Message;

            }
            return hold_id;
        }
        //get email
        public string get_email()
        {
            //hold email variable
            string hold_email = "";

            try
            {
                using (SqlConnection connects = new SqlConnection(connect.connecting()))
                {

                    connects.Open();

                    using (SqlCommand prepare = new SqlCommand("select * from active", connects))
                    {
                        using (SqlDataReader getEMAIL = prepare.ExecuteReader())
                        {

                            if (getEMAIL.HasRows)
                            {
                                //check all, but get one

                                while (getEMAIL.Read())
                                {
                                    //then get it
                                    hold_email = getEMAIL["email"].ToString();
                                }
                            }
                            getEMAIL.Close();
                        }
                    }
                    connects.Close();
                }

            }
            catch (IOException error)
            {
                Console.WriteLine(error.Message);
                hold_email = error.Message;

            }
            return hold_email;
        }

        internal string insert_claim(string module_name, string hour_work, string hour_rate)
        {
            throw new NotImplementedException();
        }
    }
}