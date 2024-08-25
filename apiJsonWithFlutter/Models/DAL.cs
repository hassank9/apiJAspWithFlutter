using Microsoft.Extensions.Configuration;
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

    }
}
