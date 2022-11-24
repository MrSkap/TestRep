﻿using HistoryRepositoryDB;
using MongoDB.Driver;
using ServiseEntities;

namespace Services;

public class RepositoriesWorker : IRepositoriesWorker
{
    private readonly ILastServiceStatusRepository _lastServiceStatusRepository;
    private readonly IHistoryRepository _historyRepository;
    private readonly IMongoClient _client;

    public RepositoriesWorker(ILastServiceStatusRepository lastServiceStatusRepository, IHistoryRepository historyRepository, IMongoClient client)
    {
        _lastServiceStatusRepository = lastServiceStatusRepository;
        _historyRepository = historyRepository;
        _client = client;
    }

    public async Task<List<ServiceStatus>> GetHistory(string serviceName, int offset, int take)
        => await _historyRepository.GetServiceStatuses(serviceName, offset, take);

    public async Task AddHistory(List<ServiceStatus> history)
    {
        if (history.Count == 0)
        {
            return;
        }

        using IClientSessionHandle? session = await _client.StartSessionAsync();
        session.StartTransaction();
        Task[] tasks =
        {
            _historyRepository.SetOrAddServiceStatuses(history),
            _lastServiceStatusRepository.SetServiceStatus(
                history.OrderByDescending(el => el.TimeOfStatusUpdate).First()),
        };
        await Task.WhenAll(tasks);
        await session.CommitTransactionAsync();
    }

    public async Task AddServiceStatus(ServiceStatus status)
    {
        using IClientSessionHandle? session = await _client.StartSessionAsync();
        session.StartTransaction();
        Task[] tasks =
        {
            _historyRepository.SetStatus(status),
            _lastServiceStatusRepository.SetServiceStatus(status),
        };
        await Task.WhenAll(tasks);
        await session.CommitTransactionAsync();
    }

    public async Task<List<ServiceStatus>> GetLastServicesStatus()
        => await _lastServiceStatusRepository.GetAllServicesStatus();

    public async Task<ServiceStatus?> GetServiceStatus(string serviceName)
        => await _lastServiceStatusRepository.GetServiceStatus(serviceName);
}