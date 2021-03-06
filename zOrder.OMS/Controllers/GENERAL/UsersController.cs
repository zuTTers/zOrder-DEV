﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;
using zOrder.OMS.Helper;
using zOrder.OMS.Models;

namespace zOrder.OMS.Controllers
{
    public class UsersController : ApiController
    {
        // LIST: api/Product

        [HttpGet, HttpPost]
        [ActionName("List")]
        public JsonResult<ReturnValue> List()
        {
            ReturnValue ret = new ReturnValue();

            try
            {
                ret.success = false;
                bool isExport = false;

                int rowCount = 0;

                using (var db = new zOrderEntities())
                {
                    var dataQuery = db.Users.OrderBy(x => x.User_Id).Where(x => 1 == 1);

                    #region "Filter"
                    //if (filter != null)
                    //{
                    //    if (!string.IsNullOrEmpty(filter.Name)) dataQuery = dataQuery.Where(x => x.Name.Contains(filter.Name));
                    //    if (filter.IsActive.HasValue) dataQuery = dataQuery.Where(x=>x.IsActive == filter.IsActive.Value);
                    //}
                    #endregion

                    rowCount = dataQuery.Count();

                    var data = dataQuery.ToList().Select(x =>
                        new
                        {
                            User_Id = x.User_Id,
                            Name = x.Name,
                            Gender = x.Gender,
                            Birthday = x.Birthday,
                            Mail = x.Mail,
                            UserType = x.UserType,
                            IsActive = x.IsActive,
                            IsDeleted = x.IsDeleted,
                            Password = x.Password
                        }).ToList();

                    if (!isExport)
                    {
                        ret.retObject = data;
                    }
                    else
                    {
                        //export excel format
                    }
                }
                ret.success = true;
                ret.message = "Listelendi";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }

        //[Route("api/Product/Save/{product}")]
        [HttpGet, HttpPost]
        [ActionName("Save")]
        public JsonResult<ReturnValue> Save(Users user)
        {
            ReturnValue ret = new ReturnValue();
            try
            {
                ret.success = false;
                //using (TransactionScope ts = TransactionUtils.CreateTransactionScope())
                //{
                using (zOrderEntities db = new zOrderEntities())
                {

                    Users nd = null;
                    if (user.User_Id.Equals(0))
                    {
                        nd = new Users();
                        //nd.Created_User = usr.uid;
                        //nd.Created_Date = DateTime.Now;
                    }
                    else
                    {
                        var rd = db.Users.Where(x => x.User_Id.Equals(user.User_Id)).ToList();
                        if (rd.Count > 0) nd = rd.First();
                    }
                    //nd.Updated_Date = DateTime.Now;
                    //nd.Updated_User = usr.uid;

                    nd.User_Id = user.User_Id;
                    nd.Name = user.Name;
                    nd.Gender = user.Gender;
                    nd.Birthday = user.Birthday;
                    nd.Mail = user.Mail;
                    nd.UserType = user.UserType;
                    nd.IsActive = user.IsActive;
                    nd.IsDeleted = user.IsDeleted;
                    nd.Password = user.Password;

                    if (user.User_Id.Equals(0))
                        db.Users.Add(nd);

                    db.SaveChanges();
                    ret.message = "Kaydedildi";
                    ret.success = true;
                }
                //ts.Complete();
                //}
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;

            }
            return Json(ret);
        }

        //[Route("api/Product/Delete/{id}")]
        [HttpGet, HttpPost]
        [ActionName("Delete")]
        public JsonResult<ReturnValue> Delete(int id)
        {
            ReturnValue ret = new ReturnValue();
            try
            {
                ret.success = false;
                using (zOrderEntities db = new zOrderEntities())
                {
                    var at = db.Users.Where(x => x.User_Id.Equals(id)).ToList();
                    if (at.Count > 0) db.Users.Remove(at.First());
                    db.SaveChanges();

                    ret.message = "Silindi";
                    ret.success = true;
                }
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }
            return Json(ret);
        }
    }
}