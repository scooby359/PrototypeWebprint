using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace Prototype_Webprint_2
{
    public class PuppetService
    {
        public static async Task GetPage()
        {
            // Download the Chromium revision if it does not already exist
            var browserFetcher = new BrowserFetcher();
            var installedBrowser = await browserFetcher.DownloadAsync();

            // Launch the browser
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                ExecutablePath = installedBrowser.GetExecutablePath(),
                Args = ["--disable-web-security"] // Get CORS errors without this
            });

            // Open a new page
            await using var page = await browser.NewPageAsync();

            // Pass browser console events to the dotnet console for debugging
            page.Console += (sender, e) => Console.WriteLine($"Console log: {e.Message.Text}");

            // URL of the service this is hosted on
            var serviceUrl = "https://localhost:7205"; // Replace with the actual URL

            // Navigate to the test website
            await page.GoToAsync(serviceUrl);

            // Save the page as a PDF
            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4
            };
            await page.PdfAsync("vite.pdf", pdfOptions);

            Console.WriteLine("PDF saved as example.pdf");
        }

    }
}
