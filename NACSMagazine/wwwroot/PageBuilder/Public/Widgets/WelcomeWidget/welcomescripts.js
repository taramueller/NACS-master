// <script type="text/javascript">
//    wisepops("properties", {
//        FormsSubmitted: "<%=wisepops_submittedforms%>",
//        LoggedInUser: "<%=wisepops_loggedinuser%>",
//        CMSRoles: "<%=wisepops_roles%>",
//        EuroNewsEligible: "<%=wisepops_euronewseligible%>"
//    }, true);
//</script> 

// <script type="text/javascript">
//    // GA4 DataLayer Push
//    dataLayer.push({
//        'event': 'login',
//        'contactid': '<%=ContactID%>',
//        'login_domain': '<%=LoginDomain%>'
//    });
//</script> 


    {/* When the user clicks on the button,*/}
    {/*toggle between hiding and showing the dropdown content */}

    function mystuffmenu() {
        document.getElementById("<%=this.ClientID%>__divMyStuffLinks").classList.toggle("show")
    };


    {/* Close the dropdown if the user clicks outside of it*/}
    window.onclick = function (event) {
        if (!event.target.matches('.dropbtn') && !event.target.matches('#<%=this.ClientID%>__lbSync')) {
            var dropdowns = document.getElementsByClassName("dropdown-content");
            var i;
            for (i = 0; i < dropdowns.length; i++) {
                var openDropdown = dropdowns[i];
                if (openDropdown.classList.contains('show')) {
                    openDropdown.classList.remove('show')
                };
            }
        }
    }

    function DisableButtonOnSync(obj) {

        var button = document.getElementById(obj.id);

        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            openDropdown.classList.add('show');
        }

        button.innerHTML = "<i class='fas fa-cog fa-spin'></i></span>&nbsp;Syncing...";
        button.className = "sync-tab-button disabled";
        button.disabled = true;

    }
