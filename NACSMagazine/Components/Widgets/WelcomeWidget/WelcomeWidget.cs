//using Azure;

//using CMS.ContactManagement;
//using CMS.EventLog;
//using CMS.Helpers;
//using CMS.Membership;

//using Kentico.PageBuilder.Web.Mvc;

//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//using NACSMagazine.Components.Widgets.WelcomeWidget;
//using System.Text;
//using System.Web;

//using CMS.Websites;
//using System.Data;
//using static NACSMagazine.Components.Widgets.WelcomeWidget.WelcomeWidgetViewComponent;
//using Kentico.Membership;
//using Microsoft.AspNetCore.Identity;
//using System.Security.Claims;
//using System.Security.Principal;

//[assembly: RegisterWidget(
//    identifier: "NACSMagazine.WelcomeWidget",
//    viewComponentType: typeof(WelcomeWidgetViewComponent),
//    name: "Welcome widget",
//    propertiesType: typeof(WelcomeWidgetProperties))]

//namespace NACSMagazine.Components.Widgets.WelcomeWidget
//{

//    public class WelcomeWidgetViewComponent : ViewComponent
//    {
//        private readonly IWebPageUrlRetriever? webPageUrlRetriever;
//        private readonly IHttpContextAccessor httpContextAccessor;
//        private readonly UserManager<ApplicationUser> userManager;

//        public const string CONTENT_TYPE_NAME = "NACSMagazine.WelcomeWidget";

//        public string subdomain = "staging";
//        public string currentUrl = "";
//        public string mxsite = "https://nacsstagednn1.pcbscloud.com";
//        public string mysplanner = "https://nacs22.mysstaging.com/8_0/login/login.cfm";
//        public string wisepops_submittedforms = "";
//        public string wisepops_loggedinuser = "";
//        public string wisepops_roles = "";
//        public string wisepops_euronewseligible = "false";
//        public bool loggedin = false;
//        public string ContactID = "";
//        public string LoginDomain = "";

//        public WelcomeWidgetViewComponent(IWebPageUrlRetriever? _webPageUrlRetriever, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager)
//        {
//            webPageUrlRetriever = _webPageUrlRetriever;
//            httpContextAccessor = _httpContextAccessor;
//            userManager = _userManager;
//        }

//        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<WelcomeWidgetProperties> properties, IWebPageUrlRetriever webPageUrlRetriever, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
//        {
//            UserInfo currentUser = MembershipContext.AuthenticatedUser;
//            var currentUrlPath = await webPageUrlRetriever!.Retrieve(properties.Page.WebPageItemID, "en");
//            var currentURL = currentUrlPath.RelativePath;

//            if (currentURL != null)
//            {
//                if (currentURL.ToString().ToLower().Contains("staging"))
//                {
//                    subdomain = "staging";
//                    mxsite = "https://nacsstagednn1.pcbscloud.com";
//                    mysplanner = "https://nacs22.mysstaging.com/8_0/login/login.cfm";
//                }
//                else if (currentURL.ToLower().Contains("kentico") || currentURL.ToLower().Contains("dev"))
//                {
//                    subdomain = "kentico";
//                    mxsite = "https://nacsstagednn1.pcbscloud.com";
//                    mysplanner = "https://nacs22.mysstaging.com/8_0/login/login.cfm";
//                }
//                else
//                {
//                    subdomain = "www";
//                    mxsite = "https://mynacs.convenience.org";
//                    mysplanner = "https://nacs22.mapyourshow.com/8_0/login/login.cfm";
//                }
//            }
            
//            var user = httpContextAccessor.HttpContext.User.Identity;
            
//            if (user != null && user.IsAuthenticated)
//            {
//                loggedin = true;

//                //ContactID = this.CustomerKey.ToLower();

//                string name = currentUser.FullName;

//                if (string.IsNullOrEmpty(name))
//                {
//                    name = (currentUser.FirstName + " " + currentUser.LastName).Trim();
//                }
//                if (string.IsNullOrEmpty(name))
//                {
//                    name = currentUser.UserName;
//                }

