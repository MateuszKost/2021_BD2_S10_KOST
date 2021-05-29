using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCollection.Client.Shared
{
    public partial class AuthLayout
    {

        private ElementReference signInTab;
        private ElementReference signUpTab;

        private string SignInTabClass;
        private string SignUpTabClass;

        private string currentLocation;

        protected override void OnInitialized()
        {
            currentLocation = NavigationManager.Uri;
            ChangeTabsStates(currentLocation);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            NavigationManager.LocationChanged += (obj, lcea) =>
            {
                currentLocation = lcea.Location;

                ChangeTabsStates(currentLocation);
                ShouldRender();
            };
        }

        private void SetActiveTab(ElementReference element)
        {
            if (element.Equals(signInTab))
            {
                SignInTabClass = "active";
                SignUpTabClass = null;
            }
            else if (element.Equals(signUpTab))
            {
                SignInTabClass = null;
                SignUpTabClass = "active";
            }
        }

        private void ChangeTabsStates(string currentLocation)
        {
            if (currentLocation.Contains("/login"))
            {
                SignInTabClass = "active";
                SignUpTabClass = null;
            }
            else if (currentLocation.Contains("/register"))
            {
                SignInTabClass = null;
                SignUpTabClass = "active";
            }
        }


    }
}
