﻿using Gabay_Final_V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Gabay_Final_V2.Views.DashBoard.Guest_Homepage
{
    public partial class Guest_Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This is to disable caching for the dashboard to prevent backward login
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (Session["GuestName"] != null)
            {
                guestNameBx.Text = "Hello! " + Session["GuestName"].ToString();
            }
            else
            {
                Response.Redirect("~/Views/Loginpages/Guest_login.aspx");
            }
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Loginpages/Guest_login.aspx");
        }

    }
}