//                //if impersonating or admin, show refresh profile option
//                //removing check for CurrentUserIsImpersonated() and also checking for Editor level privileges since that functionality is no longer available in XbK
//                if (currentUser.HasAdministrationAccess())
//                {
//                    _lblLastUpdatedTab.Text = "Profile last updated:<br/><strong>" + currentUser.UserLastModified.AddHours(1).ToString("MMM d, h:mm tt") + " EST</strong>";
//                    _updSyncUser.Visible = true;
//                }
//                else
//                {
//                    _updSyncUser.Visible = false;
//                }

//                string mxtoken = GetProtechMXToken(currentUser.UserName);
//                string mxtokenQS = (mxtoken != "") ? "?token=" + mxtoken : "";

//                _hlProfileMX.HRef = mxsite + "/My-Account/My-Profile" + mxtokenQS;
//                _hlSubscriptionsMX.HRef = mxsite + "/My-Account/Subscriptions" + mxtokenQS;
//                _hlEmailPreferencesMX.HRef = mxsite + "/My-Account/Email-Preferences" + mxtokenQS;
//                _hlPurchasesMX.HRef = "https://" + subdomain + ".convenience.org/AccountAdmin/MyDigitalContent";

//                litRoles.Text = "<div>" + GetDisplayMenu(currentUser) + "</div>"; //also populates MyLinks
//                litWelcome.Text = name;

//                hypLogout.Attributes["href"] += "?Source=" + currentURL;

//                //#region GA Data Layer Call
//                //if (Request.UrlReferrer != null)
//                //{
//                //    if (Request.UrlReferrer.Host.ToString().Contains("login."))
//                //    {
//                //        if (SiteContext.CurrentSiteName.Equals("NACSMagazine"))
//                //        {
//                //            LoginDomain = "nacsmagazine.com";
//                //        }

//                //        phJustLoggedIn.Visible = true;

//                //    }
//                //    else
//                //    {
//                //        phJustLoggedIn.Visible = false;
//                //    }
//                //}
//                //else
//                //{
//                //    phJustLoggedIn.Visible = false;
//                //}
//                //#endregion

//            }
//            else
//            {
//                loggedin = false;
//                phAnonymous.Visible = true;
//                phAuthenticated.Visible = false;

//                //Keep QS parameters, but remove the aliaspath one.
//                //BSM 8/4/2021
//                string qry = HttpContext.Request.Url.Query;
//                int index = qry.IndexOf("aliaspath");
//                if (index >= 0)
//                    qry = qry.Substring(0, index - 1); //remove the default mysterious ?aliaspath (inserted by Kentico?) from end of QS. Then proceed.

//                hypLogin.Attributes["href"] += "?Source=" + currentURL + HttpUtility.UrlEncode(qry); //keep qs params - BSM 8/4/2021

//            }

//            TimeSpan lagTime = new TimeSpan(24, 0, 0);

//            //Call ensure user
//            #region Post-login Update of User Record
//            //If user is logged in, and either coming from the IDP or their profile has not been updated in 24 hours, call EnsureUser to update their record and then update the logon timestamp
//            if (AuthenticationHelper.IsAuthenticated() &&
//                (!string.IsNullOrEmpty(Request.QueryString["LoginSource"])
//                || (DateTime.Now - CMS.Membership.MembershipContext.AuthenticatedUser.UserLastModified).TotalMinutes > lagTime.TotalMinutes
//                || (Request.QueryString["ensureuser"] != null && Request.QueryString["ensureuser"].ToString() == "true")
//                ))
//            {


//                //BEGIN PROTECH API CALL (hide if API is having issues)-----------------------
//                string status = EnsureUser(currentUser.UserName, true);
//                MembershipActivityLogger.LogLogin(currentUser.UserName, DocumentContext.CurrentDocument);
//                //END PROTECH API CALL ----------------------------------------------

//                // If coming from IDP, record the logon timestamp, and redirect to location
//                if (Request.QueryString["LoginSource"] != null)
//                {
//                    AuthenticationHelper.UpdateLastLogonInformation(user);
//                    UserInfoProvider.SetUserInfo(user);
//                    Response.Redirect(Request.QueryString["LoginSource"]);
//                }

