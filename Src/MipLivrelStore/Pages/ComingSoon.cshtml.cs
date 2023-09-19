using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MipLivrelStore.Pages
{
	public class ComingSoonModel : PageModel
	{
		private readonly ILogger<PrivacyModel> _logger;

		public ComingSoonModel(ILogger<PrivacyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
		}
	}
}
