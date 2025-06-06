﻿using BM.Constant;
using BM.Social.Domain;
using BM.Social.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.SocialModule.Abtracts
{
    public interface ISocialBussiness
    {
        Task<ResponeDto> CreateBlogAsync(SocicalCreateBlogBussiness socicalCreateBlogBussiness);
        Task<ResponeDto> GetBlogAsync(int blogId);

    }
}
