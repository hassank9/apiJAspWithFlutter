﻿using apiJsonWithFlutter.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyApi.Models;
using System.Data.SqlClient;

namespace apiJsonWithFlutter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public ResponseUsers GetUsers()
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseUsers response = new ResponseUsers();
            DAL dal = new DAL();
            response = dal.GetAllUsers(con, -1);
            return response;
        }

        [HttpGet]
        [Route("GetAllUsers/{id}")]
        public ResponseUsers GetUsers(int id)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseUsers response = new ResponseUsers();
            DAL dal = new DAL();
            response = dal.GetAllUsers(con, id);
            return response;
        }

        [HttpPost]
        [Route("AddUsers")]
        public ResponseUsers AddUsers(Users users)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseUsers response = new ResponseUsers();
            DAL dal = new DAL();
            response = dal.AddUsers(con, users);
            return response;
        }

        [HttpPost]
        [Route("reSend")]
        public ResponseVerfiycode reSendVerfiyCode(ResponseVerfiycode responseVerfiycode)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseVerfiycode response = new ResponseVerfiycode();
            DAL dal = new DAL();
            response = dal.reSendVerfiyCode(con, responseVerfiycode);
            return response;
        }


        [HttpPost]
        [Route("Verfiycode")]
        public ResponseVerfiycode Verfiycode(ResponseVerfiycode responseVerfiycode)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseVerfiycode response = new ResponseVerfiycode();
            DAL dal = new DAL();
            response = dal.Verfiycode(con, responseVerfiycode);
            return response;
        }


        [HttpPost]
        [Route("Login")]
        public ResponseLogin Login(ResponseLogin responseLogin)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseLogin response = new ResponseLogin();
            DAL dal = new DAL();
            response = dal.Login(con, responseLogin);
            return response;
        }


        [HttpPost]
        [Route("CheckEmail")]
        public ResponseLogin CheckEmail(ResponseLogin responseLogin)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseLogin response = new ResponseLogin();
            DAL dal = new DAL();
            response = dal.CheckEmail(con, responseLogin);
            return response;
        }


        [HttpPost]
        [Route("ResetPassword")]
        public ResponseLogin ResetPassword(ResponseLogin responseLogin)
        {
            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("AppConn").ToString());
            ResponseLogin response = new ResponseLogin();
            DAL dal = new DAL();
            response = dal.ResetPassword(con, responseLogin);
            return response;
        }
    }
}
