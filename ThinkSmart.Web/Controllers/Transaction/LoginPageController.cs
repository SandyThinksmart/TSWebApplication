using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Xml.Linq;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;
//using UAParser;
using System.Text;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Windows.Markup;
using TSWebApplication.Web.Models;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;
using static System.Net.WebRequestMethods;
using Shyjus.BrowserDetection;
using crypto;
using TSWebApplication.Application.IServices;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class LoginPageController : Controller
    {
        private readonly IUserLoginServices _userLoginService;

        private readonly LocalizationServices _localization;
     
        public LoginPageController(IUserLoginServices userLoginServices, LocalizationServices localization)
        {
          
            _localization = localization;
            _userLoginService = userLoginServices;
           
        }
        public IActionResult LoginPage()

        {
            DateTime filedatetime;
            string UName, Pass;
            UName = string.Empty;
            Pass = string.Empty;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var browsername = string.Empty;

            filedatetime = DateTime.Now;

            var userAgent = HttpContext.Request.Headers["User-Agent"];
            //var uaParser = Parser.GetDefault();
            //ClientInfo c = uaParser.Parse(userAgent);
            //browsername = c.UA.Family;

            //  string path1 = Environment.SpecialFolder.MyDocuments + "\file_" + browsername + ".txt";
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\file_" + browsername + ".txt";

            FileInfo fileinfo1 = new FileInfo(path1);




            if (fileinfo1.Exists)
            {


                filedatetime = fileinfo1.LastWriteTime;
                TimeSpan checkHours = filedatetime - DateTime.Now;           
                double hrs = checkHours.TotalHours;

               string totalhours = hrs.ToString().Replace("-", "");
                hrs = Convert.ToDouble(totalhours);

                if (hrs >= 6)
                {

                    fileinfo1.Delete();



                }
                else
                {


                    List<string[]> values = new List<string[]>();
                    byte[] decrypt = null;
                    using (StreamReader reader = new StreamReader(path1))
                    {
                        while (!reader.EndOfStream)
                        {
                            values.Add(reader.ReadLine().Split("&&"));

                        }

                    }

                    string[] textVal = null;
                    foreach (var i in values)
                    {
                        textVal = i;

                    }


                    decrypt = Convert.FromBase64String(textVal[0].ToString());
                    UName = System.Text.ASCIIEncoding.ASCII.GetString(decrypt);

                    decrypt = Convert.FromBase64String(textVal[1].ToString());
                    Pass = System.Text.ASCIIEncoding.ASCII.GetString(decrypt);

                    var result = _userLoginService.AuthenticateUser(UName, Pass);
                    if (result != null)
                    {
                      
                   // return RedirectToAction("Index", "MenuMaster");
                        return Redirect("http://localhost:6243/Menu.aspx");
                    }
                    else
                    {
                        fileinfo1.Delete();
                        return Ok(new { IsSuccess = true, message = result });
                        //  return RedirectToAction("LoginPage", "LoginPage");

                    }
                   
                }
            }
            return View();

        }
        // public IActionResult ValidateUser([FromBody] LoginDetails request)
        public IActionResult ValidateUser(string UserName, string Password)

        {
            
            DateTime filedatetime;
            string UName, Pass;
            UName = string.Empty;
            Pass = string.Empty;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var browsername = string.Empty;

            filedatetime = DateTime.Now;

                    var userAgent = HttpContext.Request.Headers["User-Agent"];
                    //var uaParser = Parser.GetDefault();
                    //ClientInfo c = uaParser.Parse(userAgent);
                    //browsername = c.UA.Family;

                    //  string path1 = Environment.SpecialFolder.MyDocuments + "\file_" + browsername + ".txt";
                    string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\file_" + browsername + ".txt";

                    FileInfo fileinfo1 = new FileInfo(path1);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, "file_" + browsername + ".txt")))
            {
                byte[] encrypt = System.Text.ASCIIEncoding.ASCII.GetBytes(UserName);
                if (Password == null)
                {
                    Password = "";
                }
                byte[] encrypt1 = System.Text.ASCIIEncoding.ASCII.GetBytes(Password);
                string encrypted = Convert.ToBase64String(encrypt);
                string encrypted1 = Convert.ToBase64String(encrypt1);
                outputFile.WriteLine(encrypted + "&&" + encrypted1);




            }
            var result = _userLoginService.AuthenticateUser(UserName, Password);

            if (result != null)
            {
                // return RedirectToAction("Index", "MenuMaster");
                return Ok(new { IsSuccess = true, message = result });
            }
            else
            {
                fileinfo1.Delete();
                return Ok(new { IsSuccess = true, message = result });
                // return RedirectToAction("LoginPage", "LoginPage");

            }


            //        if (fileinfo1.Exists)
            //        {


            //            filedatetime = fileinfo1.LastWriteTime;
            //            TimeSpan checkHours = filedatetime - DateTime.Now;
            //    double hrs = checkHours.TotalHours;

            //    string totalhours = hrs.ToString().Replace("-", "");
            //    hrs = Convert.ToDouble(totalhours);

            //    if (hrs >= 6)
            //            {

            //                fileinfo1.Delete();



            //                                       }
            //            else
            //            {


            //        List<string[]> values = new List<string[]>();
            //        byte[] decrypt=null;
            //        using (StreamReader reader = new StreamReader(path1))
            //                {
            //            while (!reader.EndOfStream)
            //            {
            //                values.Add(reader.ReadLine().Split("&&"));

            //            }

            //        }

            //        string[] textVal=null;
            //        foreach (var i in values)
            //        {
            //            textVal = i;

            //        }


            //        decrypt = Convert.FromBase64String(textVal[0].ToString());
            //       UName = System.Text.ASCIIEncoding.ASCII.GetString(decrypt);

            //        decrypt = Convert.FromBase64String(textVal[1].ToString());
            //        Pass = System.Text.ASCIIEncoding.ASCII.GetString(decrypt);

            //        var result = _userLoginService.AuthenticateUser(UName, Pass);
            //        if (result != null)
            //        {
            //            return Ok(new { IsSuccess = true, message = result });
            //            //return RedirectToAction("Index", "MenuMaster");
            //        }
            //        else
            //        {
            //            fileinfo1.Delete();
            //            return Ok(new { IsSuccess = true, message = result });
            //          //  return RedirectToAction("LoginPage", "LoginPage");

            //        }

            //    }
            //}
            //        else
            //        {



            //        }

            

        }

      
    }
        

}

