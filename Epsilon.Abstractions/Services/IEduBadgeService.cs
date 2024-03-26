﻿namespace Epsilon.Abstractions.Services;

public interface IEduBadgeService
{
    IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> GetData(IEnumerable<string> userIds, DateTime from, DateTime to);

    void WriteDocument(Stream stream, IEnumerable<IAsyncEnumerable<LearningDomainSubmission>> data);
}