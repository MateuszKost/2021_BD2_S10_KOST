namespace SmartCollection.Client.Shared
{
    public partial class UnauthorizedUser
    {
        private void HandleNavigation(string target)
        {
            if (target.Equals("login"))
                NavigationManager.NavigateTo("/login", false);
            else
                NavigationManager.NavigateTo("/register", false);
        }
    }
}
