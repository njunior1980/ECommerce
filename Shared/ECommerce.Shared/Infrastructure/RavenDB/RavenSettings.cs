namespace ECommerce.Shared.Infrastructure.RavenDB;

public class RavenSettings
{
    public string[] Urls { get; set; }
    public string Certificate { get; set; }
    public string Password { get; set; }
    public string[] Databases { get; set; }

    public static string SectionName = "RavenSettings";
}