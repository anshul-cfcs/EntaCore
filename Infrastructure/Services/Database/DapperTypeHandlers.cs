using Dapper;
using System.Data;

namespace Infrastructure.Services.Database
{
    public static class DapperTypeHandlers
    {
        private static bool _isRegistered;

        public static void Register()
        {
            if (_isRegistered) return;

            SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            _isRegistered = true;
        }
    }

    public sealed class TimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
    {
        public override void SetValue(IDbDataParameter parameter, TimeOnly value)
        {
            parameter.Value = value.ToTimeSpan();
            parameter.DbType = DbType.Time;
        }

        public override TimeOnly Parse(object value)
        {
            return value switch
            {
                TimeOnly timeOnly => timeOnly,
                TimeSpan timeSpan => TimeOnly.FromTimeSpan(timeSpan),
                string text => TimeOnly.Parse(text),
                _ => throw new DataException($"Cannot convert {value.GetType()} to TimeOnly.")
            };
        }
    }

    public sealed class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
            parameter.DbType = DbType.Date;
        }

        public override DateOnly Parse(object value)
        {
            return value switch
            {
                DateOnly dateOnly => dateOnly,
                DateTime dateTime => DateOnly.FromDateTime(dateTime),
                string text => DateOnly.Parse(text),
                _ => throw new DataException($"Cannot convert {value.GetType()} to DateOnly.")
            };
        }
    }
}