//                litWelcome.Text += "&nbsp;<i class='fa fa-check-circle' title='Profile Just Updated!'></i>";
//            }

//            #endregion

//            //Get and expose properties for Wisepops
//            #region Wisepops Properties

//            if (CookieHelper.GetExistingCookie("CMSSubmittedWebForms") != null)
//            {
//                wisepops_submittedforms = CookieHelper.GetValue("CMSSubmittedWebForms");
//            }

//            if (loggedin)
//            {
//                string roles = GetRoleNames(currentUser);

//                wisepops_loggedinuser = "true";

//                wisepops_roles = roles;

//                if (roles.Contains("retail member") || roles.Contains("global supplier council member"))
//                {
//                    wisepops_euronewseligible = "true";
//                }
//                else
//                {
//                    wisepops_euronewseligible = "false";
//                }
//            }
//            else
//            {
//                wisepops_euronewseligible = "true";
//            }

//            #endregion

//            var model = new WelcomeWidgetViewModel();

//            return View("~/Components/Widgets/WelcomeWidget/WelcomeWidget.cshtml", model);
//        }


//        public class WelcomeWidgetProperties : IWidgetProperties
//        {

//        }

//        private string GetDisplayMenu(CurrentUserInfo user)
//        {
//            string[] roles = user.GetRoleIdList(true, false, CurrentSiteName).Split(',');
//            StringBuilder sb = new StringBuilder();
//            StringBuilder sbMyStuffLinks = new StringBuilder();
//            int cnt_stuff = 0;

//            sb.Append("<!--<ul class='fa-ul'>\n");

//            List<MyLink> mylinks = new List<MyLink>();
//            List<MyLink> mylinks_deduped = new List<MyLink>();
//            List<MyLink> mylinks_sorted = new List<MyLink>();

//            //add links into list
//            foreach (var roleid in roles)
//            {
//                try
//                {
//                    string strWhere = "'|' + [CorrespondingRoles] + '|' like '%|" + roleid + "|%'";

//                    //find if user is in a role that is a community forum
//                    var mylink = CustomTableItem.Provider.GetItems("Convenience.MyStuff")
//                            .Where(strWhere)
//                            .FirstOrDefault();

//                    if (mylink != null)
//                    {
//                        string icon = "fa-caret-right";
//                        string type = "basic";
//                        string iconHTML = "";

//                        if (mylink.GetBooleanValue("IsCommunityForum", false) == true)
//                        {
//                            icon = "fa-comments";
//                            type = "community";
//                        }

//                        if (mylink.GetBooleanValue("IsMemberOnly", false) == true)
//                        {
//                            icon = "fa-unlock-alt";
//                            type = "memberonly";
//                        }

//                        if (mylink.GetBooleanValue("IsVirtualEvent", false) == true)
//                        {
//                            icon = "fa-tv";
//                            type = "virtualevent";
//                        }

//                        if (mylink.GetBooleanValue("Disabled", false) == false)
//                        {
//                            mylinks.Add(new MyLink
//                            {
//                                LinkType = type,
//                                LinkTypeIcon = icon,
//                                LinkTypeIconHTML = iconHTML,
//                                LinkURL = mylink.GetStringValue("URL", ""),
//                                LinkName = mylink.GetStringValue("Descriptor", "")
//                            });
//                            cnt_stuff++;
//                        }
//                    }
//                }
//                catch { }
//            }

//            //De-dupe list
//            mylinks_deduped = mylinks.Distinct(new MyLinkComparer()).ToList(); //de-dupe on a single column
//                                                                               //mylinks_deduped = mylinks.Distinct().ToList();

//            //Sort by Created Date
//            mylinks_sorted = mylinks_deduped.OrderBy(x => x.LinkType).ToList().OrderBy(x => x.LinkName).ToList();


//            if (mylinks_sorted.Count > 0)
//            {
//                sbMyStuffLinks.Append("<div style='padding-left:20px;'>\n");

