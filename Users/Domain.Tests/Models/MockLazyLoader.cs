using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Users.Domain.Tests.Models
{
    internal class MockLazyLoader : ILazyLoader
    {
        private readonly IDictionary<string, object> _values;

        public MockLazyLoader(
            IDictionary<string, object> values)
        {
            _values = values;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SetLoaded(object entity, string navigationName = "", bool loaded = true)
        {
            throw new NotImplementedException();
        }

        public void Load(object entity, string navigationName = null)
        {
            entity.GetType().GetProperty(navigationName)?
                .SetValue(entity, _values[navigationName]);
        }

        public Task LoadAsync(object entity, CancellationToken cancellationToken = new CancellationToken(),
            string navigationName = null)
        {
            throw new NotImplementedException();
        }
    }
}
