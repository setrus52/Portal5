using System.Data.SqlTypes;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class PositionSyncService(
    ILogger<DepartmentSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmPositionNormalizer positionNormalizer,
    IPositionRepository positionRepository
) : IPositionSyncService
{
    private readonly ILogger<DepartmentSyncService> _logger = logger;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;
    private readonly IIdmPositionNormalizer _positionNormalizer = positionNormalizer;
    private readonly IPositionRepository _positionRepository = positionRepository;

    public async Task<List<Position>> SyncAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Получаем должности из IDM
            var extPositions = await _idmLoadingService
                .LoadPositionsAsync(cancellationToken);

            // 2. Нормализуем данные IDM:
            //    string guid -> Guid
            //    string name -> нормализованное имя
            var positions = await _positionNormalizer
                .NormalizeAsync(extPositions, cancellationToken);

            // 3. Получаем существующие должности из БД
            var dbPositionsByGuid = await _positionRepository
                .QueryTracking()
                .ToDictionaryAsync(x => x.Guid, cancellationToken);

            var added = 0;
            var updated = 0;
            foreach (var dto in positions)
            {
                if (dbPositionsByGuid.TryGetValue(dto.Guid, out var position))
                {
                    UpdatePosition(position, dto);
                    updated++;
                }
                else
                {
                    _positionRepository.Add(CreatePosition(dto));
                }
            }

            _logger.LogInformation("Должности. Добавлено: {Added}, обновлено: {Updated}", added, updated);
            _logger.LogInformation("Синхронизация должностей завершена. Получено из IDM: {Count}", positions.Count);

            return dbPositionsByGuid.Values.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при синхронизации должностей");
            throw;
        }
    }

    private static Position CreatePosition(PositionDto dto)
        => new()
        {
            Guid = dto.Guid,
            Name = dto.Name,
            Order = dto.Order
        };


    private static void UpdatePosition(Position position, PositionDto dto)
    {
        position.Guid = dto.Guid;
        position.Name = dto.Name;
        position.Order = dto.Order;
    }
}