//                foreach (var link in mylinks_sorted)
//                {
//                    sbMyStuffLinks.Append("<a href='" + link.LinkURL + "'>");
//                    if (link.LinkTypeIcon == "HTML")
//                    {
//                        sbMyStuffLinks.Append(link.LinkTypeIconHTML + link.LinkName);
//                    }
//                    else
//                    {
//                        sbMyStuffLinks.Append("<i class='far " + link.LinkTypeIcon + "'></i>" + link.LinkName);
//                    }
//                    sbMyStuffLinks.Append("</a><br/>\n");
//                }

//                sbMyStuffLinks.Append("</div>\n");

//                _divMyLinks.Controls.Add(new LiteralControl(sbMyStuffLinks.ToString()));
//                _divMyLinks.Visible = true;
//            }
//            else
//            {
//                _divMyLinks.Visible = false;
//            }



//            sb.Append("</ul>-->\n");


//            return sb.ToString();
//        }

//        private string GetRoleNames(CurrentUserInfo user)
//        {
//            string[] roles = user.GetRoleIdList(true, false, CurrentSiteName).Split(',');
//            StringBuilder sb = new StringBuilder();

//            //add links into list
//            foreach (var roleid in roles)
//            {
//                try
//                {
//                    //ROLE INFO
//                    RoleInfo info = RoleInfoProvider.GetRoleInfo(Convert.ToInt32(roleid));
//                    if (!info.RoleDisplayName.Contains("Authenticated") && !info.RoleDisplayName.Contains("Everyone"))
//                        sb.Append(info.RoleDisplayName.ToLower() + "|");
//                }
//                catch { }
//            }

//            return sb.ToString().TrimEnd('|');
//        }

//        private class MyLink
//        {
//            public string LinkType { get; set; }

//            public string LinkTypeIcon { get; set; }

//            public string LinkTypeIconHTML { get; set; }

//            public string LinkURL { get; set; }

//            public string LinkName { get; set; }

//        }

//        class MyLinkComparer : IEqualityComparer<MyLink>
//        {
//            //removes duplicates in any list based on the organization key
//            public bool Equals(MyLink x, MyLink y)
//            {
//                return x.LinkName == y.LinkName;
//            }

//            public int GetHashCode(MyLink obj)
//            {
//                return obj.LinkName.GetHashCode();
//            }
//        }

//        private string GetProtechMXToken(string ProtechNumber)
//        {
//            var task = Task.Run(() =>
//            {
//                return GetProtechMXTokenFromAPI(ProtechNumber);
//            });

//            bool isCompletedSuccessfully = task.Wait(TimeSpan.FromMilliseconds(5000));

//            if (isCompletedSuccessfully)
//            {
//                return task.Result;
//            }
//            else
//            {
//                return "";
//            }
//        }

//        private string GetProtechMXTokenFromAPI(string ProtechNumber)
//        {
//            NACSAPIAuthenticationSoapClient authService = new NACSAPIAuthenticationSoapClient();
//            string mxtoken = "";

//            //BEGIN PROTECH API CALL (hide if API is having issues)-----------------------
//            try
//            {
//                var savedtoken = SessionHelper.GetValue("NACSMXToken") as string;

//                if (savedtoken != null)
//                {
//                    mxtoken = savedtoken;
//                }
//                else
//                {
//                    //Get new token from API
//                    NACS.Helper.AuthService.NACSUser serviceUser = authService.AuthProvider_GetUserByID(ProtechNumber, ConfigurationManager.AppSettings["NACSAPIKey"]);
//                    mxtoken = serviceUser.Token.ToString();

//                    SessionHelper.SetValue("NACSMXToken", mxtoken);
//                }
//            }
//            catch (Exception ex)
//            {
//                var eventLogInfo = new EventLogInfo();
//                eventLogInfo.EventDescription = "Welcome Control MX Auth Token";
//                eventLogInfo.EventCode = "ERROR GETTING TOKEN";
                
//                EventLogProvider.LogEvent(eventLogInfo);
//                //EventLogProvider.LogEvent("Welcome Control MX Auth Token", "ERROR GETTING TOKEN", ex);
//            }
//            //END PROTECH API CALL ----------------------------------------------

//            return mxtoken;
//        }
//    }
//}