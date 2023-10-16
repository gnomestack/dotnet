using System.Collections;

namespace GnomeStack.ClosedXml.InsertData;

internal class UntypedObjectReader : IInsertDataReader
    {
        private readonly IEnumerable<object?> data;
        private readonly IEnumerable<IInsertDataReader> readers;

        public UntypedObjectReader(IEnumerable? data)
        {
            this.data = data is null ? Array.Empty<object?>() : data.Cast<object?>();
            this.readers = CreateReaders().ToList();

            IEnumerable<IInsertDataReader> CreateReaders()
            {
                if (!this.data.Any())
                    yield break;

                var itemsOfSameType = new List<object?>();
                Type? previousType = null;

                foreach (var item in this.data)
                {
                    var currentType = item?.GetType();

                    if (previousType != currentType && itemsOfSameType.Count > 0)
                    {
                        yield return CreateReader(itemsOfSameType, previousType);
                        itemsOfSameType.Clear();
                    }

                    itemsOfSameType.Add(item);
                    previousType = currentType;
                }

                if (itemsOfSameType.Count > 0)
                {
                    yield return CreateReader(itemsOfSameType, previousType);
                }
            }

            IInsertDataReader CreateReader(List<object?> itemsOfSameType, Type? itemType)
            {
                if (itemType == null)
                    return new NullDataReader(itemsOfSameType);

                var items = Array.CreateInstance(itemType, itemsOfSameType.Count);
                Array.Copy(itemsOfSameType.ToArray(), items, items.Length);

                return InsertDataReaderFactory.Instance.CreateReader(items);
            }
        }

        public IEnumerable<IEnumerable<object?>> GetData()
        {
            foreach (var reader in this.readers)
            {
                foreach (var item in reader.GetData())
                {
                    yield return item;
                }
            }
        }

        public int GetPropertiesCount()
        {
            return this.GetFirstNonNullReader()?.GetPropertiesCount() ?? 0;
        }

        public string? GetPropertyName(int propertyIndex)
        {
            return this.GetFirstNonNullReader()?.GetPropertyName(propertyIndex);
        }

        public int GetRecordsCount()
        {
            return this.data.Count();
        }

        private IInsertDataReader? GetFirstNonNullReader()
        {
            return this.readers.FirstOrDefault(r => !(r is NullDataReader));
        }
    }