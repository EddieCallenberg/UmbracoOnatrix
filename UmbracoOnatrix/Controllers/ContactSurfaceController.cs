using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using UmbracoOnatrix.Models;

namespace UmbracoOnatrix.Controllers
{
	public class ContactSurfaceController : SurfaceController
	{
		public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
		{
		}

		public IActionResult HandleSubmit(ContactFormModel form)
		{
			if (!ModelState.IsValid)
			{
				ViewData["name"] = form.Name;
				ViewData["email"] = form.Email;
				ViewData["message"] = form.Message;

				ViewData["error_name"] = !string.IsNullOrEmpty(form.Name);
				ViewData["error_email"] = !string.IsNullOrEmpty(form.Email);
				
				return CurrentUmbracoPage();
			}

			TempData["success"] = "form submitted successfully";
			SendEmail(form.Email, "Det funkar!");
			return RedirectToCurrentUmbracoPage();
		}

		private void SendEmail(string recipientEmail, string messageBody)
		{
			var smtpClient = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				Credentials = new NetworkCredential("onatrixx@gmail.com", "bxwq vwvs unix iuzy"),
				EnableSsl = true
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress("onatrixx@gmail.com"),
				Subject = "Formulärbekräftelse",
				Body = messageBody,
				IsBodyHtml = false
			};

			mailMessage.To.Add(recipientEmail);

			smtpClient.Send(mailMessage);
		}
	}

}

