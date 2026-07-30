using System.Data.SqlTypes;
using IDM.Application.Abstractions.Synchronization;
using IDM.Application.DTO;
using IDM.Application.Repositories;
using IDM.Application.Synchronization.Departments.Normalization;
using IDM.Application.Synchronization.Records;
using IDM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDM.Infrastructure.LoadingServices;

public class PositionSyncService(
    ILogger<PositionSyncService> logger,
    IIdmLoadingService idmLoadingService,
    IIdmNormalizer<PositionDto, ExtPositionDto> positionNormalizer,
    IPositionRepository positionRepository)
    : ISyncService<Position>
{
    private readonly ILogger<PositionSyncService> _logger = logger;
    private readonly IIdmLoadingService _idmLoadingService = idmLoadingService;
    private readonly IIdmNormalizer<PositionDto, ExtPositionDto> _positionNormalizer = positionNormalizer;
    private readonly IPositionRepository _positionRepository = positionRepository;

    public async Task<List<Position>> SyncAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Синхронизация должностей начата");

        try
        {
            // 1. Получаем должности из IDM
            var extPositions = await _idmLoadingService
                .LoadPositionsAsync(cancellationToken);

            // 2. Нормализуем
            var positions = await _positionNormalizer
                .NormalizeAsync(extPositions, cancellationToken);

            // 3. Получаем существующие должности
            var dbPositionsByGuid = await _positionRepository
                .QueryTracking()
                .ToDictionaryAsync(x => x.Guid, cancellationToken);

            var added = 0;
            var updated = 0;

            // 4. Добавляем новые и обновляем существующие
            foreach (var dto in positions)
            {
                if (dbPositionsByGuid.TryGetValue(dto.Guid, out var position))
                {
                    UpdatePosition(position, dto);
                    updated++;
                }
                else
                {
                    position = CreatePosition(dto);

                    _positionRepository.Add(position);

                    // Добавляем в словарь, чтобы вернуть актуальный список
                    dbPositionsByGuid.Add(position.Guid, position);

                    added++;
                }
            }

            _logger.LogInformation(
                "Должности. Добавлено: {Added}, обновлено: {Updated}",
                added,
                updated);

            _logger.LogInformation(
                "Синхронизация должностей завершена. Получено из IDM: {Count}",
                positions.Count);

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
        position.Name = dto.Name;
        position.Order = dto.Order;
    }
}