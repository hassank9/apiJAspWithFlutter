using Microsoft.Extensions.Configuration;
using MyApi.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace apiJsonWithFlutter.Models
{
    public class DAL
    {
        Random random = new Random();
        int randomNumber;

        public void SendEmailVerfiy(string toAddress, int randomNumbers)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("hassankarim.it3@gmail.com", "ssjfxozusprkjgwf"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("hassankarim.it3@gmail.com"),
                Subject = "Verfiycode App",
                Body = "Verfiycode  " + randomNumbers.ToString(),
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toAddress);

            smtpClient.Send(mailMessage);
        }

        public ResponseUsers GetAllUsers(SqlConnection connection, int ID)
        {
            ResponseUsers response = new ResponseUsers();
            List<Users> lstUsers = new List<Users>();

            SqlCommand cmd = new SqlCommand("spSELECT_UsersAppTb", connection);
            if (ID > 0) { cmd.Parameters.AddWithValue("ID", ID); }
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                Users users = new Users();
                users.users_id = Convert.ToInt32(read["users_id"].ToString());
                users.users_name = read["users_name"].ToString();
                users.users_password = read["users_password"].ToString();
                users.users_email = read["users_email"].ToString();
                users.users_phone = read["users_phone"].ToString();
                users.users_verefiycode = Convert.ToInt32(read["users_verefiycode"].ToString());
                users.users_approve = Convert.ToInt32(read["users_approve"].ToString());
                lstUsers.Add(users);
            }
            if (lstUsers.Count > 0)
            {
                response.StatusCode = 200;
                response.ErrorMessage = "Data Found";
                response.listUsers = lstUsers;
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data Found";
                response.listUsers = null;
                connection.Close();
            }
            return response;
        }



        public ResponseUsers AddUsers(SqlConnection connection, Users users)
        {
            ResponseUsers response = new ResponseUsers();
            SqlCommand cmd = new SqlCommand("spINSERT_UsersAppTb", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            randomNumber = random.Next(10000, 99999);

            cmd.Parameters.AddWithValue("users_name", users.users_name);
            cmd.Parameters.AddWithValue("users_password", users.users_password);
            cmd.Parameters.AddWithValue("users_email", users.users_email);
            cmd.Parameters.AddWithValue("users_phone", users.users_phone);
            cmd.Parameters.AddWithValue("users_verefiycode", randomNumber);
            cmd.Parameters.AddWithValue("users_approve", users.users_approve);

            SendEmailVerfiy(users.users_email.ToString(), randomNumber);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            if (i > 0)
            {
                response.StatusCode = 200;
                response.ErrorMessage = "User added.";
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                response.ErrorMessage = "No Data inserted.";
                connection.Close();
            }
            return response;
        }



        public ResponseVerfiycode reSendVerfiyCode(SqlConnection connection, ResponseVerfiycode responseVerfiycode)
        {
            randomNumber = random.Next(10000, 99999);

            ResponseVerfiycode response = new ResponseVerfiycode();
            SqlCommand cmd = new SqlCommand("spReSeend_verefiycode", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("users_email", responseVerfiycode.users_email);
            cmd.Parameters.AddWithValue("users_verefiycode", randomNumber);

            SendEmailVerfiy(responseVerfiycode.users_email.ToString(), randomNumber);

            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            if (i >= 0)
            {
                response.StatusCode = 200;
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                connection.Close();
            }
            return response;
        }

        public ResponseVerfiycode Verfiycode(SqlConnection connection, ResponseVerfiycode responseVerfiycode)
        {
            ResponseVerfiycode response = new ResponseVerfiycode();
            SqlCommand cmd = new SqlCommand("spVerfiyCode", connection);
            cmd.CommandType = CommandType.StoredProcedure;




            cmd.Parameters.AddWithValue("users_email", responseVerfiycode.users_email);
            cmd.Parameters.AddWithValue("users_verefiycode", responseVerfiycode.users_verefiycode);


            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            if (i == 100)
            {
                response.StatusCode = 200;
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                connection.Close();
            }
            return response;
        }



        public ResponseLogin Login(SqlConnection connection, ResponseLogin responseLogin)
        {
            ResponseLogin response = new ResponseLogin();
            List<Users> lstUsers = new List<Users>();


            SqlCommand cmd = new SqlCommand("spLoginUsersApp", connection);
            cmd.Parameters.AddWithValue("users_email", responseLogin.email);
            cmd.Parameters.AddWithValue("users_password", responseLogin.password);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                Users users = new Users();
                users.users_id = Convert.ToInt32(read["users_id"].ToString());
                users.users_name = read["users_name"].ToString();
                users.users_password = read["users_password"].ToString();
                users.users_email = read["users_email"].ToString();
                users.users_phone = read["users_phone"].ToString();
                users.users_verefiycode = Convert.ToInt32(read["users_verefiycode"].ToString());
                users.users_approve = Convert.ToInt32(read["users_approve"].ToString());
                lstUsers.Add(users);

                if (lstUsers.Count > 0)
                {
                    response.id = users.users_id;
                    response.username = users.users_name;
                    response.email = users.users_email;
                    response.phone = users.users_phone;
                    response.users_approve = users.users_approve.ToString();
                }
            }
            if (lstUsers.Count > 0)
            {
                response.StatusCode = 200;
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                connection.Close();
            }
            return response;
        }

        public void UpdateVerfiy(SqlConnection connection, string email, int verfiy)
        {
            SqlCommand cmd = new SqlCommand("UpdateVerfiy", connection);
            cmd.Parameters.AddWithValue("users_email", email);
            cmd.Parameters.AddWithValue("users_verefiycode", verfiy);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteScalar();
        }


        public ResponseLogin CheckEmail(SqlConnection connection, ResponseLogin responseLogin)
        {
            ResponseLogin response = new ResponseLogin();
            SqlCommand cmd = new SqlCommand("spCheckEmail", connection);
            cmd.Parameters.AddWithValue("users_email", responseLogin.email);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            if (i > 0)
            {
                response.StatusCode = 200;
                randomNumber = random.Next(10000, 99999);
                SendEmailVerfiy(responseLogin.email.ToString(), randomNumber);
                UpdateVerfiy(connection, responseLogin.email, randomNumber);

                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                connection.Close();
            }
            return response;
        }


        public ResponseLogin ResetPassword(SqlConnection connection, ResponseLogin responseLogin)
        {
            ResponseLogin response = new ResponseLogin();
            List<Users> lstlogin = new List<Users>();

            SqlCommand cmd = new SqlCommand("spResetPassword", connection);
            cmd.Parameters.AddWithValue("users_email", responseLogin.email);
            cmd.Parameters.AddWithValue("users_password", responseLogin.password);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());

            if (i > 0)
            {
                response.StatusCode = 200;
                connection.Close();
            }
            else
            {
                response.StatusCode = 100;
                connection.Close();
            }
            return response;
        }
    }
}
