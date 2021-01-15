using System.Collections.Generic;

namespace Quan.ControlLibrary
{
    public class DbCustomerStore : ICusomerStore
    {
        /// <inheritdoc />
        public List<string> GetAll()
        {
            return new List<string>
            {
                "1",
                "2",
                "3"
            };
        }
    }
}