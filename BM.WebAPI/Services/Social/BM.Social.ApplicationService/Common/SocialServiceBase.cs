using BM.Social.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Social.ApplicationService.Common
{
    public class SocialServiceBase
    {
        protected readonly ILogger _logger;
        protected readonly SocialDbContext _dbContext;
        protected SocialServiceBase(ILogger logger, SocialDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }
}
