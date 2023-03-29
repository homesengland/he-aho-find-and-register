using Microsoft.Extensions.Configuration;

public class BetaFlag{

    private static IConfiguration conf = (new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build());
    public static readonly bool ShowBetaBanner = conf.GetValue<bool>("ShowBetaBanner");
   
}