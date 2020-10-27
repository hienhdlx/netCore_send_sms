using System;
namespace SpeedSMSAPI
{
    class Program
    {
        

        static void Main(string[] args)
        {
            
            SpeedSMSAPI api = new SpeedSMSAPI("S2ejUPVthW3rN4AwwMuHeRRYDjGxXH9W");
			
			String phones = "0342461010" ;
            String str = "5558";
            String response = api.VerifyCode(phones, str);
			Console.WriteLine(response);
			Console.ReadLine();
        }
    }
}
