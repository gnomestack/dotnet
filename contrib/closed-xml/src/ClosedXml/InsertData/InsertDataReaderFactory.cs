using System.Collections;
using System.Data;

namespace GnomeStack.ClosedXml.InsertData;

internal class InsertDataReaderFactory
    {
        private static readonly Lazy<InsertDataReaderFactory> LazyInstance =
            new(() => new InsertDataReaderFactory());

        public static InsertDataReaderFactory Instance => LazyInstance.Value;

        public IInsertDataReader CreateReader<T>(IReadOnlyCollection<T> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var itemType = typeof(T);

            if (itemType == typeof(object))
                return new UntypedObjectReader(data);
            if (itemType.IsNullableType() && itemType.GetUnderlyingType().IsSimpleType())
                return new SimpleNullableTypeReader(data);
            if (itemType.IsSimpleType())
                return new SimpleTypeReader(data);
            if (typeof(IDataRecord).IsAssignableFrom(itemType))
                return new DataRecordReader(data.OfType<IDataRecord>());
            if (itemType.IsArray || typeof(IEnumerable).IsAssignableFrom(itemType))
                return new ArrayReader(data.Cast<IEnumerable>());
            if (itemType == typeof(DataRow))
                return new DataTableReader(data.Cast<DataRow>());

            return new ObjectReader(data);
        }

        public IInsertDataReader CreateReader<T>(IEnumerable<T> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var itemType = typeof(T);
            if (itemType == typeof(object))
                return new UntypedObjectReader(data);
            if (itemType.IsNullableType() && itemType.GetUnderlyingType().IsSimpleType())
                return new SimpleNullableTypeReader(data);
            if (itemType.IsSimpleType())
                return new SimpleTypeReader(data);
            if (typeof(IDataRecord).IsAssignableFrom(itemType))
                return new DataRecordReader(data.OfType<IDataRecord>());
            if (itemType.IsArray || typeof(IEnumerable).IsAssignableFrom(itemType))
                return new ArrayReader(data.Cast<IEnumerable>());
            if (itemType == typeof(DataRow))
                return new DataTableReader(data.Cast<DataRow>());

            return new ObjectReader(data);
        }

        public IInsertDataReader CreateReader(IEnumerable data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data is IEnumerable<IDataRecord> records)
                return new DataRecordReader(records);
            if (data is IEnumerable<DataRow> rows)
                return new DataTableReader(rows);

            var list = new List<object?>();
            foreach (var item in data)
            {
                list.Add(item);
            }

            var itemType = list.GetItemType();

            if (itemType == null || itemType == typeof(object))
                return new UntypedObjectReader(list);
            if (itemType.IsNullableType() && itemType.GetUnderlyingType().IsSimpleType())
                return new SimpleNullableTypeReader(list);
            if (itemType.IsSimpleType())
                return new SimpleTypeReader(list);
            if (typeof(IDataRecord).IsAssignableFrom(itemType))
                return new DataRecordReader(list.OfType<IDataRecord>());
            if (itemType.IsArray || typeof(IEnumerable).IsAssignableFrom(itemType))
                return new ArrayReader(list.Cast<IEnumerable>());
            if (itemType == typeof(DataRow))
                return new DataTableReader(list.Cast<DataRow>());

            return new ObjectReader(list);
        }

        public IInsertDataReader CreateReader<T>(IEnumerable<T[]> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            return new ArrayReader(data);
        }

        public IInsertDataReader CreateReader(IEnumerable<IEnumerable> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.GetType().GetElementType() == typeof(string))
                return new SimpleTypeReader(data);

            return new ArrayReader(data);
        }

        public IInsertDataReader CreateReader(DataTable dataTable)
        {
            if (dataTable == null) throw new ArgumentNullException(nameof(dataTable));

            return new DataTableReader(dataTable);
        }
    }