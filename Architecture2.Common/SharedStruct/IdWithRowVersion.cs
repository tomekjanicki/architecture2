
using Architecture2.Common.Handler.Interface;

namespace Architecture2.Common.SharedStruct
{
    public class IdWithRowVersion: IRequest
    {
        public int? Id { get; set; }

        public byte[] Version { get; set; }

    }
}
