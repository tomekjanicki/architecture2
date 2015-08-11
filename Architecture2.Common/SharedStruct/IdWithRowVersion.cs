using MediatR;

namespace Architecture2.Common.SharedStruct
{
    public class IdWithRowVersion: INotification
    {
        public int? Id { get; set; }

        public byte[] Version { get; set; }

    }
}
