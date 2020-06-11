﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Data.Models;
using Catalog.API.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UnitOfWork;

namespace Catalog.API.BackgroundServices
{
    public class ApproveReviewBackgroundService : BaseBackgroundService
    {

        public ApproveReviewBackgroundService(IServiceProvider services) : base(services)
        {
        }

        public override string BackgroundName { get; } = "Approve reviews job";
        public override TimeSpan DelayTime { get; } = TimeSpan.FromMinutes(1);

        public override async Task ProcessAsync(CancellationToken cancellationToken)
        {
            using var scope = ServiceProvider.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApproveReviewBackgroundService>>();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var utcNow = DateTime.UtcNow;

            var reviews = await unitOfWork.Repository<Review>()
                .Query()
                .Where(s => s.ReviewStatus == ReviewStatus.Pending && s.CreatedDate.AddMinutes(5) <= utcNow)
                .ToListAsync(cancellationToken);

            foreach (var review in reviews)
            {
                review.ReviewStatus = ReviewStatus.Approved;
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Approved {count} reviews with Id: {reviews}", reviews.Count, reviews.Select(s => s.Id));
        }
    }
}