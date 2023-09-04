using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicLearn.Core.Security
{
    public static class ImageValidator
    {
        // Checks the file is type of image or not
        public static bool IsImage(this IFormFile file)
        {
			try
			{
				var image = Image.FromStream(file.OpenReadStream());
				return true;
			}
			catch
			{
				return false;
			}
        }
    }
}
