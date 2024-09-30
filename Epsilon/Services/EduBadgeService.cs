﻿using System.Collections.ObjectModel;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Services;

namespace Epsilon.Services;

public class EduBadgeService : IEduBadgeService
{
    private readonly ILearningOutcomeCanvasResultService _canvasResultService;

    public EduBadgeService(ILearningOutcomeCanvasResultService canvasResultService)
    {
        _canvasResultService = canvasResultService;
    }

    public async Task<List<LearningDomainSubmission>> GetData(Collection<string> userIds, DateTime from, DateTime to)
    {
        return await _canvasResultService.GetSubmissions(userIds.First())
                                         .Where(static e => e.Criteria.Any())
                                         .ToListAsync();
    }

    public void WriteDocument(Stream stream, IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> data)
    {
        throw new NotImplementedException();
    }
}