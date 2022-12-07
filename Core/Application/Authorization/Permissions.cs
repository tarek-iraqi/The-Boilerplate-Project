namespace Application.Authorization;

public enum Permissions
{
    ViewUsers = 1
}

public static class PermissionsExtensions
{
    public static string ToStringNumber(this Permissions permission)
    {
        return ((int)permission).ToString();
    }
}