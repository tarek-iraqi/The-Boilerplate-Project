using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FirebaseSendMessageResultDTO
    {
        public bool is_success { get; set; }
        public string message_id { get; set; }
        public string error { get; set; }
    }

    public class FirebaseMultiDevicesSendMessageResultDTO
    {
        public int success_count { get; set; }
        public int failure_count { get; set; }
        public IEnumerable<FirebaseSendMessageResultDTO> send_result { get; set; }
    }
}
