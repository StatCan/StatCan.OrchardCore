using System.Collections.Generic;
using StatCan.OrchardCore.Radar.Helpers.ValueConverters;

namespace StatCan.OrchardCore.Radar.Services
{
    public class RawValueConverterProvider
    {
        private IDictionary<string, IRawValueConverter> _converters;

        public RawValueConverterProvider()
        {
            _converters = new Dictionary<string, IRawValueConverter>();

            _converters[nameof(ProjectRawValueConverter)] = null;
            _converters[nameof(TopicRawValueConverter)] = null;
            _converters[nameof(EventRawValueConverter)] = null;
            _converters[nameof(ProposalRawValueConverter)] = null;
            _converters[nameof(CommunityRawValueConverter)] = null;
            _converters[nameof(ArtifactRawValueConverter)] = null;
        }

        public IRawValueConverter GetRawValueConverter<T>() where T : IRawValueConverter, new()
        {
            string typeName = typeof(T).Name;

            if (_converters[typeName] == null)
            {
                _converters[typeName] = new T();
            }

            return _converters[typeName];
        }
    }
}
