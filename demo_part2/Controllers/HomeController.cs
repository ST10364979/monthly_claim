using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using monthly_claims.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace monthly_claims.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            //check the connection
            try
            {
                //get the connction string from the connection class
                connection conn = new connection();

                //then check
                using (SqlConnection connect = new SqlConnection(conn.connecting()))
                {

                    //open connection
                    connect.Open();
                    Console.WriteLine("connected");
                    connect.Close();
                }

            }
            catch (IOException error)
            {

                //error message
                Console.WriteLine("Error : " + error.Message);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //http post for the register

        //from the register form
        [HttpPost]

        public IActionResult Register_user(register add_user)
        {
            //collect user's value
            string name = add_user.username;
            string email = add_user.email;
            string password = add_user.password;
            string role = add_user.role;

            //check if all are collecte
            // Console.WriteLine("Name: " + name + "\nEmail: " + email + "Role: " + role);

            //pass all the values to insert method
            string message = add_user.insert_user(name, email, role, password);

            //then check if the user is inserted
            if (message == "done")
            {
                //track error output
                Console.Write(message);
                //redirect
                return RedirectToAction("Login", "Home");

            }
            else
            {
                //track error output
                Console.Write(message);
                //redirect
                return RedirectToAction("Index", "Home");
            }
            //redirect
            return RedirectToAction("Index" + "Home");
        }

        //for login page
        public IActionResult Login()
        {

            return View();

        }
        public IActionResult Dashboard()
        {

            return View();

        }
        //login page
        [HttpPost]
        public IActionResult login_user(check_login user)
        {

            //the assign
            string email = user.email;
            string role = user.role;
            string password = user.password;

            string message = user.login_user(email, role, password);
            if (message == "found")
            {
                Console.WriteLine(message);
                return RedirectToAction("Dashboard", "Home");

            }
            else
            {
                Console.WriteLine(message);
                return RedirectToAction("Login", "Home");

            }
        }

        [HttpPost]
        public IActionResult claim_sub(IFormFile file, claim insert)
        {

            //assign
            string module_name = insert.user_email;
            string hour_work = insert.hours_worked;
            string hour_rate = insert.hour_rate;
            string description = insert.description;

            //file info
            string filename = "no file";
            if (file != null && file.Length > 0)
            {
                // Get the file name
                filename = Path.GetFileName(file.FileName);
                // Define the folder path (pdf folder)
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdf");
                // Ensure the pdf folder exists
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                // Define the full path where the file will be saved
                string filePath = Path.Combine(folderPath, filename);
                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);


                }
            }


            string message = insert.insert_claim(module_name, hour_work, hour_rate, description, filename);

            if (message == "done")
            {
                Console.WriteLine(message);
                return RedirectToAction("Dahboard", "Home");

            }
            else
            {
                Console.WriteLine(message);
                return RedirectToAction("Dashboard", "Home");
            }

        }
        public IActionResult view_claims ()
        {

            get_claims collect = new get_claims();


            return View(collect);
        }

        [Authorize(Roles = "pc")]
        public IActionResult Approve()
        {
            var claims = new get_claims(); // Fetch all claims
            return View(claims);
        }

        [Authorize(Roles = "pc")]
        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            string message = ApproveClaimInDatabase(claimId);
            if (message == "done")
            {
                Console.WriteLine(message);
                return RedirectToAction("Approve", "Home");
            }
            else
            {
                Console.WriteLine(message);
                return RedirectToAction("Approve", "Home");
            }
        }

        private string ApproveClaimInDatabase(int claimId)
        {
            string message = "error";
            try
            {
                using (SqlConnection connects = new SqlConnection(new connection().connecting()))
                {
                    connects.Open();
                    string query = "UPDATE claiming SET status = 'approved' WHERE id = claimId";
                    using (SqlCommand cmd = new SqlCommand(query, connects))
                    {
                        cmd.ExecuteNonQuery();
                        message = "done";
                    }
                    connects.Close();
                }
            }
            catch (IOException error_db)
            {
                Console.WriteLine(error_db.Message);
                message = error_db.Message;
            }
            return message;
        }


    }
}