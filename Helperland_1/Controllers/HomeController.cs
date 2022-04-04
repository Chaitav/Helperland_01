using Helperland_1.Data;
using Helperland_1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Helperland_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelperlandContext _DbContext;
        private readonly IWebHostEnvironment hostEnvironment;

        public HomeController(ILogger<HomeController> logger, HelperlandContext DbContext , IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _DbContext = DbContext;
            this.hostEnvironment = hostEnvironment;
        }
  
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Faqs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactUs obj)
        {

            
            obj.Name = HttpContext.Request.Form["FName"] + " " + HttpContext.Request.Form["LName"];
            obj.CreatedOn = DateTime.Now;
            _DbContext.ContactUs.Add(obj);
            _DbContext.SaveChanges();
            TempData["Msg"] = "Response has been recorded";
            return RedirectToAction("Contact");
        }

        public IActionResult Prices()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult NewAccount()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
        [HttpPost]
        public IActionResult NewAccount(User user)
        {
            User x = _DbContext.User.Where(x => x.Email == user.Email).FirstOrDefault();
            if (ModelState.IsValid && x == null)
            {
                User u = new User();
                u.FirstName = user.FirstName;
                u.LastName = user.LastName;
                u.Email = user.Email;
                u.Password = user.Password;
                u.Mobile = user.Mobile;
                u.UserTypeId = 1;
                u.CreatedDate = DateTime.Now;
                u.ModifiedDate = DateTime.Now;
                u.ModifiedBy = 0;
                u.IsApproved = false;
                u.IsActive = true;
                u.IsDeleted = false;
                _DbContext.User.Add(u);
                _DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = TempData["error"];
                return View();
            }
        }
        public IActionResult ServiceProvider()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ServiceProvider(User userr)
        {
            var x = _DbContext.User.FirstOrDefault(p => p.Email.Equals(userr.Email));
            if (ModelState.IsValid && x == null)
            {
                User u = new User();
                u.FirstName = userr.FirstName;
                u.LastName = userr.LastName;
                u.Email = userr.Email;
                u.Password = userr.Password;
                u.Mobile = userr.Mobile;
                u.UserTypeId = 2;
                u.CreatedDate = DateTime.Now;
                u.ModifiedDate = DateTime.Now;
                u.ModifiedBy = 0;
                u.IsApproved = false;
                u.IsActive = true;
                u.IsDeleted = false;
                _DbContext.User.Add(u);
                _DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = TempData["error"];
                return RedirectToAction("ServiceProvider");
            }
        }


        [HttpPost]
        public IActionResult LoginAdd(User userr)
        {

            var ue = _DbContext.User.Where(x => x.Email == userr.Email && x.Password == userr.Password).FirstOrDefault();
            if (ue != null)
            {
                if (ue.UserTypeId == 1 & ue.IsActive == true)
                {
                    ViewBag.Message = String.Format("No matching email");
                    HttpContext.Session.SetInt32("userid", ue.UserId);
                    HttpContext.Session.SetString("username", ue.FirstName + " " + ue.LastName);
                    return RedirectToAction("Index");
                }

                else if (ue.UserTypeId == 2 & ue.IsActive == true)
                {
                    ViewBag.Message = String.Format("No matching email");
                    HttpContext.Session.SetInt32("userid", ue.UserId);
                    HttpContext.Session.SetString("username", ue.FirstName + " " + ue.LastName);
                    return RedirectToAction("MainFS");
                }
                else if (ue.UserTypeId == 3)
                {

                    return RedirectToAction("UserManagement");
                }
                else
                {
                    TempData["abc"] = "Your id is deactivated!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["abc"] = "Wrong Id or Password!";
                return RedirectToAction("Index");
            }
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("userid");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ForgotPassword(User model)
        {

            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var X = _DbContext.User.FirstOrDefault((System.Linq.Expressions.Expression<Func<User, bool>>)((User p) => (bool)p.Email.Equals((string)model.Email))).UserId;
            string baseUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
            var activationUrl = $"{baseUrl}/Home/ForgotPwd?UserId={X}";

            var get_user = _DbContext.User.FirstOrDefault((System.Linq.Expressions.Expression<Func<User, bool>>)(p => (bool)p.Email.Equals((string)model.Email)));
            if (get_user != null)
            {
                MailMessage ms = new MailMessage();
                ms.To.Add(model.Email);
                ms.From = new MailAddress("studydesk69@gmail.com");
                ms.Subject = "Credentials";
                ms.Body = activationUrl;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.Port = 587;


                NetworkCredential NetworkCred = new NetworkCredential("studydesk69@gmail.com", "diciple.69" );
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Send(ms);
                ViewBag.Message = "mail has been sent successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "verify your email";
                return RedirectToAction("Faqs");
            }

        }
        public IActionResult ForgotPwd(int UserId)
        {
            User user = _DbContext.User.Where(x => x.UserId == UserId).FirstOrDefault();

            return View(user);
        }

        [HttpPost]
        public IActionResult resetpassword(User user)
        {
            User userData = _DbContext.User.Where(x => x.UserId == user.UserId).FirstOrDefault();

            userData.Password = user.Password;
            _DbContext.User.Update(userData);
            _DbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult BookService()
        {
            var xyz = HttpContext.Session.GetInt32("userid");
            if (xyz != null)
            {
                return View();
            }
            else
            {
                TempData["abc"] = "You need to Login first!";
                return RedirectToAction("Index");
            }

        }


        [HttpPost]
        public string Zipcode(string zip)
        {
            var isvalid = _DbContext.User.Where(x => x.ZipCode == zip).FirstOrDefault();
            string a;
            if (isvalid != null)
            {
                a = "true";
            }
            else
            {
                a = "false";
            }
            return a;
        }

        public string savebooking([FromBody] ServiceRequest add)
        {
            add.UserId = (int)HttpContext.Session.GetInt32("userid");
            add.ServiceId = 12345;
            _DbContext.ServiceRequest.Add(add);
            _DbContext.SaveChanges();
            var ab = _DbContext.UserAddress.Where(x => x.AddressId == add.AddressId).FirstOrDefault();
            ServiceRequestAddress sa = new ServiceRequestAddress();
            sa.ServiceRequestId = add.ServiceRequestId;
            sa.AddressLine1 = ab.AddressLine1;
            sa.AddressLine2 = ab.AddressLine2;
            sa.City = ab.City;
            sa.Mobile = ab.Mobile;
            sa.State = ab.State;
            sa.PostalCode = ab.PostalCode;
            _DbContext.ServiceRequestAddress.Add(sa);
            _DbContext.SaveChanges();
            add.ServiceId = 1000 + add.ServiceRequestId;
            _DbContext.ServiceRequest.Update(add);
            _DbContext.SaveChanges();
            string message = "true";
            return message;

        }

        public IActionResult YourDetail()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            List<UserAddress> u = _DbContext.UserAddress.Where(x => x.UserId == ty).ToList();
            System.Threading.Thread.Sleep(2000);
            return View(u);
        }


        [HttpPost]
        public string YourDetail([FromBody] UserAddress address)
        {
            address.UserId = (int)HttpContext.Session.GetInt32("userid");
            _DbContext.UserAddress.Add(address);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult UpcomingServiceFC()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            List<ServiceRequest> wt = _DbContext.ServiceRequest.Where(x => x.UserId == ty && x.Status == null).ToList();

            return View(wt);
        }

        public IActionResult SettingsFC()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            return View(u);
        }

        public IActionResult ServiceHistoryFC()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            List<ServiceRequest> u = _DbContext.ServiceRequest.Where(x => x.UserId == ty && x.Status != null).ToList();
            return View(u);
        }

        public IActionResult FavFC()
        {
            var a = HttpContext.Session.GetInt32("userid");
            if (a != null)
            {
                var query = (from user in _DbContext.User
                             join FavoriteAndBlocked in _DbContext.FavoriteAndBlocked
                             on user.UserId equals FavoriteAndBlocked.UserId
                             where FavoriteAndBlocked.TargetUserId == a
                             select new Popup
                             {
                                 Id = FavoriteAndBlocked.Id,
                                 FirstName = user.FirstName,
                                 LastName = user.LastName,
                                 IsBlocked = FavoriteAndBlocked.IsBlocked,
                                 UserId = user.UserId,
                                 Ratings = (from Rating in _DbContext.Rating where Rating.RatingTo.Equals(user.UserId) select Rating.Ratings).Average(),
                             }).ToList();
                return View(query);

            }
            else
            {
                TempData["error"] = "please login first";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult NotifyFC()
        {
            return View();
        }

        public IActionResult Open(int valuess)
        {
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         where ServiceRequest.ServiceRequestId == valuess
                         select new Popup
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             SubTotal = ServiceRequest.SubTotal,
                             ServiceHours = ServiceRequest.ServiceHours,
                             ServiceId = ServiceRequest.ServiceId,
                             Mobile = ServiceRequestAddress.Mobile,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                         }).Single();


            return View(query);
        }

        public string btnOpen([FromBody] ServiceRequest test)
        {
            ServiceRequest abc = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == test.ServiceRequestId).FirstOrDefault();
            abc.Status = 0;
            abc.Comments = test.Comments;
            _DbContext.ServiceRequest.Update(abc);
            _DbContext.SaveChanges();
            return "true";
        }

        public string DetailsTab([FromBody] User test)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            u.FirstName = test.FirstName;
            u.LastName = test.LastName;
            u.Mobile = test.Mobile;
            u.Email = test.Email;
            u.DateOfBirth = test.DateOfBirth;
            _DbContext.User.Update(u);
            _DbContext.SaveChanges();
            return "true";
        }

        public string NewPwd([FromBody] User pass)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            if (u.Password == pass.Password)
            {
                u.Password = pass.NewPassword;
                _DbContext.User.Update(u);
                _DbContext.SaveChanges();
                return "true";
            }
            return "false";
        }

        public IActionResult AddressMenu()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            List<UserAddress> tu = _DbContext.UserAddress.Where(x => x.UserId == ty).ToList();
            return View(tu);
        }

        public string deleteTab(int i)
        {
            UserAddress u = _DbContext.UserAddress.Where(x => x.AddressId == i).FirstOrDefault();
            _DbContext.UserAddress.Remove(u);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult EditAddress(int edit)
        {
            UserAddress u = _DbContext.UserAddress.Where(x => x.AddressId == edit).FirstOrDefault();
            return View(u);
        }

        public string editPopup([FromBody] UserAddress change)
        {
            UserAddress getaddress = _DbContext.UserAddress.Where(x => x.AddressId == change.AddressId).FirstOrDefault();

            getaddress.AddressLine1 = change.AddressLine1;
            getaddress.AddressLine2 = change.AddressLine2;
            getaddress.PostalCode = change.PostalCode;
            getaddress.City = change.City;
            getaddress.Mobile = change.Mobile;
            _DbContext.UserAddress.Update(getaddress);
            _DbContext.SaveChanges();
            return "true";

        }

        public IActionResult MainFS()
        {
            return View();
        }

        public IActionResult SettingsFS()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            SettingsModelEight st = new SettingsModelEight();
            st.Userr = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            st.UserAddress = _DbContext.UserAddress.Where(x => x.UserId == ty).FirstOrDefault();
            return View(st);
        }

        public IActionResult NotifyFS()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SettingsFS(SettingsModelEight sme)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            u.FirstName = sme.Userr.FirstName;
            u.LastName = sme.Userr.LastName;
            u.Mobile = sme.Userr.Mobile;
            u.DateOfBirth = sme.Userr.DateOfBirth;
            u.ZipCode = sme.Userr.ZipCode;
            u.Gender = sme.Userr.Gender;
            _DbContext.User.Update(u);
            _DbContext.SaveChanges();
            UserAddress ua = _DbContext.UserAddress.Where(x => x.UserId == ty).FirstOrDefault();
            if (ua != null)
            {
                ua.AddressLine1 = sme.UserAddress.AddressLine1;
                ua.AddressLine2 = sme.UserAddress.AddressLine2;
                ua.PostalCode = sme.UserAddress.PostalCode;
                ua.Mobile = sme.Userr.Mobile;
                ua.City = sme.UserAddress.City;
                ua.PostalCode = sme.Userr.ZipCode;
                _DbContext.UserAddress.Update(ua);
                _DbContext.SaveChanges();
            }
            else
            {
                UserAddress usa = new UserAddress();
                usa.UserId = ty;
                usa.AddressLine1 = sme.UserAddress.AddressLine1;
                usa.AddressLine2 = sme.UserAddress.AddressLine2;
                usa.PostalCode = sme.UserAddress.PostalCode;
                usa.Mobile = sme.Userr.Mobile;
                usa.City = sme.UserAddress.City;
                _DbContext.UserAddress.Add(usa);
                _DbContext.SaveChanges();
            }
            TempData["abc"] = "Data has been updated!";
            return View();
        }

        public string NewPwdEight([FromBody] User pass)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            if (u.Password == pass.Password)
            {
                u.Password = pass.NewPassword;
                _DbContext.User.Update(u);
                _DbContext.SaveChanges();
                return "true";
            }
            return "false";
        }

        public IActionResult BlockC()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            var query = (from user in _DbContext.User
                         join FavoriteAndBlocked in _DbContext.FavoriteAndBlocked
                         on user.UserId equals FavoriteAndBlocked.TargetUserId
                         where FavoriteAndBlocked.UserId == ty
                         select new Popup
                         {
                             Id = FavoriteAndBlocked.Id,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             IsBlocked = FavoriteAndBlocked.IsBlocked,
                             UserId = user.UserId
                         }).ToList();

            return View(query);

        }
        public IActionResult NewServiceFS()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join user in _DbContext.User on ServiceRequest.UserId equals user.UserId
                         where ServiceRequest.ZipCode == u.ZipCode && ServiceRequest.ServiceProviderId == null
                         select new NewServiceRequest
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceId = ServiceRequest.ServiceId,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             ZipCode = ServiceRequest.ZipCode,
                             SubTotal = ServiceRequest.SubTotal,
                             City = ServiceRequestAddress.City
                         }).ToList();
            return View(query);
        }

        public IActionResult UpcomingServiceFS()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            User u = _DbContext.User.Where(x => x.UserId == ty).FirstOrDefault();
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join user in _DbContext.User on ServiceRequest.UserId equals user.UserId
                         where ServiceRequest.ServiceProviderId == ty && ServiceRequest.Status == 2
                         select new NewServiceRequest
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceId = ServiceRequest.ServiceId,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             ZipCode = ServiceRequest.ZipCode,
                             SubTotal = ServiceRequest.SubTotal,
                             City = ServiceRequestAddress.City
                         }).ToList();
            return View(query);
        }

        public string eightAccept(int i)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            ServiceRequest srt = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == i).FirstOrDefault();
            srt.ServiceProviderId = ty;
            srt.SpacceptedDate = DateTime.Now;
            srt.Status = 2;
            _DbContext.ServiceRequest.Update(srt);
            _DbContext.SaveChanges();
            return "true";
        }

        public string eightCancel(int i)
        {
            ServiceRequest srt = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == i).FirstOrDefault();
            srt.ServiceProviderId = null;
            srt.SpacceptedDate = null;
            _DbContext.ServiceRequest.Update(srt);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult ServiceHistoryFS()
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            var query = from ServiceRequest in _DbContext.ServiceRequest
                        join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                        on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                        join user in _DbContext.User on ServiceRequest.UserId equals user.UserId
                        where ServiceRequest.ServiceProviderId == ty && ServiceRequest.Status == 1
                        select new Popup
                        {
                            ServiceRequestId = ServiceRequest.ServiceRequestId,
                            ServiceId = ServiceRequest.ServiceId,
                            ServiceHours = ServiceRequest.ServiceHours,
                            ServiceStartDate = ServiceRequest.ServiceStartDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Comments = ServiceRequest.Comments,
                            AddressLine1 = ServiceRequestAddress.AddressLine1,
                            AddressLine2 = ServiceRequestAddress.AddressLine2,
                            ZipCode = ServiceRequest.ZipCode,
                            SubTotal = ServiceRequest.SubTotal,
                            City = ServiceRequestAddress.City
                        };
            return View(query);
        }

        public IActionResult _upcomingPop(int i)
        {
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join user in _DbContext.User on ServiceRequest.UserId equals user.UserId
                         where ServiceRequest.ServiceRequestId == i
                         select new Popup
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceId = ServiceRequest.ServiceId,
                             ServiceHours = ServiceRequest.ServiceHours,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             Comments = ServiceRequest.Comments,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             ZipCode = ServiceRequest.ZipCode,
                             SubTotal = ServiceRequest.SubTotal,
                             City = ServiceRequestAddress.City
                         }).Single();

            return PartialView(query);
        }

        public string serviceComplete(int i)
        {
            ServiceRequest sr = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == i).FirstOrDefault();
            sr.Status = 1;
            _DbContext.ServiceRequest.Update(sr);
            _DbContext.SaveChanges();
            var ii = _DbContext.FavoriteAndBlocked.Where(x => x.UserId == sr.ServiceProviderId && x.TargetUserId == sr.UserId).FirstOrDefault();
            if (ii == null)
            {
                FavoriteAndBlocked sb = new FavoriteAndBlocked();
                sb.UserId = (int)sr.ServiceProviderId;
                sb.TargetUserId = sr.UserId;
                sb.IsBlocked = false;
                sb.IsFavorite = false;
                _DbContext.FavoriteAndBlocked.Add(sb);
                _DbContext.SaveChanges();
            }
            return "true";
        }

        public IActionResult _calendarPop(int i)
        {
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         join user in _DbContext.User on ServiceRequest.UserId equals user.UserId
                         where ServiceRequest.ServiceRequestId == i
                         select new Popup
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,
                             ServiceId = ServiceRequest.ServiceId,
                             ServiceHours = ServiceRequest.ServiceHours,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             Comments = ServiceRequest.Comments,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             ZipCode = ServiceRequest.ZipCode,
                             SubTotal = ServiceRequest.SubTotal,
                             City = ServiceRequestAddress.City
                         }).Single();

            return PartialView(query);
        }

        public IActionResult RatingFS(int i)
        {
            var ty = (int)HttpContext.Session.GetInt32("userid");
            var query = (from User in _DbContext.User
                         join Rating in _DbContext.Rating
                         on User.UserId equals Rating.RatingFrom
                         join ServiceRequest in _DbContext.ServiceRequest
                         on Rating.ServiceRequestId equals ServiceRequest.ServiceRequestId
                         where Rating.RatingTo == ty
                         select new Popup
                         {
                             ServiceId = ServiceRequest.ServiceId,
                             FirstName = User.FirstName,
                             LastName = User.LastName,
                             RatingDate = (DateTime)Rating.RatingDate,
                             Ratings = Rating.Ratings,
                             Comments = Rating.Comments
                         }).ToList();
            return View(query);
        }

        public IActionResult _star(int sid)
        {
            User u = _DbContext.User.Where(x => x.UserId == sid).FirstOrDefault();
            return PartialView(u);
        }

        public string starPopup([FromBody] Rating rate)
        {

            var a = (int)HttpContext.Session.GetInt32("userid");
            Rating r = _DbContext.Rating.Where(x => x.ServiceRequestId == rate.ServiceRequestId).FirstOrDefault();

            if (r != null)
            {
                r.Ratings = rate.Ratings;
                r.Comments = rate.Comments;
                r.Friendly = rate.Friendly;
                r.OnTimeArrival = rate.OnTimeArrival;
                r.QualityOfService = rate.QualityOfService;
                r.RatingDate = DateTime.Now;
                _DbContext.Rating.Update(r);
            }
            else
            {
                rate.RatingFrom = a;
                rate.RatingDate = DateTime.Now;
                _DbContext.Rating.Add(rate);
            }
            _DbContext.SaveChanges();
            return "true";
        }

        public int reschedulePop([FromBody] ServiceRequest book)
        {

            ServiceRequest detail = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == book.ServiceRequestId).FirstOrDefault();
            int result = DateTime.Compare(DateTime.Now, DateTime.Parse(book.Date));
            if (result == -1)
            {
                detail.ServiceStartDate = DateTime.Parse(book.Date);
                _DbContext.ServiceRequest.Update(detail);
                _DbContext.SaveChanges();
                return 1;
            }
            else if (result == 0)
            {
                return 0;
            }
            else if (result == 1)
            {
                return 0;
            }
            else
            {
                return 0;
            }
        }

        public IActionResult UserManagement()
        {
            List<User> u = _DbContext.User.ToList();
            return View(u);
        }

        public IActionResult ServiceRequest()
        {

            var query = (from sr in _DbContext.ServiceRequest
                         join sra in _DbContext.ServiceRequestAddress
                         on sr.ServiceRequestId equals sra.ServiceRequestId
                         join rt in _DbContext.Rating
                         on sr.ServiceRequestId equals rt.ServiceRequestId
                         into rate
                         from rt in rate.DefaultIfEmpty()
                         join ur in _DbContext.User
                         on sr.UserId equals ur.UserId
                         select new Popup
                         {
                             ServiceId = sr.ServiceId,
                             ServiceRequestId = sr.ServiceRequestId,
                             ServiceStartDate = sr.ServiceStartDate,
                             AddressLine1 = sra.AddressLine1,
                             AddressLine2 = sra.AddressLine2,
                             Ratings = rt == null ? 0 : rt.Ratings,
                             FirstName = ur.FirstName,
                             LastName = ur.LastName,
                             Status = sr.Status
                         }).ToList();
            return View(query);
        }

        public IActionResult deactivate(int id)
        {
            User u = _DbContext.User.Where(x => x.UserId == id).FirstOrDefault();
            u.IsActive = false;
            _DbContext.User.Update(u);
            _DbContext.SaveChanges();
            return RedirectToAction("UserManagement");

        }

        public IActionResult activate(int id)
        {
            User u = _DbContext.User.Where(x => x.UserId == id).FirstOrDefault();
            u.IsActive = true;
            _DbContext.User.Update(u);
            _DbContext.SaveChanges();
            return RedirectToAction("UserManagement");

        }

        public IActionResult _editreschedule(int id)
        {
            var query = (from ServiceRequest in _DbContext.ServiceRequest
                         join ServiceRequestAddress in _DbContext.ServiceRequestAddress
                         on ServiceRequest.ServiceRequestId equals ServiceRequestAddress.ServiceRequestId
                         where ServiceRequest.ServiceRequestId == id
                         select new Popup
                         {
                             ServiceRequestId = ServiceRequest.ServiceRequestId,

                             ServiceHours = ServiceRequest.ServiceHours,
                             ServiceStartDate = ServiceRequest.ServiceStartDate,

                             Comments = ServiceRequest.Comments,
                             AddressLine1 = ServiceRequestAddress.AddressLine1,
                             AddressLine2 = ServiceRequestAddress.AddressLine2,
                             ZipCode = ServiceRequest.ZipCode,

                             City = ServiceRequestAddress.City
                         }).Single();
            return PartialView(query);
        }

        public int editresUpdate([FromBody] Popup y)
        {
            ServiceRequest sr = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == y.ServiceRequestId).FirstOrDefault();
            sr.ServiceStartDate = y.Date;
            sr.Comments = y.Comments;
            sr.ZipCode = y.PostalCode;
            _DbContext.ServiceRequest.Update(sr);
            _DbContext.SaveChanges();

            ServiceRequestAddress srd = _DbContext.ServiceRequestAddress.Where(x => x.ServiceRequestId == y.ServiceRequestId).FirstOrDefault();
            srd.AddressLine1 = y.AddressLine1;
            srd.AddressLine2 = y.AddressLine2;
            srd.City = y.City;
            srd.PostalCode = y.PostalCode;

            _DbContext.ServiceRequestAddress.Update(srd);
            _DbContext.SaveChanges();
            return 1;

        }

        public IActionResult _refundPop(int id)
        {
            ServiceRequest sr = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == id).FirstOrDefault();
            return PartialView(sr);
        }

        public string refundUpdateChanges([FromBody] ServiceRequest book)
        {
            ServiceRequest sr = _DbContext.ServiceRequest.Where(x => x.ServiceRequestId == book.ServiceRequestId).FirstOrDefault();
            sr.Comments = book.Comments;
            sr.RefundedAmount = book.RefundedAmount;
            _DbContext.ServiceRequest.Update(sr);
            _DbContext.SaveChanges();
            return "true";
        }

        public IActionResult ServiceSchedule()
        {
            return View();
        }

    }
}
  