﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Data.Services.Interfaces
{
    public interface IImageService
    {
        Task<UploadResult> UploadAsync(IFormFile image);
    }
}
