﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShardingCore.Core.EntityMetadatas;
using ShardingCore.Core.ShardingConfigurations;
using ShardingCore.Core.ShardingConfigurations.Abstractions;
using ShardingCore.Core.VirtualDatabase.VirtualDataSources.Abstractions;
using ShardingCore.Core.VirtualDatabase.VirtualDataSources.Common;
using ShardingCore.Exceptions;
using ShardingCore.Extensions;
using ShardingCore.Sharding.Abstractions;

namespace ShardingCore.Core.VirtualDatabase.VirtualDataSources
{
    public class VirtualDataSourceManager<TShardingDbContext> : IVirtualDataSourceManager<TShardingDbContext> where TShardingDbContext : DbContext, IShardingDbContext
    {
        private readonly IShardingConfigurationOptions<TShardingDbContext> _options;
        private readonly IEntityMetadataManager<TShardingDbContext> _entityMetadataManager;
        private readonly IVirtualDataSourceRouteManager<TShardingDbContext> _virtualDataSourceRouteManager;
        private readonly IVirtualDataSourceAccessor _virtualDataSourceAccessor;

        private readonly ConcurrentDictionary<string, IVirtualDataSource> _virtualDataSources = new(); 

        private  string _defaultConfigId;
        private  IVirtualDataSource _defaultVirtualDataSource;
        public VirtualDataSourceManager(IServiceProvider serviceProvider,IShardingConfigurationOptions<TShardingDbContext> options,IEntityMetadataManager<TShardingDbContext> entityMetadataManager,IVirtualDataSourceRouteManager<TShardingDbContext> virtualDataSourceRouteManager, IVirtualDataSourceAccessor virtualDataSourceAccessor)
        {

            _options = options;
            _entityMetadataManager = entityMetadataManager;
            _virtualDataSourceRouteManager = virtualDataSourceRouteManager;
            var allShardingGlobalConfigOptions = options.GetAllShardingGlobalConfigOptions();
            if (allShardingGlobalConfigOptions.IsEmpty())
                throw new ArgumentException($"sharding virtual data source is empty");
            _virtualDataSourceAccessor = virtualDataSourceAccessor;
            if (options is ShardingMultiConfigurationOptions<TShardingDbContext> shardingMultiConfigurationOptions)
            {
                IsMultiShardingConfiguration = true;
                ShardingConfigurationStrategy = shardingMultiConfigurationOptions.ShardingConfigurationStrategy;
            }
            else if (options is ShardingSingleConfigurationOptions<TShardingDbContext> shardingSingleConfigurationOptions)
            {
                IsMultiShardingConfiguration = false;
                ShardingConfigurationStrategy = shardingSingleConfigurationOptions.ShardingConfigurationStrategy;
            }
            foreach (var shardingGlobalConfigOption in allShardingGlobalConfigOptions)
            {
                var simpleVirtualDataSourceConfigurationParams = new SimpleVirtualDataSourceConfigurationParams<TShardingDbContext>(serviceProvider,shardingGlobalConfigOption);
                var dataSource = new VirtualDataSource<TShardingDbContext>(_entityMetadataManager,_virtualDataSourceRouteManager, simpleVirtualDataSourceConfigurationParams);
                dataSource.CheckVirtualDataSource();
                if (!IsMultiShardingConfiguration)
                {
                    if (_defaultVirtualDataSource != null || _defaultConfigId != null)
                        throw new ShardingCoreInvalidOperationException("set sharding configuration error");
                    _defaultVirtualDataSource = dataSource;
                    _defaultConfigId = dataSource.ConfigId;
                }
                _virtualDataSources.TryAdd(dataSource.ConfigId, dataSource);
            }

            if (IsMultiShardingConfiguration)
            {
                var maxShardingConfiguration = _virtualDataSources.Values.OrderByDescending(o => o.Priority).FirstOrDefault();
                _defaultVirtualDataSource = maxShardingConfiguration;
                _defaultConfigId = maxShardingConfiguration.ConfigId;
            }
        }
        public bool IsMultiShardingConfiguration { get; }
        public ShardingConfigurationStrategyEnum ShardingConfigurationStrategy { get; }
        public IVirtualDataSource GetVirtualDataSource()
        {
            if (!IsMultiShardingConfiguration)
                return _defaultVirtualDataSource;
            var configId = _virtualDataSourceAccessor.DataSourceContext?.ConfigId;
            if (!string.IsNullOrWhiteSpace(configId))
            {
                var hasValue = _virtualDataSources.TryGetValue(configId, out var virtualDataSource);
                if (hasValue)
                {
                    return virtualDataSource;
                }
            }

            switch (ShardingConfigurationStrategy)
            {
                case ShardingConfigurationStrategyEnum.ReturnNull: return null;
                case ShardingConfigurationStrategyEnum.ReturnHighPriority: return _defaultVirtualDataSource;
                case ShardingConfigurationStrategyEnum.ThrowIfNull: throw new ShardingCoreNotFoundException($"no configuration,config id:[{configId}]");
                default:
                    throw new ShardingCoreException(
                        $"unknown {nameof(ShardingConfigurationStrategyEnum)}:[{ShardingConfigurationStrategy}]");
            }
        }

        List<IVirtualDataSource<TShardingDbContext>> IVirtualDataSourceManager<TShardingDbContext>.GetAllVirtualDataSources()
        {
            return GetAllVirtualDataSources().Select(o => (IVirtualDataSource<TShardingDbContext>)o).ToList();
        }

        public void AddVirtualDataSource(IVirtualDataSourceConfigurationParams<TShardingDbContext> configurationParams)
        {
            if (!IsMultiShardingConfiguration)
                throw new NotSupportedException("not support multi sharding configuration");
            var dataSource = new VirtualDataSource<TShardingDbContext>(_entityMetadataManager, _virtualDataSourceRouteManager, configurationParams);
            dataSource.CheckVirtualDataSource();
            if(_virtualDataSources.TryAdd(dataSource.ConfigId, dataSource))
            {
                if (IsMultiShardingConfiguration)
                {
                    var maxShardingConfiguration = _virtualDataSources.Values.OrderByDescending(o => o.Priority).FirstOrDefault();
                    _defaultVirtualDataSource = maxShardingConfiguration;
                    _defaultConfigId = maxShardingConfiguration.ConfigId;
                }
            }
        }

        IVirtualDataSource<TShardingDbContext> IVirtualDataSourceManager<TShardingDbContext>.GetVirtualDataSource()
        {
            return (IVirtualDataSource<TShardingDbContext>)GetVirtualDataSource();
        }

        public List<IVirtualDataSource> GetAllVirtualDataSources()
        {
            if (!IsMultiShardingConfiguration)
                return new List<IVirtualDataSource>(1) { _defaultVirtualDataSource };
            return _virtualDataSources.Values.ToList();
        }

        public VirtualDataSourceScope CreateScope(string configId)
        {
            var virtualDataSourceScope = new VirtualDataSourceScope(_virtualDataSourceAccessor);
            _virtualDataSourceAccessor.DataSourceContext = new VirtualDataSourceContext(configId);
            return virtualDataSourceScope;
        }
    }
}
